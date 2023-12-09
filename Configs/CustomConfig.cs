using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BattleRoyaleMod
{
    public enum EventType
    {
        None,
        Day,
        Night,
        Rain,
        SandStorm,
        BloodMoon,
        Eclipse,
        Invasion
    }

    public enum BiomeType
    {
        None,
        Beach,
        Caverns,
        Corrupt,
        Crimson,
        Desert,
        Dungeon,
        Snow,
        Glowshroom,
        Hallow,
        Hell,
        Jungle,
        LihzhardTemple,
        Overworld,
        Space,
        UndergroundDesert
    };
    public class CustomConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.BattleRoyaleMod.NormalHeader")]

        [DefaultValue(false)]
        public bool EnableProjShow;


        [DefaultValue(false)]
        public bool UseBorderSystem;

        [DefaultValue(false)]
        public bool AllSegmentUseSameImmune;

        [Increment(1)]
        [Range(1, 99999)]
        [DefaultValue(1)]
        public int MeleeDamageModifier;

        [Increment(1)]
        [Range(1, 99999)]
        [DefaultValue(1)]
        public int ProjDamageModifier;


        [Increment(1)]
        [Range(0, 99999)]
        [DefaultValue(0)]
        public int SourceNPCAggro;

        [Header("$Mods.BattleRoyaleMod.WarningHeader")]

        [DefaultValue(true)]
        public bool TakeEffectInstantly;

        public SubConfigForStat subConfigForStat = new();

        public Dictionary<NPCDefinition, NPCBehaviorData> SpecialBehavior = new();

        [SeparatePage]
        public class SubConfigForStat
        {
            public Dictionary<NPCDefinition, NPCStatData> SpecialDataNPC = new();

            public Dictionary<ProjectileDefinition, ProjStatData> SpecialDataProj = new();
            public override string ToString()
            {
                return "";
                //return Language.GetTextValue("Mods.BattleRoyaleMod.Configs.subConfigForStat.Label");
                //return $"{SpecialDataNPC}/{SpecialDataProj}";
            }

            public override bool Equals(object obj)
            {
                if (obj is SubConfigForStat other)
                    return SpecialDataNPC.Equals(other.SpecialDataNPC) && SpecialDataProj.Equals(other.SpecialDataProj);
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { SpecialDataNPC, SpecialDataProj }.GetHashCode();
            }
        }

        public override ModConfig Clone()
        {
            var clone = (CustomConfig)base.Clone();
            return clone;
        }

        public override void OnLoaded()
        {
            BattleRoyaleMod.Gconfig = this;
        }

    }
}