using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace BattleRoyaleMod
{
    [TypeConverter(typeof(ToFromStringConverter<NPCStatData>))]
    public class NPCStatData
    {
        [Range(0.1f, 10f)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float DamageModifier;

        [Range(0.1f, 10f)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float LifeModifier;

        [Range(0.1f, 10f)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float DefenseModifier;


        public override bool Equals(object obj)
        {
            if (obj is NPCStatData other)
                return DamageModifier == other.DamageModifier
                    && DefenseModifier == other.DefenseModifier
                    && LifeModifier == other.LifeModifier;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { DamageModifier, LifeModifier, DefenseModifier }.GetHashCode();
        }


        public override string ToString()
        {
            return "";
            //return $"{DamageModifier}, {LifeModifier}, {DefenseModifier}";
        }


        public static NPCStatData FromString(string s)
        {
            string[] vars = s.Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
            return new NPCStatData
            {
                DamageModifier = Convert.ToSingle(vars[0]),
                LifeModifier = Convert.ToSingle(vars[1]),
                DefenseModifier = Convert.ToSingle(vars[2]),
            };
        }

        public NPCStatData()
        {
            DamageModifier = 1;
            LifeModifier = 1;
            DefenseModifier = 1;
        }
    }
}
