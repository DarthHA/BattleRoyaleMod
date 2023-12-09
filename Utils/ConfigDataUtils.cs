
using Terraria;
using Terraria.ModLoader.Config;

namespace BattleRoyaleMod
{
    public static class ConfigDataUtils
    {
        public static EventType? GetEventType(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.SpecialBehavior.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.SpecialBehavior[definition].CustomEventType;
                }
            }
            return null;
        }

        public static BiomeType? GetBiomeType(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.SpecialBehavior.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.SpecialBehavior[definition].CustomBiomeType;
                }
            }
            return null;
        }

        public static bool GetIfNotDespawn(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.SpecialBehavior.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.SpecialBehavior[definition].DoNotDespawn;
                }
            }
            return false;
        }

        public static bool GetIfBullet(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.SpecialBehavior.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.SpecialBehavior[definition].IsBullet;
                }
            }
            return false;
        }

        public static bool GetIfNotCountBorder(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.SpecialBehavior.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.SpecialBehavior[definition].DoNotCountBorder;
                }
            }
            return false;
        }


        public static float GetDamageModifier(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC[definition].DamageModifier;
                }
            }
            return 1;
        }

        public static float GetLifeModifier(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC[definition].LifeModifier;
                }
            }
            return 1;
        }

        public static float GetDefenseModifier(NPC npc)
        {
            foreach (NPCDefinition definition in BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC.Keys)
            {
                if (definition.Type == npc.type)
                {
                    return BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataNPC[definition].DefenseModifier;
                }
            }
            return 1;
        }

        public static float GetDamageModifier(Projectile proj)
        {
            foreach (ProjectileDefinition definition in BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataProj.Keys)
            {
                if (definition.Type == proj.type)
                {
                    return BattleRoyaleMod.Gconfig.subConfigForStat.SpecialDataProj[definition].DamageModifier;
                }
            }
            return 1;
        }
    }
}
