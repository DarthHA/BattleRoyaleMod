using BattleRoyaleMod.Projectiles;
using BattleRoyaleMod.System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BattleRoyaleMod
{
    public class SourceMarkerSystem : ModSystem
    {
        public static int CurrentSource = -1;
        public static int CurrentFraction = -1;
        public static int SavedSource = -1;
        public static int SavedFraction = -1;

        public override void Load()
        {
            On_NPC.UpdateNPC += UpdateNPCHook;
            On_Projectile.Update += UpdateProjHook;
            On_NPC.checkDead += CheckDeadHook;
            On_Projectile.Kill += ProjKillHook;
            On_NPC.HitEffect_HitInfo += HitEffectHook;

            On_NPC.NewNPC += NewNPCHook;
            On_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += NewProjectileHook;

        }



        public override void Unload()
        {
            On_NPC.UpdateNPC -= UpdateNPCHook;
            On_Projectile.Update -= UpdateProjHook;
            On_NPC.checkDead -= CheckDeadHook;
            On_Projectile.Kill -= ProjKillHook;
            On_NPC.HitEffect_HitInfo -= HitEffectHook;

            On_NPC.NewNPC -= NewNPCHook;
            On_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float -= NewProjectileHook;

        }

        internal static void CheckDeadHook(On_NPC.orig_checkDead orig, NPC self)
        {
            SavedSource = CurrentSource;
            SavedFraction = CurrentFraction;

            int source = self.GetSource();
            if (source == -1)
            {
                CurrentSource = self.whoAmI;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    self.GetGlobalNPC<SourceMarkNPC>().Source = -1;
                    CurrentSource = -1;
                }
                else
                {
                    CurrentSource = source;
                }
            }
            CurrentFraction = self.GetFraction();
            orig.Invoke(self);
            CurrentSource = SavedSource;
            CurrentFraction = SavedFraction;
            SavedSource = -1;
            SavedFraction = -1;

        }

        private void HitEffectHook(On_NPC.orig_HitEffect_HitInfo orig, NPC self, NPC.HitInfo hit)
        {
            SavedSource = CurrentSource;
            SavedFraction = CurrentFraction;

            int source = self.GetSource();
            if (source == -1)
            {
                CurrentSource = self.whoAmI;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    self.GetGlobalNPC<SourceMarkNPC>().Source = -1;
                    CurrentSource = -1;
                }
                else
                {
                    CurrentSource = source;
                }
            }
            CurrentFraction = self.GetFraction();
            orig.Invoke(self, hit);
            CurrentSource = SavedSource;
            CurrentFraction = SavedFraction;
            SavedSource = -1;
            SavedFraction = -1;

        }


        internal static void ProjKillHook(On_Projectile.orig_Kill orig, Projectile self)
        {
            SavedSource = CurrentSource;
            SavedFraction = CurrentFraction;

            int source = self.GetSource();
            if (source != -1)
            {
                if (!Main.npc[source].active)
                {
                    self.GetGlobalProjectile<SourceMarkProj>().Source = -1;
                    CurrentSource = -1;
                }
                else
                {
                    CurrentSource = source;
                }
            }
            else
            {
                CurrentSource = -1;
            }
            CurrentFraction = self.GetFraction();
            orig.Invoke(self);
            CurrentSource = SavedSource;
            CurrentFraction = SavedFraction;
            SavedSource = -1;
            SavedFraction = -1;

        }

        internal static void UpdateNPCHook(On_NPC.orig_UpdateNPC orig, NPC npc, int i)
        {
            if (!npc.active)
            {
                CurrentSource = -1;
                CurrentFraction = -1;
                orig.Invoke(npc, i);
                return;
            }

            if ((npc.GetFraction() != -1 && BattleRoyaleMod.TakeYourTime == 1) || BattleRoyaleMod.TakeYourTime == 2)
            {
                if (npc.GetGlobalNPC<SaveNPCVelocity>().OldVel == null)
                {
                    npc.GetGlobalNPC<SaveNPCVelocity>().OldVel = npc.velocity;
                    npc.velocity = Vector2.Zero;
                }
                return;
            }
            else
            {
                if (npc.GetGlobalNPC<SaveNPCVelocity>().OldVel.HasValue)
                {
                    npc.velocity = npc.GetGlobalNPC<SaveNPCVelocity>().OldVel.Value;
                    npc.GetGlobalNPC<SaveNPCVelocity>().OldVel = null;
                }
            }


            int source = npc.GetSource();
            if (source == -1)
            {
                CurrentSource = npc.whoAmI;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    npc.GetGlobalNPC<SourceMarkNPC>().Source = -1;
                    CurrentSource = -1;
                }
                else
                {
                    CurrentSource = source;
                }
            }
            CurrentFraction = npc.GetFraction();
            orig.Invoke(npc, i);
            CurrentSource = -1;
            CurrentFraction = -1;
        }

        internal static void UpdateProjHook(On_Projectile.orig_Update orig, Projectile proj, int i)
        {
            if (!proj.active)
            {
                CurrentSource = -1;
                CurrentFraction = -1;
                orig.Invoke(proj, i);
                return;
            }


            if (((proj.GetFraction() != -1 && BattleRoyaleMod.TakeYourTime == 1) || BattleRoyaleMod.TakeYourTime == 2) && proj.type != ModContent.ProjectileType<MultipleSelectProj>())
            {
                if (proj.GetGlobalProjectile<SaveProjVelocity>().OldVel == null)
                {
                    proj.GetGlobalProjectile<SaveProjVelocity>().OldVel = proj.velocity;
                    proj.velocity = Vector2.Zero;
                }
                return;
            }
            else
            {
                if (proj.GetGlobalProjectile<SaveProjVelocity>().OldVel.HasValue)
                {
                    proj.velocity = proj.GetGlobalProjectile<SaveProjVelocity>().OldVel.Value;
                    proj.GetGlobalProjectile<SaveProjVelocity>().OldVel = null;
                }
            }



            int source = proj.GetSource();
            if (source != -1)
            {
                if (!Main.npc[source].active)
                {
                    proj.GetGlobalProjectile<SourceMarkProj>().Source = -1;
                    CurrentSource = -1;
                }
                else
                {
                    CurrentSource = source;
                }
            }
            else
            {
                CurrentSource = -1;
            }
            CurrentFraction = proj.GetFraction();
            orig.Invoke(proj, i);
            CurrentSource = -1;
            CurrentFraction = -1;
        }

        internal static int NewNPCHook(On_NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255)
        {
            int result = orig.Invoke(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            if (result != -1)
            {
                Main.npc[result].GetGlobalNPC<SourceMarkNPC>().Source = CurrentSource;
                Main.npc[result].GetGlobalNPC<SourceMarkNPC>().Fraction = CurrentFraction;
                PillerEnemyFix.GivePillerEnemyFraction(Main.npc[result]);
            }
            return result;
        }
        internal static int NewProjectileHook(On_Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2)
        {
            int result = orig.Invoke(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
            if (result != -1)
            {
                Main.projectile[result].GetGlobalProjectile<SourceMarkProj>().Source = CurrentSource;
                Main.projectile[result].GetGlobalProjectile<SourceMarkProj>().Fraction = CurrentFraction;
            }
            return result;
        }

    }

    public class SourceMarkNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int Source = -1;
        public int Fraction = -1;

        public override void SetDefaults(NPC entity)
        {
            entity.GetGlobalNPC<SourceMarkNPC>().Source = SourceMarkerSystem.CurrentSource;
            entity.GetGlobalNPC<SourceMarkNPC>().Fraction = SourceMarkerSystem.CurrentFraction;
        }
    }

    public class SourceMarkProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int Source = -1;
        public int Fraction = -1;
    }

}
