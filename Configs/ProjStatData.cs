using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace BattleRoyaleMod
{
    [TypeConverter(typeof(ToFromStringConverter<ProjStatData>))]
    public class ProjStatData
    {
        [Range(0.1f, 10f)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float DamageModifier;



        public override bool Equals(object obj)
        {
            if (obj is ProjStatData other)
                return DamageModifier == other.DamageModifier;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { DamageModifier }.GetHashCode();
        }


        public override string ToString()
        {
            return "";
            //return $"{DamageModifier}";
        }


        public static ProjStatData FromString(string s)
        {
            string[] vars = s.Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
            return new ProjStatData
            {
                DamageModifier = Convert.ToSingle(vars[0]),
            };
        }

        public ProjStatData()
        {
            DamageModifier = 1;
        }
    }
}
