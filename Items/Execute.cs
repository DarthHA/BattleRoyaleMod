using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class Execute : ModItem
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
            Item.rare = ItemRarityID.Red;
            Item.UseSound = new Terraria.Audio.SoundStyle("BattleRoyaleMod/Sounds/ILOVEYOU");
            Item.autoReuse = false;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<int> targets = new();
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.townNPC && !npc.CountsAsACritter && !npc.dontTakeDamage && !npc.dontTakeDamageFromHostiles)
                {
                    if (npc.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                    {
                        targets.Add(npc.whoAmI);
                    }
                }
            }

            if (targets.Count > 0)
            {
                if (player.altFunctionUse != 2)
                {
                    foreach (int target in targets)
                    {
                        CombatText.NewText(Main.npc[target].Hitbox, Color.Red, Language.GetTextValue("Mods.BattleRoyaleMod.Bonk"));
                        NPC.HitInfo hit = Main.npc[target].CalculateHitInfo(damage, 0, false, 0, null, false, 0);
                        hit.Damage = Main.npc[target].lifeMax / 3;
                        Main.npc[target].StrikeNPC(hit, false, true);
                    }
                }
                else
                {
                    foreach (int target in targets)
                    {
                        CombatText.NewText(Main.npc[target].Hitbox, Color.Red, Language.GetTextValue("Mods.BattleRoyaleMod.InstKill"));
                        Main.npc[target].StrikeInstantKill();
                    }
                }
            }

            return false;
        }
    }


}