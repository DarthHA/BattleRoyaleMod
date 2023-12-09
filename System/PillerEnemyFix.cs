using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BattleRoyaleMod
{
    public static class PillerEnemyFix
    {
        public static void GivePillerEnemyFraction(NPC npc)
        {
            if (IsPillerEnemy(npc.type) == -1 || npc.GetFraction() != -1 || npc.GetSource() != -1) return;

            int owner = FindClosestNPC(IsPillerEnemy(npc.type), npc.Center);
            if (owner != -1)
            {
                npc.GetGlobalNPC<SourceMarkNPC>().Fraction = Main.npc[owner].GetFraction();
            }

        }

        internal static int FindClosestNPC(int type, Vector2 Pos)
        {
            int result = -1;
            float dist = -1;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == type)
                {
                    if (result == -1 || npc.Distance(Pos) < dist)
                    {
                        result = npc.whoAmI;
                        dist = npc.Distance(Pos);
                    }
                }
            }
            return result;
        }

        internal static int IsPillerEnemy(int type)
        {
            switch (type)
            {
                case 516:
                case 518:
                case 519:
                case 412:
                case 413:
                case 414:
                case 415:
                case 416:
                case 417:
                case 418:
                case 419:
                    return NPCID.LunarTowerSolar;
                case 425:
                case 426:
                case 427:
                case 428:
                case 429:
                    return NPCID.LunarTowerVortex;
                case 420:
                case 421:
                case 423:
                case 424:
                    return NPCID.LunarTowerNebula;
                case 402:
                case 403:
                case 404:
                case 405:
                case 406:
                case 407:
                case 409:
                case 410:
                case 411:
                    return NPCID.LunarTowerStardust;
            }
            return -1;
        }
    }
}
