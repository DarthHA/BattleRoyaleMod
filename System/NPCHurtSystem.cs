using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.System
{
    public class NPCHurtSystem : ModSystem
    {
        public override void Load()
        {
            On_NPC.FindFrame += AnotherMeleeHit;
            On_Projectile.Damage += NewProjDamage;
        }

        private void NewProjDamage(On_Projectile.orig_Damage orig, Projectile self)
        {
            orig.Invoke(self);

            if (!BattleRoyaleMod.BattleStart) return;

            if (!self.active || self.GetFraction() == -1)
            {
                return;
            }
            ShitCode.Damage(self);
        }

        private void AnotherMeleeHit(On_NPC.orig_FindFrame orig, NPC self)
        {
            if (!BattleRoyaleMod.BattleStart) goto FinalP;

            if (self.GetFraction() == -1)
            {
                goto FinalP;
            }
            if (self.dontTakeDamage || self.dontTakeDamageFromHostiles || self.immortal)
            {
                goto FinalP;
            }
            int specialHitSetter = 1;
            float damageMultiplier = 1f;
            /*
            if (self.immune[255] != 0)
            {
                goto FinalP;
            }
            */
            Rectangle hitbox = self.Hitbox;
            foreach (NPC enemy in Main.npc)
            {
                if (enemy.active && enemy.damage > 0 && enemy.GetFraction() != -1 && enemy.GetFraction() != self.GetFraction())
                {
                    if (self.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[enemy.GetFraction() - 1] > 0) continue;              //新无敌帧

                    if ((self.velocity.Length() > enemy.velocity.Length() && self.damage > 0) || self.IsNPCBullet())       //引入超速修正
                    {
                        if (enemy.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[self.GetFraction() - 1] == 0)
                        {
                            continue;
                        }
                    }

                    Rectangle npcRect = enemy.Hitbox;
                    NPC.GetMeleeCollisionData(hitbox, enemy.whoAmI, ref specialHitSetter, ref damageMultiplier, ref npcRect);
                    if (NPCLoader.CanHitNPC(enemy, self) && hitbox.Intersects(npcRect) && enemy.type != NPCID.Gnome)
                    {
                        self.BeHurtByOtherNPC(enemy);
                        goto FinalP;
                    }
                }
            }

        FinalP:
            orig.Invoke(self);
        }

        public override void Unload()
        {
            On_NPC.FindFrame -= AnotherMeleeHit;
            On_Projectile.Damage -= NewProjDamage;
        }

    }


    public class NPCImmuneSystem : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int[] ImmuneForEvE = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public override void PostAI(NPC npc)
        {
            for (int i = 0; i < ImmuneForEvE.Length; i++)
            {
                if (ImmuneForEvE[i] > 0)
                {
                    ImmuneForEvE[i]--;
                }
                else
                {
                    ImmuneForEvE[i] = 0;
                }
            }
        }
    }

    public static class ShitCode
    {
        public static void Damage(Projectile proj)
        {
            bool vanillaCanDamage = true;
            if (proj.type == 18 || proj.type == 72 || proj.type == 86 || proj.type == 87 || proj.aiStyle == 31 || proj.aiStyle == 32 || proj.type == 226 || proj.type == 378 || proj.type == 613 || proj.type == 650 || proj.type == 882 || proj.type == 888 || proj.type == 895 || proj.type == 896 || (proj.type == 434 && proj.localAI[0] != 0f) || proj.type == 439 || proj.type == 444 || (proj.type == 451 && ((int)(proj.ai[0] - 1f) / proj.penetrate == 0 || proj.ai[1] < 5f) && proj.ai[0] != 0f) || (proj.type == 500 || proj.type == 653 || proj.type == 1018 || proj.type == 460 || proj.type == 633 || proj.type == 600 || proj.type == 601 || proj.type == 602 || proj.type == 535 || (proj.type == 631 && proj.localAI[1] == 0f) || (proj.type == 537 && proj.localAI[0] <= 30f) || proj.type == 651 || (proj.type == 188 && proj.localAI[0] < 5f) || (proj.aiStyle == 137 && proj.ai[0] != 0f) || proj.aiStyle == 138 || (proj.type == 261 && proj.velocity.Length() < 1.5f) || (proj.type == 818 && proj.ai[0] < 1f) || proj.type == 831 || proj.type == 970 || (proj.type == 833 && proj.ai[0] == 4f) || (proj.type == 834 && proj.ai[0] == 4f) || (proj.type == 835 && proj.ai[0] == 4f) || (proj.type == 281 && proj.ai[0] == -3f) || ((proj.type == 598 || proj.type == 636 || proj.type == 614 || proj.type == 971 || proj.type == 975) && proj.ai[0] == 1f)) || (proj.type == 923 && proj.localAI[0] <= 60f) || (proj.type == 919 && proj.localAI[0] <= 60f) || (proj.aiStyle == 15 && proj.ai[0] == 0f && proj.localAI[1] <= 12f) || (proj.type == 861 || (proj.type >= 511 && proj.type <= 513 && proj.ai[1] >= 1f)) || (proj.type == 1007 || (proj.aiStyle == 93 && proj.ai[0] != 0f && proj.ai[0] != 2f)) || (proj.aiStyle == 10 && proj.localAI[1] == -1f) || (proj.type == 85 && proj.localAI[0] >= 54f) || (Main.projPet[proj.type] && proj.type != 266 && proj.type != 407 && proj.type != 317 && (proj.type != 388 || proj.ai[0] != 2f) && (proj.type < 390 || proj.type > 392) && (proj.type < 393 || proj.type > 395) && (proj.type != 533 || proj.ai[0] < 6f || proj.ai[0] > 8f) && (proj.type < 625 || proj.type > 628) && (proj.type != 755 || proj.ai[0] == 0f) && (proj.type != 946 || proj.ai[0] == 0f) && proj.type != 758 && proj.type != 951 && proj.type != 963 && (proj.type != 759 || proj.frame == Main.projFrames[proj.type] - 1) && proj.type != 833 && proj.type != 834 && proj.type != 835 && proj.type != 864 && (proj.type != 623 || proj.ai[0] != 2f)))
            {
                vanillaCanDamage = false;
            }
            if (Main.projPet[proj.type] && ProjectileLoader.MinionContactDamage(proj))
            {
                vanillaCanDamage = true;
            }
            if (!(ProjectileLoader.CanDamage(proj) ?? vanillaCanDamage))
            {
                return;
            }

            MethodInfo method = typeof(Projectile).GetMethod("Damage_GetHitbox", BindingFlags.NonPublic | BindingFlags.Instance);
            Rectangle rectangle = (Rectangle)method.Invoke(proj, Array.Empty<object>());

            if (proj.damage > 0)
            {
                bool CanDamageForPenetrate = true;
                foreach (NPC target in Main.npc)
                {
                    if (!CanDamageForPenetrate) break;
                    if (target.active && target.GetFraction() != -1 && target.GetFraction() != proj.GetFraction())
                    {
                        //bool ImmunityCanHit = (!proj.usesLocalNPCImmunity && !proj.usesIDStaticNPCImmunity) || (proj.usesLocalNPCImmunity && proj.localNPCImmunity[target.whoAmI] == 0) || (proj.usesIDStaticNPCImmunity && Projectile.IsNPCIndexImmuneToProjectileType(proj.type, target.whoAmI));

                        if (!target.dontTakeDamage && (target.aiStyle != 112 || target.ai[2] <= 1f))
                        {
                            bool canHitFlag = true;

                            bool NoNeedForImmunity = proj.maxPenetrate == 1;//&& !proj.usesLocalNPCImmunity && !proj.usesIDStaticNPCImmunity;
                            if (target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] == 0 || NoNeedForImmunity)
                            {
                                bool ForTrapImmune = true;
                                if (target.trapImmune && proj.trap)
                                {
                                    ForTrapImmune = false;
                                }
                                if (canHitFlag)
                                {
                                    ForTrapImmune = true;
                                }
                                if (ForTrapImmune)
                                {
                                    bool colliding;
                                    if (target.type == NPCID.SolarCrawltipedeTail)
                                    {
                                        Rectangle rect = target.getRect();
                                        rect.X -= 8;
                                        rect.Y -= 8;
                                        rect.Width += 16;
                                        rect.Height += 16;
                                        colliding = proj.Colliding(rectangle, rect);
                                    }
                                    else
                                    {
                                        colliding = proj.Colliding(rectangle, target.getRect());
                                    }

                                    if (colliding)
                                    {
                                        NPC.HitModifiers modifiers = target.GetIncomingStrikeModifiers(proj.DamageType, proj.direction, false);
                                        modifiers.ArmorPenetration += proj.ArmorPenetration;
                                        CombinedHooks.ModifyHitNPCWithProj(proj, target, ref modifiers);
                                        float kb = proj.knockBack;
                                        float armorPenetrationPercent = 0f;

                                        float num18 = 1000f;
                                        if (proj.type == 1002)
                                        {
                                            num18 /= 2f;
                                        }
                                        if (proj.trap && NPCID.Sets.BelongsToInvasionOldOnesArmy[target.type])
                                        {
                                            num18 /= 2f;
                                        }
                                        if (proj.type == 482 && (target.aiStyle == 6 || target.aiStyle == 37))
                                        {
                                            num18 /= 2f;
                                        }
                                        if ((proj.type == 400 || proj.type == 401 || proj.type == 402) && target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                                        {
                                            num18 = (int)((double)num18 * 0.65);
                                            if (proj.penetrate > 1)
                                            {
                                                proj.penetrate--;
                                            }
                                        }
                                        if (proj.type == 504 || proj.type == 954 || proj.type == 979)
                                        {
                                            float num19 = (60f - proj.ai[0]) / 2f;
                                            proj.ai[0] += num19;
                                        }
                                        else if (proj.type == 582 || proj.type == 902)
                                        {
                                            if (proj.ai[0] != 0f)
                                            {
                                                proj.direction *= -1;
                                            }
                                        }
                                        else if (proj.type == 624)
                                        {
                                            float num20 = 1f;
                                            if (target.knockBackResist > 0f)
                                            {
                                                num20 = 1f / target.knockBackResist;
                                            }
                                            proj.knockBack = 4f * num20;
                                            kb = proj.knockBack;
                                            if (target.Center.X < proj.Center.X)
                                            {
                                                proj.direction = 1;
                                            }
                                            else
                                            {
                                                proj.direction = -1;
                                            }
                                        }
                                        else if (proj.aiStyle == 16)
                                        {
                                            if (proj.timeLeft > 3)
                                            {
                                                proj.timeLeft = 3;
                                            }
                                            if (target.position.X + target.width / 2 < proj.position.X + proj.width / 2)
                                            {
                                                proj.direction = -1;
                                            }
                                            else
                                            {
                                                proj.direction = 1;
                                            }
                                        }
                                        else if (proj.aiStyle == 68)
                                        {
                                            if (proj.timeLeft > 3)
                                            {
                                                proj.timeLeft = 3;
                                            }
                                            if (target.position.X + target.width / 2 < proj.position.X + proj.width / 2)
                                            {
                                                proj.direction = -1;
                                            }
                                            else
                                            {
                                                proj.direction = 1;
                                            }
                                        }
                                        else if (proj.aiStyle == 50)
                                        {
                                            if (target.position.X + target.width / 2 < proj.position.X + proj.width / 2)
                                            {
                                                proj.direction = -1;
                                            }
                                            else
                                            {
                                                proj.direction = 1;
                                            }
                                        }
                                        else if (proj.type == 908)
                                        {
                                            if (target.position.X + target.width / 2 < proj.position.X + proj.width / 2)
                                            {
                                                proj.direction = -1;
                                            }
                                            else
                                            {
                                                proj.direction = 1;
                                            }
                                        }

                                        if (proj.type == 598 || proj.type == 636 || proj.type == 614 || proj.type == 971 || proj.type == 975)
                                        {
                                            proj.ai[0] = 1f;
                                            proj.ai[1] = target.whoAmI;
                                            proj.velocity = (target.Center - proj.Center) * 0.75f;

                                        }
                                        if (proj.type >= 511 && proj.type <= 513)
                                        {
                                            proj.ai[1] += 1f;

                                        }
                                        if (proj.type == 659)
                                        {
                                            proj.timeLeft = 0;
                                        }
                                        if (proj.type == 524)
                                        {

                                            proj.ai[0] += 50f;
                                        }
                                        if ((proj.type == 688 || proj.type == 689 || proj.type == 690) && target.type != NPCID.DungeonGuardian && target.defense < 999)
                                        {
                                            armorPenetrationPercent = 1f;
                                        }

                                        if (proj.type == 41 && proj.timeLeft > 1)
                                        {
                                            proj.timeLeft = 1;
                                        }

                                        if (proj.aiStyle == 93)
                                        {
                                            if (proj.ai[0] == 0f)
                                            {
                                                proj.ai[1] = 0f;
                                                int num24 = -target.whoAmI - 1;
                                                proj.ai[0] = num24;
                                                proj.velocity = target.Center - proj.Center;
                                            }
                                            num18 = (proj.ai[0] != 2f) ? ((int)((double)num18 * 0.15)) : ((int)((double)num18 * 1.35));
                                        }
                                        if (Main.expertMode)
                                        {
                                            if ((proj.type == 30 || proj.type == 397 || proj.type == 517 || proj.type == 28 || proj.type == 37 || proj.type == 516 || proj.type == 29 || proj.type == 470 || proj.type == 637 || proj.type == 108 || proj.type == 281 || proj.type == 588 || proj.type == 519 || proj.type == 773 || proj.type == 183 || proj.type == 181 || proj.type == 566 || proj.type == 1002) && target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                                            {
                                                num18 /= 5f;
                                            }
                                            if (proj.type == 280 && ((target.type >= NPCID.TheDestroyer && target.type <= NPCID.TheDestroyerTail) || target.type == NPCID.Probe))
                                            {
                                                num18 = (int)((double)num18 * 0.75);
                                            }
                                        }
                                        if (Main.netMode != NetmodeID.Server && target.type == NPCID.CultistBoss && proj.type >= 0 && ProjectileID.Sets.CultistIsResistantTo[proj.type])
                                        {
                                            num18 = (int)(num18 * 0.75f);
                                        }
                                        if (proj.type == 497 && proj.penetrate != 1)
                                        {
                                            proj.ai[0] = 25f;
                                            float num26 = proj.velocity.Length();
                                            Vector2 vector5 = target.Center - proj.Center;
                                            vector5.Normalize();
                                            vector5 *= num26;
                                            proj.velocity = -vector5 * 0.9f;

                                        }
                                        if (proj.type == 323 && (target.type == NPCID.Vampire || target.type == NPCID.VampireBat))
                                        {
                                            num18 *= 10f;
                                        }
                                        if (proj.type == 981 && target.type == NPCID.Werewolf)
                                        {
                                            num18 *= 3f;
                                        }
                                        if (proj.type == 261 && proj.velocity.Length() < 3.5f)
                                        {
                                            modifiers.SourceDamage /= 2f;
                                            kb /= 2f;
                                        }
                                        int? HitDir = null;

                                        if (proj.type >= 700 && proj.type <= 708 || proj.type == 759)
                                        {
                                            HitDir = new int?((proj.Center.X < target.Center.X) ? 1 : -1);
                                        }
                                        modifiers.ScalingArmorPenetration += armorPenetrationPercent;
                                        modifiers.Knockback *= kb / proj.knockBack;
                                        modifiers.TargetDamageMultiplier *= num18 / 1000f;
                                        if (HitDir != null)
                                        {
                                            modifiers.HitDirectionOverride = HitDir;
                                        }

                                        float basedmg = proj.damage;
                                        basedmg = RealProjectileDamage(proj.damage, BattleRoyaleMod.Gconfig.ProjDamageModifier);        //敌怪弹幕伤害修正/修改

                                        NPC.HitInfo strike = modifiers.ToHitInfo(basedmg, false, kb, true, 0f);
                                        HitDir = new int?(strike.HitDirection);
                                        if (proj.type == 294)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 265)
                                        {
                                            proj.damage = (int)(proj.damage * 0.75);
                                        }
                                        if (proj.type == 355)
                                        {
                                            proj.damage = (int)(proj.damage * 0.75);
                                        }
                                        if (proj.type == 114)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 76 || proj.type == 78 || proj.type == 77)
                                        {
                                            proj.damage = (int)(proj.damage * 0.95);
                                        }
                                        if (proj.type == 85)
                                        {
                                            proj.damage = (int)(proj.damage * 0.85);
                                        }
                                        if (proj.type == 866)
                                        {
                                            proj.damage = (int)(proj.damage * 0.8);
                                        }
                                        if (proj.type == 841)
                                        {
                                            proj.damage = (int)(proj.damage * 0.5);
                                        }
                                        if (proj.type == 914)
                                        {
                                            proj.damage = (int)(proj.damage * 0.6);
                                        }
                                        if (proj.type == 952)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 913)
                                        {
                                            proj.damage = (int)(proj.damage * 0.66);
                                        }
                                        if (proj.type == 912)
                                        {
                                            proj.damage = (int)(proj.damage * 0.7);
                                        }
                                        if (proj.type == 847)
                                        {
                                            proj.damage = (int)(proj.damage * 0.8);
                                        }
                                        if (proj.type == 848)
                                        {
                                            proj.damage = (int)(proj.damage * 0.95);
                                        }
                                        if (proj.type == 849)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 915)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 931)
                                        {
                                            proj.damage = (int)(proj.damage * 0.8);
                                        }
                                        if (proj.type == 242)
                                        {
                                            proj.damage = (int)(proj.damage * 0.85);
                                        }
                                        if (proj.type == 323)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 5)
                                        {
                                            proj.damage = (int)(proj.damage * 0.9);
                                        }
                                        if (proj.type == 4)
                                        {
                                            proj.damage = (int)(proj.damage * 0.95);
                                        }
                                        if (proj.type == 309)
                                        {
                                            proj.damage = (int)(proj.damage * 0.85);
                                        }
                                        if (proj.type == 132)
                                        {
                                            proj.damage = (int)(proj.damage * 0.85);
                                        }
                                        if (proj.type == 985)
                                        {
                                            proj.damage = (int)(proj.damage * 0.75);
                                        }
                                        if (proj.type == 950)
                                        {
                                            proj.damage = (int)(proj.damage * 0.98);
                                        }
                                        if (proj.type == 964)
                                        {
                                            proj.damage = (int)(proj.damage * 0.85);
                                        }

                                        proj.StatusNPC(target.whoAmI);
                                        if (ProjectileID.Sets.ImmediatelyUpdatesNPCBuffFlags[proj.type])
                                        {
                                            target.UpdateNPC_BuffSetFlags(false);
                                        }
                                        if (proj.type == 317)
                                        {
                                            proj.ai[1] = -1f;
                                        }

                                        int strikedmg = target.StrikeNPC(strike, false, true);

                                        if (target.immortal && proj.active && proj.timeLeft > 10 && target.active && target.type == 676 && proj.CanBeReflected())
                                        {
                                            //反弹射弹可以做
                                            ReflectProjectile(proj, target);
                                            proj.penetrate++;
                                        }

                                        /*
                                        if (proj.usesIDStaticNPCImmunity)
                                        {
                                            if (proj.penetrate != 1 || proj.appliesImmunityTimeOnSingleHits)
                                            {
                                                target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                                Projectile.perIDStaticNPCImmunity[proj.type][target.whoAmI] = Main.GameUpdateCount + (uint)proj.idStaticNPCHitCooldown;
                                            }
                                        }
                                        */

                                        else if (proj.type == 434)
                                        {
                                            proj.numUpdates = 0;
                                        }
                                        else if (proj.type == 632)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 514)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 1;
                                        }
                                        else if (proj.type == 595 || proj.type == 735)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 927)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 4;
                                        }
                                        else if (proj.type == 286)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 443)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 8;
                                        }
                                        else if (proj.type >= 424 && proj.type <= 426)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 634 || proj.type == 635)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 659)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 5;
                                        }
                                        else if (proj.type == 246)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 7;
                                        }
                                        else if (proj.type == 249)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 7;
                                        }
                                        else if (proj.type == 16)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 8;
                                        }
                                        else if (proj.type == 409)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 6;
                                        }
                                        else if (proj.type == 311)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 7;
                                        }
                                        else if (proj.type == 582 || proj.type == 902)
                                        {
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 7;
                                            if (proj.ai[0] != 1f)
                                            {
                                                proj.ai[0] = 1f;

                                            }
                                        }
                                        else
                                        {
                                            if (proj.type == 451)
                                            {
                                                if (proj.ai[0] == 0f)
                                                {
                                                    proj.ai[0] += proj.penetrate;
                                                }
                                                else
                                                {
                                                    proj.ai[0] -= proj.penetrate + 1;
                                                }
                                                proj.ai[1] = 0f;

                                                target.position -= target.netOffset;
                                                break;
                                            }
                                            if (proj.type == 864)
                                            {
                                                /*
                                                proj.localNPCImmunity[target.whoAmI] = 10;
                                                target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                                */
                                                if (proj.ai[0] > 0f)
                                                {
                                                    proj.ai[0] = -1f;
                                                    proj.ai[1] = 0f;
                                                }
                                            }
                                            else if (proj.type == 661 || proj.type == 856)
                                            {
                                                proj.localNPCImmunity[target.whoAmI] = 8;
                                                target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            }
                                            /*
                                            else if (proj.usesLocalNPCImmunity && proj.localNPCHitCooldown != -2)
                                            {
                                                target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                                proj.localNPCImmunity[target.whoAmI] = proj.localNPCHitCooldown;
                                            }
                                            */
                                            else if (proj.penetrate != 1 || proj.appliesImmunityTimeOnSingleHits)
                                            {
                                                target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 10;
                                            }
                                        }

                                        CombinedHooks.OnHitNPCWithProj(proj, target, strike, strikedmg);

                                        //穿透相关
                                        if (proj.penetrate > 0 && proj.type != 317 && proj.type != 866)
                                        {
                                            if (proj.type == 357)
                                            {
                                                proj.damage = (int)(proj.damage * 0.8);
                                            }
                                            proj.penetrate--;
                                            if (proj.penetrate == 0)
                                            {
                                                target.position -= target.netOffset;
                                                if (proj.stopsDealingDamageAfterPenetrateHits)
                                                {
                                                    proj.penetrate = -1;
                                                    proj.damage = 0;
                                                }
                                                CanDamageForPenetrate = false;
                                            }
                                        }
                                        if (proj.aiStyle == 7)
                                        {
                                            proj.ai[0] = 1f;
                                            proj.damage = 0;
                                        }
                                        else if (proj.aiStyle == 13)
                                        {
                                            proj.ai[0] = 1f;

                                        }
                                        else if (proj.aiStyle == 69)
                                        {
                                            proj.ai[0] = 1f;

                                        }
                                        else if (proj.type == 607)
                                        {
                                            proj.ai[0] = 1f;

                                            proj.friendly = false;
                                        }
                                        else if (proj.type == 638 || proj.type == 639 || proj.type == 640)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = -1;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                            proj.damage = (int)(proj.damage * 0.96);
                                        }
                                        else if (proj.type == 617)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 8;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                        }
                                        else if (proj.type == 656)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 8;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                            proj.localAI[0] += 1f;
                                        }
                                        else if (proj.type == 618)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 20;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                        }
                                        else if (proj.type == 642)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 10;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                        }
                                        else if (proj.type == 857)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 10;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                        }
                                        else if (proj.type == 611 || proj.type == 612)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 6;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                        }
                                        else if (proj.type == 645)
                                        {
                                            /*
                                            proj.localNPCImmunity[target.whoAmI] = 0;
                                            target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] = 0;
                                            */
                                            if (proj.ai[1] != -1f)
                                            {
                                                proj.ai[0] = 0f;
                                                proj.ai[1] = -1f;

                                            }
                                        }
                                        if (target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1] > 0)
                                        {
                                            if (BattleRoyaleMod.Gconfig.AllSegmentUseSameImmune)
                                            {
                                                GiveAllSegmentsImmune(target, proj.GetFraction(), target.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[proj.GetFraction() - 1]);
                                            }
                                        }

                                        proj.numHits++;
                                        if (proj.type == 697)
                                        {
                                            if (proj.ai[0] >= 42f)
                                            {
                                                proj.localAI[1] = 1f;
                                            }
                                        }
                                        else if (proj.type == 706)
                                        {
                                            proj.damage = (int)(proj.damage * 0.95f);
                                        }
                                        else if (proj.type == 34)
                                        {
                                            if (proj.ai[0] == -1f)
                                            {
                                                proj.ai[1] = -1f;

                                            }
                                        }
                                        else if (proj.type == 79)
                                        {
                                            if (proj.ai[0] == -1f)
                                            {
                                                proj.ai[1] = -1f;

                                            }
                                            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings
                                            {
                                                PositionInWorld = target.Center,
                                                MovementVector = proj.velocity
                                            }, null);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

            }
        }

        public static void ReflectProjectile(Projectile proj, NPC target)
        {
            SoundEngine.PlaySound(SoundID.Item150, new Vector2?(proj.position), null);
            for (int i = 0; i < 3; i++)
            {
                int num = Dust.NewDust(proj.position, proj.width, proj.height, 31, 0f, 0f, 0, default, 1f);
                Main.dust[num].velocity *= 0.3f;
            }
            proj.reflected = true;
            Vector2 vector;
            if (proj.GetSource() != -1 && Main.npc[proj.GetSource()].active)
            {
                vector = Vector2.Normalize(Main.npc[proj.GetSource()].Center - proj.Center);
            }
            else
            {
                vector = Vector2.Normalize(-proj.velocity);
            }
            proj.GetGlobalProjectile<SourceMarkProj>().Source = target.whoAmI;
            proj.GetGlobalProjectile<SourceMarkProj>().Fraction = target.GetFraction();
            vector *= proj.oldVelocity.Length();
            proj.velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
            proj.velocity.Normalize();
            proj.velocity *= vector.Length();
            proj.velocity += vector * 20f;
            proj.velocity.Normalize();
            proj.velocity *= vector.Length();
            proj.penetrate = 1;
        }


        public static void BeHurtByOtherNPC(this NPC self, NPC thatNPC)
        {
            int immune = 30;
            if (self.type == NPCID.DD2EterniaCrystal)
            {
                immune = 20;
            }
            int basekb = 6;
            int hitDirection = (thatNPC.Center.X <= self.Center.X) ? 1 : -1;
            NPC.HitModifiers modifiers = self.GetIncomingStrikeModifiers(DamageClass.Default, hitDirection, false);
            NPCLoader.ModifyHitNPC(thatNPC, self, ref modifiers);

            float basedmg = thatNPC.damage;
            basedmg *= BattleRoyaleMod.Gconfig.MeleeDamageModifier;

            NPC.HitInfo strike = modifiers.ToHitInfo(basedmg, false, basekb, true, 0f);
            self.StrikeNPC(strike, false, true);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendStrikeNPC(self, strike, -1);
            }
            self.netUpdate = true;
            self.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[thatNPC.GetFraction() - 1] = immune;
            if (BattleRoyaleMod.Gconfig.AllSegmentUseSameImmune)
            {
                GiveAllSegmentsImmune(self, thatNPC.GetFraction(), immune);
            }
            NPCLoader.OnHitNPC(thatNPC, self, strike);

            /*
            int num5;
            if (self.dryadWard)
            {
                num5 = (int)num4 / 3;
                basekb = 6;
                hitDirection *= -1;
                thatNPC.StrikeNPC(self.CalculateStrikeFromLegacyValues(num5, basekb, hitDirection, false), false, true);

                thatNPC.netUpdate = true;
                thatNPC.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[self.GetFraction() - 1] = immune;
            }
            
            if (NPCID.Sets.HurtingBees[thatNPC.type])
            {
                num5 = self.damage;
                basekb = 6;
                hitDirection *= -1;
                thatNPC.StrikeNPC(self.CalculateStrikeFromLegacyValues(num5, basekb, hitDirection, false), false, true);

                thatNPC.netUpdate = true;
                thatNPC.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[self.GetFraction() - 1] = immune;
            }
            */
        }


        internal static NPC.HitInfo CalculateStrikeFromLegacyValues(this NPC self, int Damage, float knockBack, int hitDirection, bool crit)
        {
            NPC.HitInfo hit;
            if (Damage == 9999)
            {
                hit = new NPC.HitInfo
                {
                    Crit = crit,
                    Knockback = knockBack,
                    HitDirection = hitDirection,
                    InstantKill = true
                };
            }
            else
            {
                hit = self.GetIncomingStrikeModifiers(DamageClass.Default, hitDirection, true).ToHitInfo(Damage, crit, knockBack, false, 0f);
            }
            return hit;
        }

        public static float ProjWorldDamage => Main.GameModeInfo.IsJourneyMode
    ? CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>().StrengthMultiplierToGiveNPCs
    : Main.GameModeInfo.EnemyDamageMultiplier;

        public static int RealProjectileDamage(int ProjDamage, float modifier = 1)
        {
            const float inherentHostileProjMultiplier = 2;
            return (int)(ProjDamage * inherentHostileProjMultiplier * ProjWorldDamage * modifier);
        }



        public static void GiveAllSegmentsImmune(NPC npc, int Fraction, int immune)
        {
            int owner = npc.whoAmI;
            if (npc.realLife != -1)
            {
                owner = npc.realLife;
            }
            foreach (NPC others in Main.npc)
            {
                if ((others.whoAmI == owner || others.realLife == owner) && others.whoAmI != npc.whoAmI)
                {
                    if (others.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[Fraction - 1] < immune)
                        others.GetGlobalNPC<NPCImmuneSystem>().ImmuneForEvE[Fraction - 1] = immune;
                }
            }
        }
    }
}
