using System.Linq;
using fastJSON;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem.OdinWrath;

[HarmonyPatch(typeof(BaseAI), nameof(BaseAI.Awake))]
public static class BossStart
{
    public static void Postfix(BaseAI __instance)
    {
        var dbMonsterName = __instance.gameObject.name;
        var monsterName = dbMonsterName.Replace("(Clone)", "");
        var character = __instance.m_character;
        if (!character) return;

        var isBoss = character.IsBoss();

        var maxCounter = isBoss
            ? BossCounterApi.GetMaxBossCounterInRange(__instance.transform.position,
                EpicMMOSystem.counterCheckDistance.Value, monsterName)
            : 0;

        var targetLevel = DataMonsters.contains(dbMonsterName) ? DataMonsters.getLevel(dbMonsterName) : 0;
        if (character.GetMMOLevel() <= 1)
        {
            if (isBoss && maxCounter != 0)
            {
                var baseLevel = targetLevel;
                targetLevel += maxCounter * EpicMMOSystem.bossLevelCounterMultiplier.Value;
                var mmoLevelRatio = targetLevel / baseLevel;
                character.SetLevel(character.GetLevel() + maxCounter * 3);
                character.SetMaxHealth(character.GetMaxHealth() * mmoLevelRatio);
            }

            character.SetMMOLevel(targetLevel);
        }

        if (maxCounter > 1 && isBoss)
            CallOdinsWrath(monsterName, character.transform.position, maxCounter, targetLevel);
    }

    private static void CallOdinsWrath(string monsterName, Vector3 position, int counter, int mmoLevel)
    {
        EpicMMOSystem.MLLogger.LogWarning($"Calling Odin Wrath, ${monsterName}");
        var events = RandEventSystem.instance.m_events
            .Where(ev => ev.m_name.Contains("OW_") && ev.m_name.ToLower().Contains(monsterName.ToLower())).ToArray();
        if (events.Length == 0) return;
        EpicMMOSystem.MLLogger.LogWarning(
            $"Possible events, ${JSON.ToNiceJSON(events.Select(x => x.m_name).ToArray())}");
        var randomIndex = Random.Range(0, events.Length);

        var cloned = events[randomIndex].Clone();
        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, cloned.m_endMessage);

        var spawners = cloned.m_spawn.Where(s => s.m_enabled).Take(counter - 1).ToArray();

        foreach (var spawner in spawners)
        {
            for (int i = 0; i < spawner.m_maxSpawned; i++)
            {
                var insideUnitCircle = Random.insideUnitCircle;
                var spawnPoint =
                    position + new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y) * spawner.m_spawnDistance;
                EpicMMOSystem.MLLogger.LogWarning($"spawning, ${spawner.m_name}; point: {spawnPoint}");
                var gameObject = Object.Instantiate(spawner.m_prefab, spawnPoint + Vector3.up * spawner.m_groundOffset,
                    Quaternion.identity);
                var ai = gameObject.GetComponent<BaseAI>();
                ai.SetHuntPlayer(true);

                var character = gameObject.GetComponent<Character>();
                character.m_faction = Character.Faction.Boss;
                var baseLevel = character.GetMMOLevel();
                var mmoLevelRatio = mmoLevel / baseLevel;
                character.SetMMOLevel(mmoLevel - 5);
                character.SetLevel(Random.Range(spawner.m_minLevel, spawner.m_maxLevel) + counter * 2);
                character.SetMaxHealth(character.GetMaxHealth() * mmoLevelRatio);
                var characterDrop = gameObject.GetComponent<CharacterDrop>();
                if (characterDrop != null)
                {
                    characterDrop.m_drops.Clear();
                }


                if (EpicMMOSystem.CLLCLoaded)
                {
                    CreatureLevelControl.API.SetInfusionCreature(character);
                    CreatureLevelControl.API.SetExtraEffectCreature(character);
                }
            }
        }
    }
}