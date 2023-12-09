using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class ArenaRuler : ModItem
    {
        public bool SwitchWhich = true;
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item115;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            return BattleRoyaleMod.Gconfig.UseBorderSystem;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Vector2 vec1 = Main.MouseWorld;
                Vector2 vec2;
                if (SwitchWhich)
                {
                    vec2 = BattleRoyaleMod.BottomRight;
                }
                else
                {
                    vec2 = BattleRoyaleMod.TopLeft;
                }
                SwitchWhich = !SwitchWhich;
                float Left = Math.Min(vec1.X, vec2.X);
                float Right = Math.Max(vec1.X, vec2.X);
                float Top = Math.Min(vec1.Y, vec2.Y);
                float Bottom = Math.Max(vec1.Y, vec2.Y);
                BattleRoyaleMod.TopLeft = new Vector2(Left, Top);
                BattleRoyaleMod.BottomRight = new Vector2(Right, Bottom);
            }
            else
            {
                SwitchWhich = true;
                float WorldOffsetX = 40 * 2 + Main.screenWidth / 2 + 200;
                float WorldOffsetY = 40 * 2 + Main.screenHeight / 2 + 200;
                float Left = WorldOffsetX;
                float Right = Main.maxTilesX * 16 - WorldOffsetX;
                float Top = WorldOffsetY;
                float Bottom = Main.maxTilesY * 16 - WorldOffsetY;

                BattleRoyaleMod.TopLeft = new Vector2(Left, Top);
                BattleRoyaleMod.BottomRight = new Vector2(Right, Bottom);
            }
            return false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.GetModPlayer<YiYanDingZhenPlayer>().ShowBorder = true;
            }
        }
    }

}


