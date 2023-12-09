using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace BattleRoyaleMod
{
    [TypeConverter(typeof(ToFromStringConverter<NPCBehaviorData>))]
    public class NPCBehaviorData
    {
        [DefaultValue(true)]
        public bool DoNotDespawn;
        [DefaultValue(false)]
        public bool IsBullet;
        [DefaultValue(false)]
        public bool DoNotCountBorder;
        public EventType CustomEventType;
        public BiomeType CustomBiomeType;


        public override bool Equals(object obj)
        {
            if (obj is NPCBehaviorData other)
                return DoNotDespawn == other.DoNotDespawn
                    && IsBullet == other.IsBullet
                    && DoNotCountBorder == other.DoNotCountBorder
                    && CustomEventType == other.CustomEventType
                    && CustomBiomeType == other.CustomBiomeType;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { DoNotDespawn, IsBullet, DoNotCountBorder, CustomEventType, CustomBiomeType }.GetHashCode();
        }


        public override string ToString()
        {
            return "";
            //return $"{DoNotDespawn},{IsBullet},{DoNotCountBorder}, {(int)CustomEventType}, {(int)CustomBiomeType}";
        }


        public static NPCBehaviorData FromString(string s)
        {
            string[] vars = s.Split(new char[] { ',' }, 5, StringSplitOptions.RemoveEmptyEntries);
            return new NPCBehaviorData
            {
                DoNotDespawn = Convert.ToBoolean(vars[0]),
                IsBullet = Convert.ToBoolean(vars[1]),
                DoNotCountBorder = Convert.ToBoolean(vars[2]),
                CustomEventType = (EventType)Convert.ToInt32(vars[3]),
                CustomBiomeType = (BiomeType)Convert.ToInt32(vars[4])
            };
        }

        public NPCBehaviorData()
        {
            DoNotDespawn = true;
            IsBullet = false;
            DoNotCountBorder = false;
            CustomEventType = EventType.None;
            CustomBiomeType = BiomeType.None;
        }
    }
}
