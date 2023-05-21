using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace EpicMMOSystem;

public partial class LevelSystem
{
    public float getAddPhysicDamage()
    {
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.physicDamage.Value;
        return parameter * multiplayer;
    }
    
    public float getAddWeight()
    {
        if (!Player.m_localPlayer) return 0;
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.addWeight.Value;
        return parameter * multiplayer;
    }

    public float getReducedStaminaBlock()
    {
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.staminaBlock.Value;
        return parameter * multiplayer;
    }

    public float getAddCriticalDmg()
    {
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.critDmg.Value;
        return parameter * multiplayer;

    }



    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
    public class AddDamageStrength_Path
    {
        public static void Postfix(ref ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (Player.m_localPlayer == null) return;
            if (!Player.m_localPlayer.m_inventory.ContainsItem(__instance)) return;
            float add = Instance.getAddPhysicDamage();
            var value = add / 100 + 1;

            __result.m_blunt *= value;
            __result.m_slash *= value;
            __result.m_pierce *= value;
            __result.m_chop *= value;
            __result.m_pickaxe *= value;
        }
    }
    
    [HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]
    public class AddWeight_Path
    {
        static void Postfix(ref float __result)
        {
            var addWeight = Instance.getAddWeight() + EpicMMOSystem.addDefaultWeight.Value;
            __result += (float)Math.Round(addWeight);
        }
    }

    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
    static class Humanoid_BlockAttack_Patch
    {
        private static float ReturnMyValue()
        {
            return 1 - Instance.getReducedStaminaBlock() / 100;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> StaminaBlock(IEnumerable<CodeInstruction> code)
        {

            var method = AccessTools.DeclaredMethod(typeof(Character), nameof(Character.UseStamina));
            var MyMethod = AccessTools.DeclaredMethod(typeof(Humanoid_BlockAttack_Patch), nameof(ReturnMyValue));
            foreach (var instruction in code)
            {
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand == method)
                {
                    yield return new CodeInstruction(OpCodes.Call, MyMethod);
                    yield return new CodeInstruction(OpCodes.Mul);
                }
                yield return instruction;

            }
        }
    }


}