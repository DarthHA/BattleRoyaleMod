using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class RagTime : ModItem
    {
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Expert;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (BattleRoyaleMod.TakeYourTime == 0)
                {
                    BattleRoyaleMod.TakeYourTime = 2;
                }
                else
                {
                    BattleRoyaleMod.TakeYourTime = 0;
                }
            }
            else
            {
                if (BattleRoyaleMod.TakeYourTime == 0)
                {
                    BattleRoyaleMod.TakeYourTime = 1;
                }
                else
                {
                    BattleRoyaleMod.TakeYourTime = 0;
                }
            }
            if (BattleRoyaleMod.TakeYourTime > 0)
            {
                SoundEngine.PlaySound(new SoundStyle("BattleRoyaleMod/Sounds/ZaWarudo"));
                CombatText.NewText(player.Hitbox, BattleRoyaleMod.TakeYourTime > 1 ? Color.Red : Color.Cyan, Language.GetTextValue("Mods.BattleRoyaleMod.ZaWarudo"));
            }
            else
            {
                SoundEngine.PlaySound(new SoundStyle("BattleRoyaleMod/Sounds/ZaWarudoResume"));
                CombatText.NewText(player.Hitbox, Color.Cyan, Language.GetTextValue("Mods.BattleRoyaleMod.ZaWarudoResume"));
            }
            return false;
        }
    }


}