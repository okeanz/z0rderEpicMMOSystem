using HarmonyLib;
using UnityEngine;
namespace EpicMMOSystem;

public partial class LevelSystem
{

    public float getAddAttackSpeed()
    {
        var parameter = getParameter(Parameter.Agility);
        var multiplayer = EpicMMOSystem.attackSpeed.Value;
        return parameter * multiplayer;
    }
    public float getAttackStamina()
    {
        var parameter = getParameter(Parameter.Agility);
        var multiplayer = EpicMMOSystem.attackStamina.Value;
        return parameter * multiplayer;
    }
    
    public float getStaminaReduction()
    {
        var parameter = getParameter(Parameter.Agility);
        var multiplayer = EpicMMOSystem.staminaReduction.Value;
        return parameter * multiplayer;
    }



     [HarmonyPatch(typeof(CharacterAnimEvent), "FixedUpdate")]
     private static class CharacterAnimEvent_Awake_Patch
     {
         private static void Prefix(CharacterAnimEvent __instance)
         {
             //Bows warning, can be OP easy regardless
             if (Player.m_localPlayer != __instance.m_character) return;
             if (!__instance.m_character.InAttack()) return;

            Player localPlayer = Player.m_localPlayer;
            GameObject val = localPlayer.GetCurrentWeapon()?.m_dropPrefab;
            var skilltype = localPlayer.GetCurrentWeapon().m_shared.m_skillType;
            //EpicMMOSystem.MLLogger.LogWarning(" normal speed " + __instance.m_animator.speed + " for " + skilltype);
           if (skilltype == Skills.SkillType.Bows) return; // no bows

           if (skilltype == Skills.SkillType.Unarmed)
            {
                var speed2 = Instance.getAddAttackSpeed() / 100 + 2; // unarmed special
                __instance.m_animator.speed = speed2;
                return;
            }
            var speed = Instance.getAddAttackSpeed() / 100 + 1;
             __instance.m_animator.speed = speed;
         }
    }


    [HarmonyPatch(typeof(Attack), nameof(Attack.GetAttackStamina))]
    public static class StaminaAttack_Patch
    {
        public static void Postfix(ref float __result)
        {
            var multi = 1 - Instance.getAttackStamina() / 100;
            __result *= multi;
        }
    }


    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyRunStaminaDrain))]
    public static class ModifyRun_Patch
    {
        public static void Postfix(ref float drain)
        {
            var multi = 1 - Instance.getStaminaReduction() / 100;
            drain *= multi;
        }
    }
    
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyJumpStaminaUsage))]
    public static class ModifyJump_Patch
    {
        public static void Postfix( ref float staminaUse)
        {
            var multi = 1 - Instance.getStaminaReduction() / 100 ;
            staminaUse *= multi;
        }
    } 
   
}