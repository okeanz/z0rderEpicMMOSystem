using System.Linq;
using fastJSON;
using UnityEngine;

namespace EpicMMOSystem.OdinWrath;

public static class BossCounterApi
{
    private const string UniqueKey = "LG_V1_";
    public static void IncrementBossCounterKey(this Player player, string name)
    {
        var cleanName = name.Replace("(Clone)", "");
        EpicMMOSystem.MLLogger.LogWarning($"Increment counter for name {cleanName}");
        var key = player.GetBossCounterKey(cleanName);
        EpicMMOSystem.MLLogger.LogWarning($"Previous key {key}");
        if (key != null)
        {
            var counter = GetCounterFromKey(key);
            player.RemoveUniqueKey(key);
            var newKey = $"{UniqueKey}{cleanName}#{counter + 1}";
            EpicMMOSystem.MLLogger.LogWarning($"New key {newKey}");
            player.AddUniqueKey(newKey);
        }
        else
        {
            player.AddUniqueKey($"{UniqueKey}{cleanName}#{1}");
        }
    }

    public static int GetBossCounterValue(this Player player, string bossName)
    {
        var key = player.GetBossCounterKey(bossName);
        return key == null ? 0 : GetCounterFromKey(key);
    }
    
    public static string? GetBossCounterKey(this Player player, string bossName)
    {
        var keys = player.GetUniqueKeys();
        EpicMMOSystem.MLLogger.LogWarning($"[GetBossCounterKey] Unique keys: {JSON.ToNiceJSON(keys)}");
        if (keys.Count == 0) return null;
        var key = keys.FirstOrDefault(key => key.Contains(bossName) && key.Contains(UniqueKey));
        EpicMMOSystem.MLLogger.LogWarning($"[GetBossCounterKey] key: {JSON.ToNiceJSON(keys)}");
        return key;
    }

    public static int GetMaxBossCounterInRange(Vector3 position, float range, string bossName)
    {
        var closestPlayers = PlayerUtils.GetPlayersInRadius(position, range);
        if (closestPlayers.Count == 0) return 0;

        var bossCounters = closestPlayers.Select(player => player.GetBossCounterValue(bossName)).ToArray();
        return bossCounters.Length == 0 ? 0 : bossCounters.Max();
    }

    public static int GetCounterFromKey(string key)
    {
        return int.Parse(key.Split('#').Last());
    }
}