using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace EpicMMOSystem;

public partial class LevelSystem
{

    public float getaddMiningSpeed()
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

    public float getAddTreeCuttingSpeed()
    {
        var parameter = getParameter(Parameter.Special);
        var multiplayer = EpicMMOSystem.treeCuttingSpeed.Value;
        return parameter * multiplayer;
    }

}