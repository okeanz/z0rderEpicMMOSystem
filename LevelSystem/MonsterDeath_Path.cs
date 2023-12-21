using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Groups;
using HarmonyLib;
using ItemManager;
using UnityEngine;
//using UnityEngine.UIElements;



namespace EpicMMOSystem;

public static class MonsterDeath_Path
{
    private static readonly Dictionary<Character, long> CharacterLastDamageList = new();



    [HarmonyPatch(typeof(Game), nameof(Game.Start))]
    public static class RegisterRpc
    {
        
        public static void Postfix()
        {
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} DeadMonsters", new Action<long, ZPackage>(RPC_DeadMonster));
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} AddGroupExp", new Action<long, int, Vector3, int>(RPC_AddGroupExp));
        }
    }


    public static void RPC_AddGroupExp(long sender, int exp, Vector3 position, int monsterLevel)
    {
        if (EpicMMOSystem.extraDebug.Value)
            EpicMMOSystem.MLLogger.LogInfo("Player was in group so applying exp from group kill");

        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= EpicMMOSystem.groupRange.Value) return;

        var playerExp = exp;
        var MobisBoss = false;
        if (monsterLevel < 0)
        {
            MobisBoss = true;
            monsterLevel = -monsterLevel; // or -monsterLevel
        }

        if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.curveExp.Value || MobisBoss && EpicMMOSystem.curveBossExp.Value || EpicMMOSystem.noExpPastLVL.Value) && monsterLevel != 0)
        {
            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo("Checking player lvl for group exp");

            int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            if (monsterLevel > maxRangeLevel)
            {
                if (EpicMMOSystem.noExpPastLVL.Value)
                    playerExp = 0; // no exp
                else if (EpicMMOSystem.curveExp.Value)
                    playerExp = Convert.ToInt32(exp / (monsterLevel - maxRangeLevel));
                else if (MobisBoss && EpicMMOSystem.curveBossExp.Value)
                    playerExp = Convert.ToInt32(exp / (monsterLevel - maxRangeLevel));
            }
            int minRangeLevel = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
            if (monsterLevel < minRangeLevel)
            {
                if (EpicMMOSystem.noExpPastLVL.Value)
                    playerExp = 0; // no exp
                else if (EpicMMOSystem.curveExp.Value)
                    playerExp = Convert.ToInt32(exp / (minRangeLevel - monsterLevel));
                else if (MobisBoss && EpicMMOSystem.curveBossExp.Value)
                    playerExp = Convert.ToInt32(exp / (minRangeLevel - monsterLevel));
            }

            if (monsterLevel > maxRangeLevel && EpicMMOSystem.mentor.Value)
                playerExp = exp; // give full *group exp with mentor mode
        }


        LevelSystem.Instance.AddExp(playerExp);
    }
    

    public static void RPC_DeadMonster(long sender, ZPackage pkg)
    {
        if (!Player.m_localPlayer) return;
        if(Player.m_localPlayer.IsDead()) return;
        string monsterName = pkg.ReadString();
        int level = pkg.ReadInt();
        Vector3 position = pkg.ReadVector3();
        var isBoss = pkg.ReadBool();
        var monsterLevel = pkg.ReadInt();
        
        EpicMMOSystem.MLLogger.LogDebug($"isBoss = {isBoss}, name = {monsterName}");
        
        if (isBoss)
        {
            Player.m_localPlayer.IncrementBossCounterKey(monsterName);
        }


        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= EpicMMOSystem.playerRange.Value) return;

        int expMonster = DataMonsters.getExp(monsterName);
        int maxExp = DataMonsters.getMaxExp(monsterName);
        float lvlExp = EpicMMOSystem.expForLvlMonster.Value;
        var resultExp = expMonster + (maxExp * lvlExp * (level - 1));
        var exp = Convert.ToInt32(resultExp);
        var playerExp = exp;


        if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.curveExp.Value || isBoss && EpicMMOSystem.curveBossExp.Value || EpicMMOSystem.noExpPastLVL.Value) && monsterLevel != 0)
        {
            if (EpicMMOSystem.extraDebug.Value) 
                EpicMMOSystem.MLLogger.LogInfo("Checking player lvl");

            int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            if (monsterLevel > maxRangeLevel)
            {
                if (EpicMMOSystem.noExpPastLVL.Value)
                    playerExp = -2;// no exp
                else if (EpicMMOSystem.curveExp.Value)
                    playerExp = Convert.ToInt32(exp / (monsterLevel - maxRangeLevel));
                else if (isBoss && EpicMMOSystem.curveBossExp.Value)
                    playerExp = Convert.ToInt32(exp / (monsterLevel - maxRangeLevel));
            }
            int minRangeLevel = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
            if (monsterLevel < minRangeLevel)
            {
                if (EpicMMOSystem.noExpPastLVL.Value)
                    playerExp = -2; // no exp
                else if (EpicMMOSystem.curveExp.Value)
                    playerExp = Convert.ToInt32( exp / (minRangeLevel - monsterLevel));
                else if (isBoss && EpicMMOSystem.curveBossExp.Value)
                    playerExp = Convert.ToInt32(exp / (minRangeLevel - monsterLevel));
            }
        }

        LevelSystem.Instance.AddExp(playerExp);
        if (!Groups.API.IsLoaded()) return;

        if (EpicMMOSystem.extraDebug.Value)
            EpicMMOSystem.MLLogger.LogInfo("Player in Group");

        //Convert Monsterlvl to negative if boss because max send amount is 3 para
        if (isBoss && monsterLevel != 0)
            monsterLevel = -1 * monsterLevel;

        var groupFactor = EpicMMOSystem.groupExp.Value;
        foreach (var playerReference in Groups.API.GroupPlayers())
        {
            if (playerReference.name != Player.m_localPlayer.GetPlayerName())
            {
                var sendExp = exp * groupFactor;
                ZRoutedRpc.instance.InvokeRoutedRPC(
                    playerReference.peerId, 
                    $"{EpicMMOSystem.ModName} AddGroupExp", 
                    new object[] { (int)sendExp, position, monsterLevel }
                    );
            }
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
    public static class ModifierDamage
    {
        public static void Prefix(Character __instance, HitData hit) // maybe check for tames as well to prevent them from hurting high lvl
        {
            if (!EpicMMOSystem.enabledLevelControl.Value) return;
            //if (EpicMMOSystem.removeDrop.Value) hit.m_toolTier = LevelSystem.Instance.getLevel(); // using toolTier to pass the Lvl of player
            hit.m_toolTier = (short)LevelSystem.Instance.getLevel();
            if (EpicMMOSystem.lowDamageLevel.Value)
            {
                if (__instance.IsPlayer()) return;
                if (__instance.IsTamed()) return;
                if (__instance.GetMMOLevel() == 0) return;

                if (!DataMonsters.contains(__instance.gameObject.name)) return;
                var playerLevel = LevelSystem.Instance.getLevel();
                var maxLevelExp = playerLevel + EpicMMOSystem.maxLevelExp.Value +EpicMMOSystem.lowDamageExtraConfig.Value;
                var monsterLevel = __instance.GetMMOLevel() + __instance.m_level - 1;
                if (monsterLevel > maxLevelExp)
                {
                    var damageFactor = Mathf.Clamp( (float)((playerLevel + EpicMMOSystem.lowDamageExtraConfig.Value) / monsterLevel),0.1f, 1.0f );
                    hit.ApplyModifier(damageFactor);
                }
            }
        }
    }
    
        
    
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    static class QuestEnemyKill
    {
        static void Prefix(Character __instance, long sender, HitData hit)
        {
            if (__instance.GetHealth() <= 0) return;
            bool BossDropFlag = __instance.GetFaction() == Character.Faction.Boss;
            var attacker = hit.GetAttacker();
            //attacker. faction check Guilds API
            if (attacker)
            {
                if (attacker.IsPlayer() || attacker.IsTamed() && EpicMMOSystem.tamesGiveXP.Value) // simple, but will have to come back to this tamed check
                {
                    CharacterLastDamageList[__instance] = sender;
                    if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value) && BossDropFlag)// removeboss drop and is a boss
                    {
                        __instance.m_nview.GetZDO().Set("epic playerLevel", hit.m_toolTier); // Check level because is boss

                    }
                    else if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value) && !BossDropFlag) //remove mobdrop and is not a boss
                    {
                        __instance.m_nview.GetZDO().Set("epic playerLevel", -hit.m_toolTier); // reg mob check for lvl 
                    }else if (EpicMMOSystem.enabledLevelControl.Value && EpicMMOSystem.removeAllDropsFromNonPlayerKills.Value && attacker.IsTamed())
                    {
                        __instance.m_nview.GetZDO().Set("epic playerLevel", -hit.m_toolTier); // reg mob check for lvl 
                    }
                    else /// No lvl check
                    {

                        if (EpicMMOSystem.extraDebug.Value) 
                            EpicMMOSystem.MLLogger.LogInfo("else ZDO epic playerLevel to 0");

                        if (0 != __instance.m_nview.GetZDO().GetInt("epic playerLevel"))
                        {
                            __instance.m_nview.GetZDO().Set("epic playerLevel", 0); // if not set to 0 then set to 0 - minimize zdo traffic
                            if (EpicMMOSystem.extraDebug.Value) 
                                EpicMMOSystem.MLLogger.LogInfo("Set ZDO epic playerLevel to 0");
                        }  
                    }
                }
                else
                {
                    if (!attacker.IsTamed())
                    {
                        CharacterLastDamageList[__instance] = 100;
                        if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value || EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value || EpicMMOSystem.removeAllDropsFromNonPlayerKills.Value))
                        { 
                            //if (EpicMMOSystem.extraDebug.Value) 
                            // EpicMMOSystem.MLLogger.LogInfo("Player Hit");

                            __instance.m_nview.GetZDO().Set("epic playerLevel", 1000512);// only for removeAllDropsFromNonPlayerKills
                        }
                    }
                }
            }
        }
        
        static void Postfix(Character __instance, long sender, HitData hit)
        {
            if (__instance.IsTamed()) return;
            if (__instance.GetHealth() <= 0f && CharacterLastDamageList.ContainsKey(__instance))
            {
                
                
                
                var pkg = new ZPackage();
                pkg.Write(__instance.gameObject.name);
                pkg.Write(__instance.GetLevel());
                pkg.Write(__instance.transform.position);
                pkg.Write(__instance.IsBoss());
                pkg.Write(__instance.GetMMOLevel());
                
                long attacker = CharacterLastDamageList[__instance];
                
                ZRoutedRpc.instance.InvokeRoutedRPC(attacker, $"{EpicMMOSystem.ModName} DeadMonsters", new object[] { pkg });
                CharacterLastDamageList.Remove(__instance);
            }
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
    public static class ApplyDamage
    {
        public static void Postfix(Character __instance, HitData hit)
        {
            if (__instance.IsTamed()) return;
            if (__instance.GetHealth() <= 0f && CharacterLastDamageList.ContainsKey(__instance))
            {
                var pkg = new ZPackage();
                pkg.Write(__instance.gameObject.name);
                pkg.Write(__instance.GetLevel());
                pkg.Write(__instance.transform.position);
                pkg.Write(__instance.IsBoss());
                pkg.Write(__instance.GetMMOLevel());
                
                long attacker = CharacterLastDamageList[__instance];
                ZRoutedRpc.instance.InvokeRoutedRPC(attacker, $"{EpicMMOSystem.ModName} DeadMonsters", new object[] { pkg });
                CharacterLastDamageList.Remove(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(Character),nameof(Character.OnDestroy))]
    static class Character_OnDestroy_Patch
    {
        static void Postfix(Character __instance)
        {
            if (CharacterLastDamageList.ContainsKey(__instance)) CharacterLastDamageList.Remove(__instance);
        }
    }
}