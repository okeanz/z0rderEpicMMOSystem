using System;
using System.Collections.Generic;
using BepInEx;
using EpicMMOSystem.Gui;
using EpicMMOSystem.OdinWrath;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EpicMMOSystem.LevelSystem;

public enum Parameter
{
    Strength = 0,
    Agility = 1,
    Intellect = 2,
    Body = 3,
    Vigour = 4,
    Special = 5 // Strength, Dexterity, Intelligence, Endurance, Vigour, Specializing
}

public partial class LevelSystem
{
    #region Singlton

    private static LevelSystem instance;

    public static LevelSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelSystem();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private Dictionary<int, long> levelsExp;
    private const string pluginKey = EpicMMOSystem.ModName;
    private const string midleKey = "LevelSystem";
    private int[] depositPoint = {0, 0, 0, 0, 0, 0}; //6
    private float singleRate = 1f;

    private const string CurrentExpKey = $"{pluginKey}_{midleKey}_CurrentExp";
    private const string CurrentLevelKey = $"{pluginKey}_{midleKey}_Level";

    public LevelSystem()
    {
        FillLevelsExp();
    }

    public int getLevel()
    {
        if (!Player.m_localPlayer) return 1;
        if (!Player.m_localPlayer.m_knownTexts.ContainsKey(CurrentLevelKey))
        {
            return 1;
        }

        return int.Parse(Player.m_localPlayer.m_knownTexts[CurrentLevelKey]);
    }

    private void setLevel(int value)
    {
        if (!Player.m_localPlayer) return;
        Player.m_localPlayer.m_knownTexts[CurrentLevelKey] = value.ToString();
    }


    private long CurrentExpFallback()
    {
        if (!Player.m_localPlayer) return 1;
        try
        {
            Player.m_localPlayer.m_knownTexts[CurrentExpKey] = "1";
            return 1;
        }
        catch (Exception e)
        {
            EpicMMOSystem.MLLogger.LogError("Cannot write currentExp");
        }

        return 1;
    }

    public long getCurrentExp()
    {
        if (!Player.m_localPlayer) return 0;
        if (
            !Player.m_localPlayer.m_knownTexts.ContainsKey(CurrentExpKey) ||
            !int.TryParse(Player.m_localPlayer.m_knownTexts[CurrentExpKey], out var currentExp)
        )
        {
            return CurrentExpFallback();
        }

        return currentExp;
    }

    private void setCurrentExp(long value)
    {
        if (!Player.m_localPlayer) return;
        Player.m_localPlayer.m_knownTexts[CurrentExpKey] = value.ToString();
    }
    
    private void setParameter(Parameter parameter, int value, Player target = null)
    {
        var player = target == null ? Player.m_localPlayer : target;
        if (!player) return;
        // int max = EpicMMOSystem.maxValueAttribute.Value;

        int max = parameter.ToString() switch
        {
            "Strength" => EpicMMOSystem.maxValueStrength.Value,
            "Agility" => EpicMMOSystem.maxValueDexterity.Value,
            "Intellect" => EpicMMOSystem.maxValueIntelligence.Value,
            "Body" => EpicMMOSystem.maxValueEndurance.Value,
            "Vigour" => EpicMMOSystem.maxValueVigour.Value,
            "Special" => EpicMMOSystem.maxValueSpecializing.Value,
            _ => max = 205
        };
        var setValue = Mathf.Clamp(value, 0, max);
        player.m_knownTexts[$"{pluginKey}_{midleKey}_{parameter.ToString()}"] = setValue.ToString();
        // Player.m_localPlayer.m_customData. maybe later
    }

    public int getParameter(Parameter parameter)
    {
        if (!Player.m_localPlayer) return 0;
        if (!Player.m_localPlayer.m_knownTexts.ContainsKey($"{pluginKey}_{midleKey}_{parameter.ToString()}"))
        {
            return 0;
        }

        int value = int.Parse(Player.m_localPlayer.m_knownTexts[$"{pluginKey}_{midleKey}_{parameter.ToString()}"]);
        //int max = EpicMMOSystem.maxValueAttribute.Value;

        int max = parameter.ToString() switch
        {
            "Strength" => EpicMMOSystem.maxValueStrength.Value,
            "Agility" => EpicMMOSystem.maxValueDexterity.Value,
            "Intellect" => EpicMMOSystem.maxValueIntelligence.Value,
            "Body" => EpicMMOSystem.maxValueEndurance.Value,
            "Vigour" => EpicMMOSystem.maxValueVigour.Value,
            "Special" => EpicMMOSystem.maxValueSpecializing.Value,
            _ => max = 205
        };
        return Mathf.Clamp(value, 0, max);
    }

    public int getFreePoints()
    {
        var levelPoint = EpicMMOSystem.freePointForLevel.Value;
        var freePoint = EpicMMOSystem.startFreePoint.Value;
        var level = getLevel();
        int addPoints = 0;
        try
        {
            string str = EpicMMOSystem.levelsForBinusFreePoint.Value;
            if (str.IsNullOrWhiteSpace())
            {
            }
            else
            {
                var map = str.Split(',');
                foreach (var value in map)
                {
                    var keyValue = value.Split(':');
                    if (Int32.Parse(keyValue[0]) <= level)
                    {
                        addPoints += Int32.Parse(keyValue[1]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            EpicMMOSystem.print($"Free point, bonus error: {e.Message}");
        }


        var total = level * levelPoint + freePoint + addPoints;
        int usedUp = 0;
        for (int i = 0; i < EpicMMOSystem.numofCats; i++)
        {
            usedUp += getParameter((Parameter) i);
        }


        return total - usedUp;
    }

    public void addPointsParametr(Parameter parameter, int addPoint)
    {
        var freePoint = getFreePoints();
        if (!(freePoint > 0)) return;
        var applyPoint = Mathf.Clamp(addPoint, 1, freePoint);
        depositPoint[(int) parameter] += applyPoint;
        var currentPoint = getParameter(parameter);
        setParameter(parameter, currentPoint + applyPoint);
    }

    public long getNeedExp(int addLvl = 0)
    {
        var lvl = Mathf.Clamp(getLevel() + 1 + addLvl, 1, EpicMMOSystem.maxLevel.Value);
        return levelsExp[lvl];
    }

    public void ResetAllParameter(Player player = null)
    {
        for (int i = 0; i < EpicMMOSystem.numofCats; i++)
        {
            setParameter((Parameter) i, 0, player);
        }

        MyUI.UpdateParameterPanel();
    }

    public void ResetAllParameterPayment()
    {
        var text = EpicMMOSystem.prefabNameCoins.Value;
        var pref = ZNetScene.instance.GetPrefab(text);
        var name = pref.GetComponent<ItemDrop>().m_itemData.m_shared.m_name;
        var price = getPriceResetPoints();
        var currentCoins = Player.m_localPlayer.m_inventory.CountItems(name);
        if (currentCoins < price)
        {
            var prefTrophy = ZNetScene.instance.GetPrefab("ResetTrophy");
            var TrophyName = prefTrophy.GetComponent<ItemDrop>().m_itemData;
            var TrophySharedname = TrophyName.m_shared.m_name;
            var currentTrophy = Player.m_localPlayer.m_inventory.CountItems(TrophySharedname);
            if (currentTrophy > 0)
            {
                Player.m_localPlayer.m_inventory.RemoveItem(TrophySharedname, 1);
                ResetAllParameter();
                return;
            }

            return; // return if no coins and no ResetTrophy
        }

        Player.m_localPlayer.m_inventory.RemoveItem(name, price);
        ResetAllParameter();
    }

    public void SetSingleRate(float rate)
    {
        singleRate = rate;
    }

    public void AddExp(int addAmount, bool noxpMulti = false)
    {
        if (addAmount < 1)
        {
            if (addAmount == -2)
            {
                Util.FloatingText($"No XP for red/blue creatures :( ");
            }

            return;
        }


        if ((!ZoneSystem.instance.CheckKey("defeated_eikthyr") && getLevel() >= 15) ||
            (!ZoneSystem.instance.CheckKey("defeated_gdking") && getLevel() >= 25) ||
            (!ZoneSystem.instance.CheckKey("defeated_bonemass") && getLevel() >= 35) ||
            (!ZoneSystem.instance.CheckKey("defeated_dragon") && getLevel() >= 45) ||
            (!ZoneSystem.instance.CheckKey("defeated_goblinking") && getLevel() >= 55)||
            (!ZoneSystem.instance.CheckKey("defeated_queen") && getLevel() >= 65)
           )
        {
            Util.FloatingText($"Достигнут максимальный уровень прогресса");
            return;
        }
        

        var rate = EpicMMOSystem.rateExp.Value;
        var giveExp = noxpMulti ? addAmount : addAmount * (rate + singleRate);

        if (!noxpMulti)
        {
            if (Player.m_localPlayer.m_seman.HaveStatusEffect("Potion_MMO_Greater".GetStableHashCode()))
            {
                giveExp *= EpicMMOSystem.XPforGreaterPotion.Value;
            }
            else if (Player.m_localPlayer.m_seman.HaveStatusEffect("Potion_MMO_Medium".GetStableHashCode()))
            {
                giveExp *= EpicMMOSystem.XPforMediumPotion.Value;
            }
            else if (Player.m_localPlayer.m_seman.HaveStatusEffect("Potion_MMO_Minor".GetStableHashCode()))
            {
                giveExp *= EpicMMOSystem.XPforMinorPotion.Value;
            }
        }
        

        var current = getCurrentExp();
        var need = getNeedExp();

        if (current + giveExp > need)
        {
            AddLevel(1);
            setCurrentExp(0);
        }
        else
        {
            setCurrentExp(current + (long)giveExp);
        }

        
        MyUI.updateExpBar();
        
        if (EpicMMOSystem.leftMessageXP.Value)
        {
            Player.m_localPlayer.Message(
                MessageHud.MessageType.TopLeft,
                $"{(EpicMMOSystem.localizationold["$get_exp"])}: {(long) giveExp}"
            );
        }

        var xpStringTemplate = EpicMMOSystem.XPstring.Value;
        var xpString = xpStringTemplate.Replace("@", $"{(long)giveExp}");
        Util.FloatingText($"{xpString}");
    }

    public void AddLevel(int toAdd)
    {
        if (toAdd <= 0) return;
        var current = getLevel();
        var target = current + toAdd;
        setLevel(Mathf.Clamp(target, 1, EpicMMOSystem.maxLevel.Value));
        PlayerFVX.levelUp();
        var zdo = Player.m_localPlayer.m_nview.GetZDO();
        zdo.Set($"{pluginKey}_level", target);
        ZDOMan.instance.ForceSendZDO(zdo.m_uid);
    }

    public bool hasDepositPoints()
    {
        bool result = false;
        foreach (var deposit in depositPoint)
        {
            if (deposit > 0)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public void applyDepositPoints()
    {
        for (int i = 0; i < depositPoint.Length; i++)
        {
            depositPoint[i] = 0;
        }

        MyUI.UpdateParameterPanel();
    }

    public void cancelDepositPoints()
    {
        if (!Player.m_localPlayer) return;
        for (int i = 0; i < depositPoint.Length; i++)
        {
            if (depositPoint[i] == 0) continue;
            var parameter = (Parameter) i;
            var deposit = depositPoint[i];
            var point = getParameter(parameter);
            setParameter(parameter, point - deposit);
            depositPoint[i] = 0;
        }

        MyUI.UpdateParameterPanel();
    }

    public int getPriceResetPoints()
    {
        var count = 0;
        for (int i = 0; i < EpicMMOSystem.numofCats; i++)
        {
            count += getParameter((Parameter) i);
        }

        count *= EpicMMOSystem.priceResetPoints.Value;
        return count;
    }

    public void DeathPlayer(Player player)
    {
        if (EpicMMOSystem.hardcoreDeath.Value)
        {
            EpicMMOSystem.MLLogger.LogInfo($"Player {player.GetPlayerName()} is hardcored!");
            SetLevelTargetPlayer(player, 1);
            player.DropPlayerKeys();
            player.m_knownTexts.Clear();
            player.m_knownStations.Clear();
            player.m_inventory.RemoveAll();
            player.m_skills.LowerAllSkills(0.25f);
            return;
        }

        if (!EpicMMOSystem.lossExp.Value) return;
        if (!Player.m_localPlayer.HardDeath()) return;

        var minExp = EpicMMOSystem.minLossExp.Value;
        var maxExp = EpicMMOSystem.maxLossExp.Value;
        var lossExp = 1f - Random.Range(minExp, maxExp);

        var currentExp = getCurrentExp();
        long newExp = (long) (currentExp * lossExp);
        setCurrentExp(newExp);// remove some totalexp as well
        MyUI.updateExpBar();
    }

    //FillLevelExp
    public void FillLevelsExp()
    {
        var levelExp = EpicMMOSystem.levelExp.Value;
        var multiply = EpicMMOSystem.multiNextLevel.Value;
        var maxLevel = EpicMMOSystem.maxLevel.Value;
        levelsExp = new();
        if (EpicMMOSystem.levelexpforeachlevel.Value)
        {
            long current = 0;
            for (int i = 1; i <= maxLevel; i++)
            {
                current = (long) Math.Round(current * multiply + levelExp);
                levelsExp[i + 1] = current;
            }
        }
        else
        {
            long current = levelExp;
            for (int i = 1; i <= maxLevel; i++)
            {
                current = (long) Math.Round(current * multiply);
                levelsExp[i + 1] = current;
            }
        }
    }

    //Terminal
    public void terminalSetLevel(int value)
    {
        var level = Mathf.Clamp(value, 1, EpicMMOSystem.maxLevel.Value);
        setLevel(level);
        setCurrentExp(0);
        ResetAllParameter();
        PlayerFVX.levelUp();
        MyUI.updateExpBar();
        var zdo = Player.m_localPlayer.m_nview.GetZDO();
        zdo.Set($"{pluginKey}_level", level);
        ZDOMan.instance.ForceSendZDO(zdo.m_uid);
    }

    public void SetLevelTargetPlayer(Player player, int value)
    {
        var level = Mathf.Clamp(value, 1, EpicMMOSystem.maxLevel.Value);
        player.m_knownTexts[CurrentLevelKey] = $"{level}";
        player.m_knownTexts[$"{pluginKey}_{midleKey}_CurrentExp"] = "0";
        ResetAllParameter(player);
        PlayerFVX.levelUp();
        MyUI.updateExpBar();

        var zdo = player.m_nview.GetZDO();
        zdo.Set($"{pluginKey}_level", level);
        ZDOMan.instance.ForceSendZDO(zdo.m_uid);
    }
}

[HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))]
[HarmonyPriority(1000)]
public static class SetZDOLevel
{
    public static void Postfix()
    {
        var zdo = Player.m_localPlayer.m_nview.GetZDO();
        zdo.Set($"{EpicMMOSystem.ModName}_level", LevelSystem.Instance.getLevel());
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
public static class Death
{
    [HarmonyPriority(Priority.First)]
    public static void Prefix(Player __instance)
    {
        LevelSystem.Instance.DeathPlayer(__instance);
    }
}