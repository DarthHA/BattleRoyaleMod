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
    public class Teleporter : ModItem
    {
        public static int FNum = 1;
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 0;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.FairyGlowstick;
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
                if (!Item.favorited)
                {
                    bool success = false;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.GetFraction() == FNum)
                        {
                            npc.Teleport(Main.MouseWorld);
                            success = true;
                        }
                    }
                    if (success)
                        SoundEngine.PlaySound(new SoundStyle("BattleRoyaleMod/Sounds/Teleport"));
                }
                else
                {
                    bool success = false;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.GetFraction() != -1)
                        {
                            npc.Teleport(Main.MouseWorld);
                            success = true;
                        }
                    }
                    if (success)
                        SoundEngine.PlaySound(new SoundStyle("BattleRoyaleMod/Sounds/Teleport"));
                }
            }
            else
            {
                bool[] activefractions = new bool[] { false, false, false, false, false, false, false, false };
                bool HasTeams = false;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.GetFraction() != -1)
                    {
                        HasTeams = true;
                        activefractions[npc.GetFraction() - 1] = true;
                    }
                }
                if (HasTeams)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        FNum++;
                        if (FNum > 8)
                        {
                            FNum = 1;
                        }
                        if (activefractions[FNum - 1]) break;
                    }
                    CombatText.NewText(player.Hitbox, Color.Cyan, string.Format(Language.GetTextValue("Mods.BattleRoyaleMod.SwitchToTeam"), FNum));
                }
                else
                {
                    CombatText.NewText(player.Hitbox, Color.Cyan, Language.GetTextValue("Mods.BattleRoyaleMod.NoTeamAvailable"));
                }
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