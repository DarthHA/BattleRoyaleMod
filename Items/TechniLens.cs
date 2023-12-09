using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.Items
{
    public class TechniLens : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("仇恨晶状体");
            //Tooltip.SetDefault("显示怪物仇恨情况\n测试用,怪物较多时可能会产生卡顿，甚至是游戏崩溃");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 0;
            Item.rare = ItemRarityID.Expert;
        }

        public override void UpdateInventory(Player player)
        {
            player.GetModPlayer<YiYanDingZhenPlayer>().DingZhen = true;
            if (Item.favorited)
            {
                player.GetModPlayer<YiYanDingZhenPlayer>().ShowMore = true;
            }
        }
    }

    public class YiYanDingZhenPlayer : ModPlayer
    {
        public bool DingZhen = false;
        public bool ShowMore = false;
        public bool ShowBorder = false;

        public override void ResetEffects()
        {
            DingZhen = false;
            ShowMore = false;
            ShowBorder = false;
        }
        public override void UpdateDead()
        {
            DingZhen = false;
            ShowMore = false;
            ShowBorder = false;
        }
    }

    public class ShowTeamNPC : GlobalNPC
    {
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Main.LocalPlayer.GetModPlayer<YiYanDingZhenPlayer>().ShowMore)
            {
                if (npc.GetFraction() != -1)
                {
                    Utils.DrawBorderString(spriteBatch, npc.GetFraction().ToString(), npc.Bottom - screenPos + new Vector2(0, 20), Color.White);
                }
            }
        }

    }

    public class ShowTeamProj : GlobalProjectile
    {
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (Main.LocalPlayer.GetModPlayer<YiYanDingZhenPlayer>().ShowMore && BattleRoyaleMod.Gconfig.EnableProjShow)
            {
                if (projectile.GetFraction() != -1)
                {
                    Utils.DrawBorderString(Main.spriteBatch, projectile.GetFraction().ToString(), projectile.Center - Main.screenPosition + new Vector2(0, 20), Color.White);
                }
            }
        }

    }
}