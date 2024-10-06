namespace EpicMMOSystem.LevelSystem;

public static class ZDOUtils
{
    private const string LevelKey = "EpicMMOKey";
    public static void SetMMOLevel(this Character? character, int level)
    {
        if (character == null) return;
        character.GetComponent<ZNetView>().GetZDO().Set(LevelKey, level);
    }

    public static int GetMMOLevel(this Character? character)
    {
        if (character == null) return 1;
        return character.GetComponent<ZNetView>()?.GetZDO()?.GetInt(LevelKey) ?? 1;
    }
}