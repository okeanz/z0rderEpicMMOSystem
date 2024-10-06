using HarmonyLib;

namespace EpicMMOSystem.LevelSystem;

public partial class LevelSystem // Body or Endurance
{

    public float getStaminaRegen()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.staminaRegen.Value;
        return parameter * multiplayer;
    }

    public float getAddStamina()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.addStamina.Value;
        return parameter * multiplayer;
    }

    public float getAddPhysicArmor()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.physicArmor.Value;
        return parameter * multiplayer;
    }



    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
    public static class RegenStamina_Patch
    {
        public static void Postfix(SEMan __instance, ref float staminaMultiplier)
        {
            if (__instance.m_character.IsPlayer())
            {
                float add = Instance.getStaminaRegen();
                staminaMultiplier += add / 100;
            }
        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
    public static class AddStamina_Path
    {
        public static void Postfix(ref float stamina)
        {
            stamina += Instance.getAddStamina();
        }
    }


    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public static class PhysicArmor_Path
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!__instance.IsPlayer()) return;
            if (hit.GetAttacker() == __instance) return;

            float add = Instance.getAddPhysicArmor();
            var value = 1 - add / 100;

            hit.m_damage.m_blunt *= value;
            hit.m_damage.m_slash *= value;
            hit.m_damage.m_pierce *= value;
            hit.m_damage.m_chop *= value;
            hit.m_damage.m_pickaxe *= value;
        }
    }

}