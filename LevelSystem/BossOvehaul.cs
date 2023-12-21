using System.Linq;
using CreatureLevelControl;
using fastJSON;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

[HarmonyPatch(typeof(BaseAI), nameof(BaseAI.Awake))]
public static class BossStart
{
    public static void Postfix(BaseAI __instance)
    {
        var dbMonsterName = __instance.gameObject.name;
        var monsterName = dbMonsterName.Replace("(Clone)", "");
        var isBoss = __instance.m_character?.IsBoss() ?? false;
        
        var maxCounter = isBoss ?
            PlayerUtils.GetMaxBossCounterInRange(__instance.transform.position,
                EpicMMOSystem.counterCheckDistance.Value, monsterName) : 0;

        if (__instance.m_character.GetMMOLevel() <= 1)
        {
            var targetLevel = DataMonsters.contains(dbMonsterName) ? DataMonsters.getLevel(dbMonsterName) : 0;
            if (isBoss && maxCounter != 0)
            {
                targetLevel += maxCounter * EpicMMOSystem.bossLevelCounterMultiplier.Value;
            }
        
            __instance.m_character.SetMMOLevel(targetLevel);
        }

        if (maxCounter > 1 && isBoss)
            CallOdinsWrath(monsterName, __instance.transform.position, maxCounter);
    }

    private static void CallOdinsWrath(string monsterName, Vector3 position, int counter)
    {
        EpicMMOSystem.MLLogger.LogWarning($"Calling Odin Wrath, ${monsterName}");
        var events = RandEventSystem.instance.m_events
            .Where(ev => ev.m_name.Contains("OW_") && ev.m_name.ToLower().Contains(monsterName.ToLower())).ToArray();
        if (events.Length == 0) return;
        EpicMMOSystem.MLLogger.LogWarning(
            $"Possible events, ${JSON.ToNiceJSON(events.Select(x => x.m_name).ToArray())}");
        var randomIndex = Random.Range(0, events.Length);

        var cloned = events[randomIndex].Clone();
        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, cloned.m_startMessage);
        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, cloned.m_endMessage);

        foreach (var spawner in cloned.m_spawn.Where(s => s.m_enabled).Take(counter - 1))
        {
            for (int i = 0; i < spawner.m_maxSpawned; i++)
            {
                var insideUnitCircle = Random.insideUnitCircle;
                var spawnPoint =
                    position + new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y) * spawner.m_spawnDistance;
                EpicMMOSystem.MLLogger.LogWarning($"spawning, ${spawner.m_name}; point: {spawnPoint}");
                var gameObject = Object.Instantiate(spawner.m_prefab, spawnPoint + Vector3.up * spawner.m_groundOffset, Quaternion.identity);
                var ai = gameObject.GetComponent<BaseAI>();
                ai.SetHuntPlayer(true);
                
                if (EpicMMOSystem.CLLCLoaded)
                {
                    var character = gameObject.GetComponent<Character>();
                    character.SetLevel(Random.Range(spawner.m_minLevel, spawner.m_maxLevel));
                    CreatureLevelControl.API.SetInfusionCreature(character);
                    CreatureLevelControl.API.SetExtraEffectCreature(character);
                }
            }
            
        }
    }
}