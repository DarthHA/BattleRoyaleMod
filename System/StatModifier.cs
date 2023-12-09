using Terraria;
using Terraria.ModLoader;

namespace BattleRoyaleMod
{
    public class NPCStatModifier : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool Modified = false;

        public override void PostAI(NPC npc)
        {
            if (BattleRoyaleMod.Gconfig.TakeEffectInstantly || npc.GetFraction() != -1)
            {
                ModifyStat(npc);
            }
        }

        public static void ModifyStat(NPC entity, bool SD = false)
        {
            if (!entity.GetGlobalNPC<NPCStatModifier>().Modified)
            {
                int originalLifeMax = entity.lifeMax;
                entity.GetGlobalNPC<NPCStatModifier>().Modified = true;
                entity.defDamage = entity.damage = (int)(entity.damage * ConfigDataUtils.GetDamageModifier(entity));
                entity.lifeMax = (int)(entity.lifeMax * ConfigDataUtils.GetLifeModifier(entity));
                entity.defDefense = entity.defense = (int)(entity.defense * ConfigDataUtils.GetDefenseModifier(entity));
                if (!SD)
                {
                    entity.defDamage = (int)(entity.defDamage * ConfigDataUtils.GetDamageModifier(entity));
                    entity.defDefense = (int)(entity.defDefense * ConfigDataUtils.GetDefenseModifier(entity));
                    entity.life = (int)(entity.life / (float)originalLifeMax * entity.lifeMax);
                }
            }
        }
    }


    public class ProjStatModifier : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool Modified = false;

        public override void PostAI(Projectile projectile)
        {
            if (BattleRoyaleMod.Gconfig.TakeEffectInstantly || projectile.GetFraction() != -1)
            {
                ModifyStat(projectile);
            }
        }

        public static void ModifyStat(Projectile entity, bool SD = false)
        {
            if (!entity.GetGlobalProjectile<ProjStatModifier>().Modified)
            {
                entity.GetGlobalProjectile<ProjStatModifier>().Modified = true;
                entity.damage = (int)(entity.damage * ConfigDataUtils.GetDamageModifier(entity));
                if (!SD)
                {
                    entity.originalDamage = (int)(entity.originalDamage * ConfigDataUtils.GetDamageModifier(entity));
                }
            }
        }
    }
}
