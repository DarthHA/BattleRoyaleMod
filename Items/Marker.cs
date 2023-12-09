using BattleRoyaleMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class Marker : ModItem
    {
        public static int FNum = 1;
        public override void SetStaticDefaults()
        {
            /*
DisplayName.SetDefault("Creature marker");
DisplayName.AddTranslation(GameCulture.Chinese, "生物标记器");
Tooltip.SetDefault("Left click to apply Mark [c/ff0000:A] to the creature, and right click to apply Mark [c/00ffff:B] to another creature\n" +
	"When Mark [c/ff0000:A] and Mark [c/00ffff:B] coexist, the two creatures will fight with each other\n" +
	"Cannot apply marks to boss minions or minor parts of boss.\n" +
	"NOTE: Cannot apply marks to Eater of World or its segments");
Tooltip.AddTranslation(GameCulture.Chinese, "左键向生物施加标记[c/ff0000:A]，右键向另一个生物施加标记[c/00ffff:B]\n" +
	"当标记[c/ff0000:A]和标记[c/00ffff:B]共存时两生物会互相产生仇恨\n" +
	"不能施加在Boss仆从或者次要部分上。\n" +
	"注意：不能施加于世界吞噬者及其体节上");
		*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.value = 0;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MultipleSelectProj>();
            Item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.channel = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.channel = false;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, 0, 0, player.whoAmI);
            }
            else
            {
                FNum++;
                if (FNum > 8)
                {
                    FNum = 1;
                }
                CombatText.NewText(player.Hitbox, Color.Cyan, string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.SwitchToTeam"), FNum));
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            bool selected = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] == Item;
            float size = selected ? 0.8f : 0.7f;
            Vector2 vector = selected ? new Vector2(14f, -8f) : new Vector2(12, -4f);
            Utils.DrawBorderStringFourWay(spriteBatch, Terraria.GameContent.FontAssets.ItemStack.Value, FNum.ToString(), position.X, position.Y, Color.White, Color.Black, vector, size);
        }
    }
}