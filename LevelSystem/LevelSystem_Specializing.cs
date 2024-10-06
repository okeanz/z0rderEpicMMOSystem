using HarmonyLib;

namespace EpicMMOSystem.LevelSystem;

public partial class LevelSystem
{

    public float getaddMiningDmg()
    {
        var parameter = getParameter(Parameter.Special);
        var multiplayer = EpicMMOSystem.miningSpeed.Value;
        return parameter * multiplayer;
    }

    public float getAddPieceHealth()
    {
        var parameter = getParameter(Parameter.Special);
        var multiplayer = EpicMMOSystem.constructionPieceHealth.Value;
        return parameter * multiplayer;
    }

    public float getAddTreeCuttingDmg() 
    {
        var parameter = getParameter(Parameter.Special);
        var multiplayer = EpicMMOSystem.treeCuttingSpeed.Value;
        return parameter * multiplayer;
    }





    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), new[] { typeof(int), typeof(float) }) ]
    private static class MiningPostfix
    {
        private static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (__instance != null && __instance.m_shared.m_skillType == Skills.SkillType.Pickaxes) // don't really care about mobs weak to pickaxe, StoneGolems
            {
                float num = 1f + (Instance.getaddMiningDmg() / 100f);
                __result.m_pickaxe *= num;
            }
        }
    }


    [HarmonyPatch(typeof(WearNTear), "OnPlaced")]
    internal static class Player_HealthChange
    {
        internal static void Prefix(ref WearNTear __instance)
        {
            //piece.gameObject.GetComponent<WearNTear>().m_health
            __instance.m_health = Instance.getAddPieceHealth() + __instance.m_health;
            //EpicMMOSystem.MLLogger.LogWarning("health "+ __instance.m_health);

        }
    }



    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), new[] { typeof(int), typeof(float) })]
    private static class TreeCuttingPostfix
    {
        private static void Postfix(ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (__instance != null && (__instance.m_shared.m_skillType == Skills.SkillType.Axes || __instance.m_shared.m_skillType == Skills.SkillType.WoodCutting)) 
                // need to make sure it's only being used for wood not mob slaying // no idea where that is checked
            {
                
                float num = 1f + (Instance.getAddTreeCuttingDmg() / 100f);
                __result.m_chop *= num;
            }
        }
    }

}