using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace BattleRoyaleMod
{
    public class BattleRoyaleMod : Mod
    {
        public static bool BattleStart = false;
        public static int TakeYourTime = 0;

        public static Vector2 TopLeft = Vector2.Zero;
        public static Vector2 BottomRight = Vector2.Zero;

        public static CustomConfig Gconfig;

        public override void Unload()
        {
            Gconfig = null;
        }
    }

}