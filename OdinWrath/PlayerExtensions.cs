using fastJSON;

namespace EpicMMOSystem.OdinWrath;

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
}