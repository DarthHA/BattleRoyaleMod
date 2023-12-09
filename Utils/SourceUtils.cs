using Terraria;

namespace BattleRoyaleMod
{
    public static class SourceUtils
    {
        public static int _getSource(this NPC npc)
        {
            return npc.GetGlobalNPC<SourceMarkNPC>().Source;
        }

        internal static int _getSource(this Projectile proj)
        {
            return proj.GetGlobalProjectile<SourceMarkProj>().Source;
        }
        public static int GetSource(this NPC npc)
        {
            int source = npc._getSource();
            if (source == -1)
            {
                return -1;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    return -1;
                }
                else
                {
                    return source;
                }
            }
        }
        public static int GetSourceOrSelf(this NPC npc)
        {
            int source = npc._getSource();
            if (source == -1)
            {
                return npc.whoAmI;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    return npc.whoAmI;
                }
                else
                {
                    return source;
                }
            }
        }

        public static int GetSource(this Projectile proj)
        {
            int source = proj._getSource();
            if (source == -1)
            {
                return -1;
            }
            else
            {
                if (!Main.npc[source].active)
                {
                    return -1;
                }
                else
                {
                    return source;
                }
            }
        }

        public static int GetFraction(this NPC npc)
        {
            return npc.GetGlobalNPC<SourceMarkNPC>().Fraction;
        }
        public static int GetFraction(this Projectile proj)
        {
            return proj.GetGlobalProjectile<SourceMarkProj>().Fraction;
        }

        public static bool IsNPCBullet(this NPC npc)
        {
            return (npc.life == 1 && npc.damage > 0) || ConfigDataUtils.GetIfBullet(npc);
        }
    }
}