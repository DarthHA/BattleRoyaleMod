using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class ExecuteAll : ModItem
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
            Item.damage = 114514;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = new Terraria.Audio.SoundStyle("BattleRoyaleMod/Sounds/SLAYALL");
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (player.altFunctionUse != 2)
            {
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.GetFraction() != -1)
                    {
                        target.active = false;
                    }
                }
            }
            else
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.GetFraction() != -1)
                    {
                        proj.active = false;
                    }
                }
            }


            return false;
        }
    }


}