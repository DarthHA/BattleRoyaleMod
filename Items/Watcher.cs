using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class Watcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Watch the battle");
            //DisplayName.AddTranslation(GameCulture.Chinese, "观战");
            //Tooltip.SetDefault("Left click to fix your perspective on a certain creature, right click to escape.\nJust be a onlooker.");
            //Tooltip.AddTranslation(GameCulture.Chinese, "左键将你的视角固定在指定生物身上，右键脱离。\n吃瓜就行了");
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
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int target = -1;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.townNPC && !npc.CountsAsACritter)
                    {
                        if (npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                        {
                            target = npc.whoAmI;
                            break;
                        }
                    }
                }

                if (target != -1)
                {
                    player.GetModPlayer<WatcherPlayer>().WatchNPC = target;
                    SoundEngine.PlaySound(SoundID.Item6);
                }
            }
            else
            {

                if (player.GetModPlayer<WatcherPlayer>().WatchNPC != -1)
                {
                    SoundEngine.PlaySound(SoundID.Item1);
                }
                player.GetModPlayer<WatcherPlayer>().WatchNPC = -1;
            }
            return false;
        }
    }

    public class WatcherPlayer : ModPlayer
    {
        public int WatchNPC = -1;
        public override void ModifyScreenPosition()
        {
            if (WatchNPC == -1) return;
            if (!Main.npc[WatchNPC].active)
            {
                WatchNPC = -1;
                return;
            }
            Main.screenPosition = Main.npc[WatchNPC].Center - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
        }
    }
}