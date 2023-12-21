using System.Collections.Generic;
using System.Linq;
using fastJSON;
using UnityEngine;

namespace EpicMMOSystem;

internal static class PlayerUtils
{
    public static List<Player> GetPlayersInRadius(Vector3 point, float range)
    {
        List<Player> players = new();
        foreach (Player player in Player.GetAllPlayers())
        {
            if (Utils.DistanceXZ(player.transform.position, point) < range)
            {
                players.Add(player);
            }
        }

        return players;
    }

    public static List<ZDO> GetPlayerZdosInRadius(Vector3 point, float range)
    {
        List<ZDO> players = new();
        foreach (ZDO player in ZNet.instance.GetAllCharacterZDOS())
        {
            if (Utils.DistanceXZ(player.GetPosition(), point) < range)
            {
                players.Add(player);
            }
        }

        return players;
    }
    
    public static int GetMaxBossCounterInRange(Vector3 position, float range, string bossName)
    {
        var closestPlayers = GetPlayersInRadius(position, range);
        if (closestPlayers.Count == 0) return 0;

        var playersKeys = closestPlayers.SelectMany(player => player.GetUniqueKeys()).ToArray();
        if (playersKeys.Length == 0) return 0;

        var bossCounters = playersKeys.Where(key => key.Contains(bossName) && key.Contains("#")).ToArray();
        EpicMMOSystem.MLLogger.LogWarning($"[bossCounters] within 50m: {JSON.ToNiceJSON(bossCounters)}");
        if (bossCounters.Length == 0) return 0;

        return bossCounters.Select(GetCounterFromKey).Max();
    }

    public static int GetCounterFromKey(string key)
    {
        return int.Parse(key.Split('#').Last());
    }
}