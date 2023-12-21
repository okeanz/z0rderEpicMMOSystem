using System.Linq;
using fastJSON;
using UnityEngine;

namespace EpicMMOSystem;

public static class PlayerExtensions
{
    public static void DropPlayerKeys(this Player player)
    {
        EpicMMOSystem.MLLogger.LogWarning($"DropPlayerKeys before - Keys: {JSON.ToNiceJSON(player.GetUniqueKeys().ToArray())}");
        player.m_uniques.Clear();
        player.m_tempUniqueKeys.Clear();
        ZoneSystem.instance.UpdateWorldRates();
        player.UpdateEvents();
        EpicMMOSystem.MLLogger.LogWarning($"DropPlayerKeys after - Keys: {JSON.ToNiceJSON(player.GetUniqueKeys().ToArray())}");
    }

    public static void IncrementBossCounterKey(this Player player, string name)
    {
        var cleanName = name.Replace("(Clone)", "");
        EpicMMOSystem.MLLogger.LogWarning($"Increment counter for name {cleanName}");
        var keys = player.GetUniqueKeys();
        var existingIndex = keys.FindIndex(key => key.Contains(cleanName) && key.Contains("#"));
        EpicMMOSystem.MLLogger.LogWarning($"existingIndex {existingIndex}");
        if (existingIndex != -1)
        {
            var existingKey = keys[existingIndex];
            var counter = PlayerUtils.GetCounterFromKey(existingKey);
            player.RemoveUniqueKey(existingKey);
            player.AddUniqueKey($"{cleanName}#{counter + 1}");
        }
        else
        {
            player.AddUniqueKey($"{cleanName}#{1}");
        }
    }
}