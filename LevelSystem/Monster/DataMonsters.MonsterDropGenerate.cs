using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem.LevelSystem.Monster;

public static partial class DataMonsters
{
    [HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.GenerateDropList))]
    public static class MonsterDropGenerate
    {
        [HarmonyPriority(1)] // maybe stop epic loot? Last is 0, so 1 will be almost last for any other mod

        static void DropItem(GameObject prefab, Vector3 centerPos, float dropArea)  // Thx KG
        {
            Quaternion rotation = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
            Vector3 b = UnityEngine.Random.insideUnitSphere * dropArea;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, centerPos + b, rotation);
            Rigidbody component = gameObject.GetComponent<Rigidbody>();
            if (component)
            {
                Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
                if (insideUnitSphere.y < 0f)
                {
                    insideUnitSphere.y = -insideUnitSphere.y;
                }

                component.AddForce(insideUnitSphere * 5f, ForceMode.VelocityChange);
            }
        }

        public static void Postfix(CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
        {
            if (__instance.m_character.IsTamed() ) return;
            Heightmap.Biome biome = EnvMan.instance.m_currentBiome;

            float rand = Random.value;
            var dropChance = __instance.m_character.IsBoss() ? EpicMMOSystem.OrbDropChancefromBoss.Value : EpicMMOSystem.OrbDropChance.Value;
            var isBoss = __instance.m_character.IsBoss(); // could remove extra code


            // clear to add Magic orbs now // always orb chance
            if (OrbsByBiomes.TryGetValue(biome, out var orb) && rand <= dropChance / 100f)
            {
                if (isBoss)
                {
                    for (int i = 0; i < Random.Range(1, EpicMMOSystem.OrdDropMaxAmountFromBoss.Value); i++) // random amount 1-4
                    {
                        DropItem(orb, __instance.transform.position + Vector3.up * 0.75f, 0.5f);
                    }
                }
                else
                {
                    DropItem(orb, __instance.transform.position + Vector3.up * 0.75f, 0.5f);
                }
            }

            if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value || EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value || EpicMMOSystem.removeAllDropsFromNonPlayerKills.Value))
            {
                var playerLevel = __instance.m_character.m_nview.GetZDO().GetInt("epic playerLevel");

                if (playerLevel == 1000512)
                {
                    if (EpicMMOSystem.removeAllDropsFromNonPlayerKills.Value)
                    {
                        if (contains(__instance.m_character.gameObject.name))
                        {
                            __result = new(); // no drops from charcter related objects
                        }
                    }
                    playerLevel = 0;
                }

                if (!contains(__instance.m_character.gameObject.name)) return;
                if (playerLevel != 0)
                {                
                    // could just use isBoss above
                    var Regmob = true;
                    if (EpicMMOSystem.extraDebug.Value)
                        EpicMMOSystem.MLLogger.LogInfo("Player level " + playerLevel);
                    if (playerLevel > 0) // postive so boss
                    {
                        Regmob = false;
                    }
                    else // reg mobs
                    {
                        Regmob = true;
                        playerLevel = -playerLevel;
                    }

                    int maxLevelExp = playerLevel + EpicMMOSystem.maxLevelExp.Value;
                    int minLevelExp = playerLevel - EpicMMOSystem.minLevelExp.Value;
                    int monsterLevel = __instance.m_character.GetMMOLevel() + __instance.m_character.m_level - 1; // interesting that it's using m_char as well
                    if (__instance.m_character.GetMMOLevel() == 0)
                        return;

                    if ((monsterLevel > maxLevelExp) && (EpicMMOSystem.removeBossDropMax.Value && !Regmob || EpicMMOSystem.removeDropMax.Value && Regmob))
                    {
                        __result = new();
                        return;
                    }
                    if ((monsterLevel < minLevelExp) && (EpicMMOSystem.removeBossDropMin.Value && !Regmob || EpicMMOSystem.removeDropMin.Value && Regmob))
                    {
                        __result = new();
                        return;
                    }
                }
            }

        }
    }
}