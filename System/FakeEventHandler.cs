using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace BattleRoyaleMod.System
{
    public class FakeEventHandler : ModSystem
    {
        public static bool? DayTime = null;
        public static bool? BloodMoon = null;
        public static bool? Eclipse = null;
        public static bool? Raining = null;
        public static bool? SandStorm = null;
        public static int? InvasionType = null;
        public static bool? SnowMoon = null;
        public static bool? PumpkinMoon = null;
        public static bool? OldOneArmy = null;

        public static bool? Beach = null;
        public static bool? Caverns = null;
        public static bool? Corrupt = null;
        public static bool? Crimson = null;
        public static bool? Desert = null;
        public static bool? Dungeon = null;
        public static bool? Snow = null;
        public static bool? Overworld = null;
        public static bool? Glowshroom = null;
        public static bool? Graveyard = null;
        public static bool? Hallow = null;
        public static bool? Hell = null;
        public static bool? Jungle = null;
        public static bool? LihzhardTemple = null;
        public static bool? Space = null;
        public static bool? UndergroundDesert = null;

        public override void Load()
        {

        }

        public static void SetEvent(NPC npc)
        {
            if (ConfigDataUtils.GetEventType(npc).HasValue)
            {
                EventType eventType = ConfigDataUtils.GetEventType(npc).Value;
                switch (eventType)
                {
                    case EventType.Day:
                        DayTime = Main.dayTime;
                        Main.dayTime = true;
                        break;
                    case EventType.Night:
                        DayTime = Main.dayTime;
                        Main.dayTime = false;
                        break;
                    case EventType.BloodMoon:
                        DayTime = Main.dayTime;
                        Main.dayTime = false;
                        BloodMoon = Main.bloodMoon;
                        Main.bloodMoon = true;
                        break;
                    case EventType.Eclipse:
                        DayTime = Main.dayTime;
                        Main.dayTime = true;
                        Eclipse = Main.eclipse;
                        Main.eclipse = true;
                        break;
                    case EventType.Rain:
                        Raining = Main.raining;
                        Main.raining = true;
                        break;
                    case EventType.SandStorm:
                        SandStorm = Sandstorm.Happening;
                        Sandstorm.Happening = true;
                        break;
                    case EventType.Invasion:
                        int type = NPC.GetNPCInvasionGroup(npc.type);
                        switch (type)
                        {
                            case -3:         //旧日
                                OldOneArmy = DD2Event.Ongoing;
                                DD2Event.Ongoing = true;
                                break;
                            case -2:         //南瓜月
                                PumpkinMoon = Main.pumpkinMoon;
                                Main.pumpkinMoon = true;
                                DayTime = Main.dayTime;
                                Main.dayTime = false;
                                break;
                            case -1:         //霜月
                                SnowMoon = Main.snowMoon;
                                Main.snowMoon = true;
                                DayTime = Main.dayTime;
                                Main.dayTime = false;
                                break;
                            case 0:         //无
                                break;
                            case 1:          //哥布林
                            case 2:          //雪人
                            case 3:          //海盗
                            case 4:          //火星
                                InvasionType = Main.invasionType;
                                Main.invasionType = type;
                                break;
                        }
                        break;
                }

                if (eventType == EventType.Invasion && npc.type == NPCID.PumpkingBlade)              //一个Fix
                {
                    PumpkinMoon = Main.pumpkinMoon;
                    Main.pumpkinMoon = true;
                    DayTime = Main.dayTime;
                    Main.dayTime = false;
                }
            }


            if (ConfigDataUtils.GetBiomeType(npc).HasValue)
            {
                BiomeType biomeType = ConfigDataUtils.GetBiomeType(npc).Value;
                switch (biomeType)
                {
                    case BiomeType.Beach:
                        Beach = Main.LocalPlayer.ZoneBeach;
                        Main.LocalPlayer.ZoneBeach = true;
                        break;
                    case BiomeType.Caverns:
                        Caverns = Main.LocalPlayer.ZoneRockLayerHeight;
                        Main.LocalPlayer.ZoneRockLayerHeight = true;
                        break;
                    case BiomeType.Corrupt:
                        Corrupt = Main.LocalPlayer.ZoneCorrupt;
                        Main.LocalPlayer.ZoneCorrupt = true;
                        break;
                    case BiomeType.Crimson:
                        Crimson = Main.LocalPlayer.ZoneCrimson;
                        Main.LocalPlayer.ZoneCrimson = true;
                        break;
                    case BiomeType.Desert:
                        Desert = Main.LocalPlayer.ZoneDesert;
                        Main.LocalPlayer.ZoneDesert = true;
                        break;
                    case BiomeType.Dungeon:
                        Dungeon = Main.LocalPlayer.ZoneDungeon;
                        Main.LocalPlayer.ZoneDungeon = true;
                        break;
                    case BiomeType.Snow:
                        Snow = Main.LocalPlayer.ZoneSnow;
                        Main.LocalPlayer.ZoneSnow = true;
                        break;
                    case BiomeType.Glowshroom:
                        Glowshroom = Main.LocalPlayer.ZoneGlowshroom;
                        Main.LocalPlayer.ZoneGlowshroom = true;
                        break;
                    case BiomeType.Hallow:
                        Hallow = Main.LocalPlayer.ZoneHallow;
                        Main.LocalPlayer.ZoneHallow = true;
                        break;
                    case BiomeType.Hell:
                        Hallow = Main.LocalPlayer.ZoneUnderworldHeight;
                        Main.LocalPlayer.ZoneUnderworldHeight = true;
                        break;
                    case BiomeType.Jungle:
                        Jungle = Main.LocalPlayer.ZoneJungle;
                        Main.LocalPlayer.ZoneJungle = true;
                        break;
                    case BiomeType.LihzhardTemple:
                        LihzhardTemple = Main.LocalPlayer.ZoneLihzhardTemple;
                        Main.LocalPlayer.ZoneLihzhardTemple = true;
                        break;
                    case BiomeType.Overworld:
                        Overworld = Main.LocalPlayer.ZoneOverworldHeight;
                        Main.LocalPlayer.ZoneOverworldHeight = true;
                        break;
                    case BiomeType.Space:
                        Space = Main.LocalPlayer.ZoneSkyHeight;
                        Main.LocalPlayer.ZoneSkyHeight = true;
                        break;
                    case BiomeType.UndergroundDesert:
                        UndergroundDesert = Main.LocalPlayer.ZoneUndergroundDesert;
                        Main.LocalPlayer.ZoneUndergroundDesert = true;
                        break;
                }
            }

        }

        public static void ResetEvent()
        {
            if (Eclipse.HasValue)
            {
                Main.eclipse = Eclipse.Value;
                Eclipse = null;
            }
            if (DayTime.HasValue)
            {
                Main.dayTime = DayTime.Value;
                DayTime = null;
            }
            if (Raining.HasValue)
            {
                Main.raining = Raining.Value;
                Raining = null;
            }
            if (SandStorm.HasValue)
            {
                Sandstorm.Happening = SandStorm.Value;
                SandStorm = null;
            }
            if (BloodMoon.HasValue)
            {
                Main.bloodMoon = BloodMoon.Value;
                BloodMoon = null;
            }
            if (OldOneArmy.HasValue)
            {
                DD2Event.Ongoing = OldOneArmy.Value;
                OldOneArmy = null;
            }
            if (PumpkinMoon.HasValue)
            {
                Main.pumpkinMoon = PumpkinMoon.Value;
                PumpkinMoon = null;
            }
            if (SnowMoon.HasValue)
            {
                Main.snowMoon = SnowMoon.Value;
                SnowMoon = null;
            }
            if (InvasionType.HasValue)
            {
                Main.invasionType = InvasionType.Value;
                InvasionType = null;
            }


            if (Beach.HasValue)
            {
                Main.LocalPlayer.ZoneBeach = Beach.Value;
                Beach = null;
            }
            if (Caverns.HasValue)
            {
                Main.LocalPlayer.ZoneRockLayerHeight = Caverns.Value;
                Caverns = null;
            }
            if (Corrupt.HasValue)
            {
                Main.LocalPlayer.ZoneCorrupt = Corrupt.Value;
                Corrupt = null;
            }
            if (Crimson.HasValue)
            {
                Main.LocalPlayer.ZoneCrimson = Crimson.Value;
                Crimson = null;
            }
            if (Desert.HasValue)
            {
                Main.LocalPlayer.ZoneDesert = Desert.Value;
                Desert = null;
            }
            if (Dungeon.HasValue)
            {
                Main.LocalPlayer.ZoneDungeon = Dungeon.Value;
                Dungeon = null;
            }

            if (Glowshroom.HasValue)
            {
                Main.LocalPlayer.ZoneGlowshroom = Glowshroom.Value;
                Glowshroom = null;
            }
            if (Hallow.HasValue)
            {
                Main.LocalPlayer.ZoneHallow = Hallow.Value;
                Hallow = null;
            }
            if (Hell.HasValue)
            {
                Main.LocalPlayer.ZoneUnderworldHeight = Hell.Value;
                Hell = null;
            }
            if (Jungle.HasValue)
            {
                Main.LocalPlayer.ZoneJungle = Jungle.Value;
                Jungle = null;
            }
            if (LihzhardTemple.HasValue)
            {
                Main.LocalPlayer.ZoneLihzhardTemple = LihzhardTemple.Value;
                LihzhardTemple = null;
            }
            if (Overworld.HasValue)
            {
                Main.LocalPlayer.ZoneOverworldHeight = Overworld.Value;
                Overworld = null;
            }
            if (Snow.HasValue)
            {
                Main.LocalPlayer.ZoneSnow = Snow.Value;
                Snow = null;
            }
            if (Space.HasValue)
            {
                Main.LocalPlayer.ZoneSkyHeight = Space.Value;
                Space = null;
            }
            if (UndergroundDesert.HasValue)
            {
                Main.LocalPlayer.ZoneUndergroundDesert = UndergroundDesert.Value;
                UndergroundDesert = null;
            }

        }




    }


}
