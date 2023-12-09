using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BattleRoyaleMod.System
{
    public class BattleBorderSystem : ModSystem
    {
        public override void Load()
        {
        }


        public override void PostUpdateNPCs()
        {
            if (!BattleRoyaleMod.BattleStart || !BattleRoyaleMod.Gconfig.UseBorderSystem) return;
            float Left = BattleRoyaleMod.TopLeft.X;
            float Right = BattleRoyaleMod.BottomRight.X;
            float Top = BattleRoyaleMod.TopLeft.Y;
            float Bottom = BattleRoyaleMod.BottomRight.Y;

            float? _left = null;
            float? _right = null;
            float? _top = null;
            float? _bottom = null;
            bool HasEnemies = false;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.GetFraction() != -1 && !ConfigDataUtils.GetIfNotCountBorder(npc) && !SourceUtils.IsNPCBullet(npc))
                {
                    HasEnemies = true;
                    if (_left == null || npc.position.X < _left.Value)
                    {
                        _left = npc.position.X;
                    }
                    if (_right == null || npc.position.X + npc.width > _right.Value)
                    {
                        _right = npc.position.X + npc.width;
                    }
                    if (_top == null || npc.position.Y < _top.Value)
                    {
                        _top = npc.position.Y;
                    }
                    if (_bottom == null || npc.position.Y + npc.height > _bottom.Value)
                    {
                        _bottom = npc.position.Y + npc.height;
                    }
                }
            }

            if (HasEnemies)
            {
                Vector2 Offset = Vector2.Zero;
                if (_left < Left)
                {
                    Offset.X = Left - _left.Value;
                }
                else if (_right > Right)
                {
                    Offset.X = Right - _right.Value;
                }

                if (_top < Top)
                {
                    Offset.Y = Top - _top.Value;
                }
                else if (_bottom > Bottom)
                {
                    Offset.Y = Bottom - _bottom.Value;
                }

                if (Offset != Vector2.Zero)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.GetFraction() != -1)
                        {
                            npc.position += Offset;
                        }
                    }
                    foreach (Projectile proj in Main.projectile)
                    {
                        if (proj.active && proj.GetFraction() != -1)
                        {
                            proj.position += Offset;
                        }
                    }
                }
            }

        }
    }
}
