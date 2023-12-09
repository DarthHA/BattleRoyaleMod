using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class Starter : ModItem
    {
        public int FNum = 1;
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
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 0;
            Item.rare = ItemRarityID.Expert;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            BattleRoyaleMod.BattleStart = !BattleRoyaleMod.BattleStart;
            SoundEngine.PlaySound(SoundID.Roar);
            if (BattleRoyaleMod.BattleStart)
            {
                Main.NewText(Language.GetTextValue("Mods.BattleRoyaleMod.BattleStart"), new Color(175, 75, 255));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.BattleRoyaleMod.BattleEnd"), new Color(175, 75, 255));
            }
            return false;
        }
    }
}