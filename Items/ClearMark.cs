using BattleRoyaleMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class ClearMark : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*
			DisplayName.SetDefault("Clear All Marks");
			DisplayName.AddTranslation(GameCulture.Chinese, "清除标记");
			Tooltip.SetDefault("Left click to remove all the marks on the creature\n" +
				"Right click to remove all the marked creatures and their minions and projectiles\n" +
				"When your game get stucked, try using this");
			Tooltip.AddTranslation(GameCulture.Chinese, "左键清除掉所有生物的标记\n" +
				"右键清除标记的生物及其衍生仆从和弹幕\n" +
				"如果你游戏出现了问题，用一下这个试试");
			*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MultipleSelectProj>();
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
                Item.useStyle = ItemUseStyleID.Swing;
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

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active)
                    {
                        npc.GetGlobalNPC<SourceMarkNPC>().Fraction = -1;
                        npc.GetGlobalNPC<SourceMarkNPC>().Source = -1;
                    }
                }
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active)
                    {
                        proj.GetGlobalProjectile<SourceMarkProj>().Fraction = -1;
                        proj.GetGlobalProjectile<SourceMarkProj>().Source = -1;
                    }
                }
                Main.NewText(Language.GetTextValue("Mods.BattleRoyaleMod.AllMarksClear"), Color.Cyan);
                SoundEngine.PlaySound(SoundID.Item115);
            }
            return false;
        }

    }
}