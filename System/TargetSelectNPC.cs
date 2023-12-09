
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.System
{

    public class TargetSelectNPC : GlobalNPC
    {
        public static Vector2? OldPos = null;
        public static Vector2? OldVel = null;


        public override void Load()
        {
            //MethodBase source = typeof(NPC).GetMethod("AI", BindingFlags.Public | BindingFlags.Instance);
            //MethodInfo instead = GetType().GetMethod("HasArmorSet", BindingFlags.Public | BindingFlags.Static);
            //new Hook(source, instead).Apply();
        }



        public override bool PreAI(NPC npc)
        {
            if (BattleRoyaleMod.Gconfig.TakeEffectInstantly)
            {
                FakeEventHandler.SetEvent(npc);
            }

            if (!BattleRoyaleMod.BattleStart)
            {
                return true;
            }

            if (npc.GetFraction() == -1)
            {
                return true;
            }

            if (!BattleRoyaleMod.Gconfig.TakeEffectInstantly)
            {
                FakeEventHandler.SetEvent(npc);
            }


            List<int> enemy = new();
            foreach (NPC target in Main.npc)
            {
                if (target.active && !target.dontTakeDamage && target.GetFraction() != -1 && target.GetFraction() != npc.GetFraction())         //参与EvE的NPC中
                {
                    enemy.Add(target.whoAmI);
                }
            }

            if (enemy.Count == 0)
            {
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.GetFraction() != -1 && target.GetFraction() != npc.GetFraction())         //找不到可以打的npc就锁定无敌的npc
                    {
                        enemy.Add(target.whoAmI);
                    }
                }
            }

            if (enemy.Count == 0)
            {
                return true;
            }
            float SavedDist = 114514;
            int RealTarget = -1;

            foreach (int t in enemy)
            {
                NPC target = Main.npc[t];
                float RealDist = target.Distance(npc.Center);
                float ModifiedDist = RealDist;
                if (target.GetSource() == -1)
                {
                    ModifiedDist -= BattleRoyaleMod.Gconfig.SourceNPCAggro;
                }
                if (RealTarget == -1 || ModifiedDist < SavedDist)
                {
                    RealTarget = t;
                    SavedDist = ModifiedDist;
                }
            }
            if (RealTarget == -1)
            {
                return true;
            }
            OldPos = Main.LocalPlayer.Center;
            OldVel = Main.LocalPlayer.velocity;
            Main.LocalPlayer.Center = Main.npc[RealTarget].Center;
            Main.LocalPlayer.velocity = Main.npc[RealTarget].velocity;
            SafePlayer.FuckingInvincible = true;
            SlimeFix(npc);
            return true;
        }

        public override void PostAI(NPC npc)
        {
            FakeEventHandler.ResetEvent();
            if (OldPos.HasValue)
            {
                Main.LocalPlayer.Center = OldPos.Value;
                OldPos = null;
            }

            if (OldVel.HasValue)
            {
                Main.LocalPlayer.velocity = OldVel.Value;
                OldVel = null;
            }

            if (ConfigDataUtils.GetIfNotDespawn(npc))
            {
                if ((npc.GetFraction() != -1 || BattleRoyaleMod.Gconfig.TakeEffectInstantly) && !npc.IsNPCBullet())
                {
                    if (npc.timeLeft < NPC.activeTime)
                        npc.timeLeft = NPC.activeTime;
                    npc.despawnEncouraged = false;
                }
            }

            SafePlayer.FuckingInvincible = false;
        }

        public override bool CheckActive(NPC npc)
        {
            if (ConfigDataUtils.GetIfNotDespawn(npc))
            {
                if ((npc.GetFraction() != -1 || BattleRoyaleMod.Gconfig.TakeEffectInstantly) && !npc.IsNPCBullet())
                {
                    if (npc.timeLeft < NPC.activeTime)
                        npc.timeLeft = NPC.activeTime;
                    npc.despawnEncouraged = false;
                    return false;
                }
            }
            return true;
        }


        internal void SlimeFix(NPC npc)
        {
            if (npc.aiStyle == NPCAIStyleID.Slime)
            {
                Main.LocalPlayer.npcTypeNoAggro[npc.type] = false;
                npc.TargetClosest();
            }
        }

    }


    public class TargetSelectProj : GlobalProjectile
    {
        public static Vector2? OldPos = null;
        public override bool PreAI(Projectile projectile)
        {

            if (!BattleRoyaleMod.BattleStart)
            {
                return true;
            }

            if (projectile.GetFraction() == -1)
            {
                return true;
            }

            List<int> enemy = new();
            foreach (NPC target in Main.npc)
            {
                if (target.active && target.GetFraction() != -1 && target.GetFraction() != projectile.GetFraction())         //参与EvE的NPC中
                {
                    enemy.Add(target.whoAmI);
                }
            }
            if (enemy.Count == 0)
            {
                return true;
            }
            float dist = 114514;
            int RealTarget = -1;

            foreach (int t in enemy)
            {
                NPC target = Main.npc[t];
                if (RealTarget == -1 || target.Distance(projectile.Center) < dist)
                {
                    RealTarget = t;
                    dist = target.Distance(projectile.Center);
                }
            }
            if (RealTarget == -1)
            {
                return true;
            }
            OldPos = Main.LocalPlayer.Center;
            Main.LocalPlayer.Center = Main.npc[RealTarget].Center;
            SafePlayer.FuckingInvincible = true;
            return true;
        }
        public override void PostAI(Projectile projectile)
        {
            if (OldPos.HasValue)
            {
                Main.LocalPlayer.Center = OldPos.Value;
                OldPos = null;
            }
            SafePlayer.FuckingInvincible = false;
        }

    }


    public class SafePlayer : ModPlayer
    {
        public static bool FuckingInvincible = false;
        public override void Load()
        {
            On_Player.AddBuff += AddBuffHook;
        }
        public override void Unload()
        {
            On_Player.AddBuff -= AddBuffHook;
        }
        internal static void AddBuffHook(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
        {
            if (FuckingInvincible) return;
            orig.Invoke(self, type, timeToAdd, quiet, foodHack);

        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (FuckingInvincible) return false;
            return true;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (FuckingInvincible) return false;
            return true;
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (FuckingInvincible) return true;
            return false;
        }

        public override void UpdateDead()
        {
            if (BattleRoyaleMod.BattleStart)
            {
                if (Player.respawnTimer > 1)
                {
                    Player.respawnTimer = 1;
                }
            }

        }

        public override void OnEnterWorld()
        {
            BattleRoyaleMod.BattleStart = false;
            BattleRoyaleMod.TakeYourTime = 0;


            float WorldOffsetX = 40 * 2 + Main.screenWidth / 2 + 200;
            float WorldOffsetY = 40 * 2 + Main.screenHeight / 2 + 200;
            float Left = WorldOffsetX;
            float Right = Main.maxTilesX * 16 - WorldOffsetX;
            float Top = WorldOffsetY;
            float Bottom = Main.maxTilesY * 16 - WorldOffsetY;

            BattleRoyaleMod.TopLeft = new Vector2(Left, Top);
            BattleRoyaleMod.BottomRight = new Vector2(Right, Bottom);

            Main.NewText(Language.GetTextValue("Mods.BattleRoyaleMod.OpeningRemind"), Color.Cyan);
        }
    }

    public class SaveNPCVelocity : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Vector2? OldVel = null;
    }

    public class SaveProjVelocity : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public Vector2? OldVel = null;
    }
}
