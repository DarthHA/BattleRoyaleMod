
using BattleRoyaleMod.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Projectiles
{
    public class MultipleSelectProj : ModProjectile
    {
        public Vector2 EndPos;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 114514;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            EndPos = Main.MouseWorld;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost || owner.CCed)
            {
                Projectile.Kill();
                return;
            }
            if (owner.Distance(Projectile.Center) > 3000)
            {
                Projectile.active = false;
                return;
            }

            owner.itemAnimation = 2;
            owner.itemTime = 2;
            Vector2 unit = Main.MouseWorld - owner.Center;
            owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X);
            int dir = owner.direction;
            owner.itemRotation = (float)Math.Atan2(unit.Y * dir, unit.X * dir);

            Projectile.timeLeft = 114514;
            EndPos = Main.MouseWorld;
            if (!owner.channel)
            {
                if (owner.HeldItem.type == ModContent.ItemType<Marker>())
                {
                    if (!owner.HeldItem.favorited)
                    {
                        List<int> targets = new();
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active && !npc.friendly && !npc.townNPC && !npc.CountsAsACritter)
                            {
                                if (GetRectangle().Intersects(npc.Hitbox))
                                {
                                    targets.Add(npc.whoAmI);
                                }
                            }
                        }

                        if (targets.Count > 0)
                        {
                            Dictionary<string, int> battleNPCsName = new();
                            List<int> battleNPCs = new();
                            foreach (int target in targets)
                            {
                                int wmi = Main.npc[target].GetSourceOrSelf();
                                foreach (NPC npc in Main.npc)
                                {
                                    if (npc.active && (npc.whoAmI == wmi || npc.GetSource() == wmi))
                                    {
                                        npc.GetGlobalNPC<SourceMarkNPC>().Fraction = Marker.FNum;
                                        if (!battleNPCs.Contains(npc.whoAmI))
                                        {
                                            battleNPCs.Add(npc.whoAmI);
                                        }

                                    }
                                }



                                foreach (Projectile proj in Main.projectile)
                                {
                                    if (proj.active && proj.GetSource() == wmi)
                                    {
                                        proj.GetGlobalProjectile<SourceMarkProj>().Fraction = Marker.FNum;
                                    }
                                }
                            }

                            foreach (int npcid in battleNPCs)
                            {
                                if (Main.npc[npcid].IsNPCBullet()) continue;
                                if (!battleNPCsName.ContainsKey(Lang.GetNPCNameValue(Main.npc[npcid].type)))
                                {
                                    battleNPCsName.Add(Lang.GetNPCNameValue(Main.npc[npcid].type), 1);
                                }
                                else
                                {
                                    battleNPCsName[Lang.GetNPCNameValue(Main.npc[npcid].type)]++;
                                }
                            }

                            if (battleNPCsName.Count > 0)
                            {
                                string result = "";
                                foreach (string name in battleNPCsName.Keys)
                                {
                                    if (battleNPCsName[name] > 1)
                                    {
                                        result += name + " x" + battleNPCsName[name];
                                    }
                                    else
                                    {
                                        result += name;
                                    }
                                    result += ", ";
                                }
                                result = result[..^2];
                                result += string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.JoinTheTeam"), Marker.FNum);
                                Main.NewText(result, Color.Cyan);
                            }
                        }
                    }
                    else
                    {
                        List<int> targets = new();
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.active)
                            {
                                if (GetRectangle().Intersects(npc.Hitbox))
                                {
                                    targets.Add(npc.whoAmI);
                                }
                            }
                        }

                        if (targets.Count > 0)
                        {
                            Dictionary<string, int> battleNPCsName = new();
                            List<int> battleNPCs = new();
                            foreach (int target in targets)
                            {
                                int wmi = Main.npc[target].whoAmI;

                                foreach (NPC npc in Main.npc)
                                {
                                    if (npc.active && npc.whoAmI == wmi)
                                    {
                                        npc.GetGlobalNPC<SourceMarkNPC>().Fraction = Marker.FNum;
                                        if (!battleNPCs.Contains(npc.whoAmI))
                                        {
                                            battleNPCs.Add(npc.whoAmI);
                                        }

                                    }
                                }

                                foreach (Projectile proj in Main.projectile)
                                {
                                    if (proj.active && proj.GetSource() == wmi)
                                    {
                                        proj.GetGlobalProjectile<SourceMarkProj>().Fraction = Marker.FNum;
                                    }
                                }
                            }

                            foreach (int npcid in battleNPCs)
                            {
                                if (Main.npc[npcid].IsNPCBullet()) continue;
                                if (!battleNPCsName.ContainsKey(Lang.GetNPCNameValue(Main.npc[npcid].type)))
                                {
                                    battleNPCsName.Add(Lang.GetNPCNameValue(Main.npc[npcid].type), 1);
                                }
                                else
                                {
                                    battleNPCsName[Lang.GetNPCNameValue(Main.npc[npcid].type)]++;
                                }
                            }

                            if (battleNPCsName.Count > 0)
                            {
                                string result = "";
                                foreach (string name in battleNPCsName.Keys)
                                {
                                    if (battleNPCsName[name] > 1)
                                    {
                                        result += name + " x" + battleNPCsName[name];
                                    }
                                    else
                                    {
                                        result += name;
                                    }
                                    result += ", ";
                                }
                                result = result[..^2];
                                result += string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.JoinTheTeam"), Marker.FNum);
                                Main.NewText(result, Color.Red);
                            }
                        }
                    }
                }
                else if (owner.HeldItem.type == ModContent.ItemType<ClearMark>())
                {
                    List<int> targets = new();
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && !npc.friendly && !npc.townNPC && !npc.CountsAsACritter)
                        {
                            if (GetRectangle().Intersects(npc.Hitbox))
                            {
                                targets.Add(npc.whoAmI);
                            }
                        }
                    }

                    if (targets.Count > 0)
                    {
                        foreach (int target in targets)
                        {
                            int wmi = Main.npc[target].GetSourceOrSelf();
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc.active && (npc.whoAmI == wmi || npc.GetSource() == wmi))
                                {
                                    npc.GetGlobalNPC<SourceMarkNPC>().Fraction = -1;
                                }
                            }

                            foreach (Projectile proj in Main.projectile)
                            {
                                if (proj.active && proj.GetSource() == wmi)
                                {
                                    proj.GetGlobalProjectile<SourceMarkProj>().Fraction = -1;
                                }
                            }
                        }
                        Main.NewText(Language.GetTextValue("Mods.BattleRoyaleMod.MarksClear"), Color.Cyan);

                    }
                }
                SoundEngine.PlaySound(SoundID.Item115);
                Projectile.Kill();
            }

        }


        public override bool? CanDamage()
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Utils.DrawRectangle(Main.spriteBatch, GetRectangle().TopLeft(), GetRectangle().BottomRight(), Color.White, Color.White, 2);
            return false;
        }

        private Rectangle GetRectangle()
        {
            if (EndPos == Vector2.Zero)
            {
                return new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 0, 0);
            }
            return new Rectangle(
                (int)Math.Min(EndPos.X, Projectile.Center.X),
                (int)Math.Min(EndPos.Y, Projectile.Center.Y),
                (int)Math.Abs(EndPos.X - Projectile.Center.X),
                (int)Math.Abs(EndPos.Y - Projectile.Center.Y)
                );
        }

    }
}