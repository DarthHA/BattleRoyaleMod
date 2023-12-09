using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace BattleRoyaleMod.System
{

    public class ILEditing : ModSystem
    {
        public static BindingFlags UniversalBindingFlags => BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        public override void Load()
        {
            //MonoModHooks.Modify(typeof(NPCLoader).GetMethod("PreAI", UniversalBindingFlags), new ILContext.Manipulator(NPCPreAIChange));
            //MonoModHooks.Modify(typeof(NPCLoader).GetMethod("PostAI", UniversalBindingFlags), new ILContext.Manipulator(NPCPostAIChange));

            //MonoModHooks.Modify(typeof(ProjectileLoader).GetMethod("PreAI", UniversalBindingFlags), new ILContext.Manipulator(ProjPreAIChange));
            //MonoModHooks.Modify(typeof(ProjectileLoader).GetMethod("PostAI", UniversalBindingFlags), new ILContext.Manipulator(ProjPostAIChange));
        }
        public override void Unload()
        {
        }
        internal static void NPCPreAIChange(ILContext context)
        {
            try
            {
                ILCursor cursor = new(context);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate<Func<NPC, bool>>(npc =>
                {
                    GlobalHookList<GlobalNPC> list = (GlobalHookList<GlobalNPC>)typeof(NPCLoader).GetField("HookPreAI", UniversalBindingFlags).GetValue(null);

                    bool result = true;

                    foreach (GlobalNPC g in list.Enumerate())
                    {
                        if (g is TargetSelectNPC)
                            result &= g.PreAI(npc);
                    }

                    foreach (GlobalNPC g in list.Enumerate())
                    {
                        if (g is not TargetSelectNPC)
                            result &= g.PreAI(npc);
                    }

                    if (result && npc.ModNPC != null)
                    {
                        return npc.ModNPC.PreAI();
                    }
                    return result;
                });
                cursor.Emit(OpCodes.Ret);
            }
            catch
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<BattleRoyaleMod>(), context);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                // throw new ILPatchFailureException(ModContent.GetInstance<ExampleMod>(), il, e);
            }

        }

        internal static void NPCPostAIChange(ILContext context)
        {
            ILCursor cursor = new(context);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate(new Action<NPC>(npc =>
            {
                if (npc.ModNPC != null)
                {
                    npc.ModNPC.PostAI();
                }

                GlobalHookList<GlobalNPC> list = (GlobalHookList<GlobalNPC>)typeof(NPCLoader).GetField("HookPostAI", UniversalBindingFlags).GetValue(null);

                foreach (GlobalNPC g in list.Enumerate())
                {
                    if (g is not TargetSelectNPC)
                        g.PostAI(npc);
                }

                foreach (GlobalNPC g in list.Enumerate())
                {
                    if (g is TargetSelectNPC)
                        g.PostAI(npc);
                }

                return;
            }));
            cursor.Emit(OpCodes.Ret);
        }

        internal static void ProjPreAIChange(ILContext context)
        {
            try
            {
                ILCursor cursor = new(context);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate<Func<Projectile, bool>>(projectile =>
                {
                    GlobalHookList<GlobalProjectile> list = (GlobalHookList<GlobalProjectile>)typeof(ProjectileLoader).GetField("HookPreAI", UniversalBindingFlags).GetValue(null);

                    bool result = true;

                    foreach (GlobalProjectile g in list.Enumerate())
                    {
                        if (g is TargetSelectProj)
                            result &= g.PreAI(projectile);
                    }

                    foreach (GlobalProjectile g in list.Enumerate())
                    {
                        if (g is not TargetSelectProj)
                            result &= g.PreAI(projectile);
                    }
                    if (result && projectile.ModProjectile != null)
                    {
                        return projectile.ModProjectile.PreAI();
                    }
                    return result;
                });
                cursor.Emit(OpCodes.Ret);
            }

            catch
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                MonoModHooks.DumpIL(ModContent.GetInstance<BattleRoyaleMod>(), context);
            }
        }



        internal static void ProjPostAIChange(ILContext context)
        {
            ILCursor cursor = new(context);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<Projectile>>(proj =>
            {
                if (proj.ModProjectile != null)
                {
                    proj.ModProjectile.PostAI();
                }

                GlobalHookList<GlobalProjectile> list = (GlobalHookList<GlobalProjectile>)typeof(ProjectileLoader).GetField("HookPostAI", UniversalBindingFlags).GetValue(null);

                foreach (GlobalProjectile g in list.Enumerate())
                {
                    if (g is not TargetSelectProj)
                        g.PostAI(proj);
                }

                foreach (GlobalProjectile g in list.Enumerate())
                {
                    if (g is TargetSelectProj)
                        g.PostAI(proj);
                }

                return;
            });
            cursor.Emit(OpCodes.Ret);
        }
    }

}
