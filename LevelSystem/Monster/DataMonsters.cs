//using UnityEngine.UI;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using ItemManager;
using UnityEngine;
using Random = UnityEngine.Random;
//using Text = UnityEngine.UI.Text;

namespace EpicMMOSystem.LevelSystem.Monster;


public static partial class DataMonsters
{
    private static Dictionary<string, global::EpicMMOSystem.Monster> dictionary = new();
    private static string MonsterDB = "";
    public static List<string> MonsterDBL;
    private static readonly Dictionary<Heightmap.Biome, GameObject> OrbsByBiomes = new(10);
    public static readonly Dictionary<GameObject, int> MagicOrbDictionary = new Dictionary<GameObject, int>(10);// thx KG


    public static void InitItems()
    {// Visit  https://github.com/Wacky-Mole/MagicHeim/blob/master/MagicTomes.cs for more details

        Item Orb1 = new("mmo_xp", "mmo_orb1", "asset");
        Orb1.ToggleConfigurationVisibility(Configurability.Disabled);
        Item Orb2 = new("mmo_xp", "mmo_orb2", "asset");
        Orb2.ToggleConfigurationVisibility(Configurability.Disabled);
        Item Orb3 = new("mmo_xp", "mmo_orb3", "asset");
        Orb3.ToggleConfigurationVisibility(Configurability.Disabled);
        Item Orb4 = new("mmo_xp", "mmo_orb4", "asset");
        Orb4.ToggleConfigurationVisibility(Configurability.Disabled);
        Item Orb5 = new("mmo_xp", "mmo_orb5", "asset");
        Orb5.ToggleConfigurationVisibility(Configurability.Disabled);
        Item Orb6 = new("mmo_xp", "mmo_orb6", "asset");
        Orb6.ToggleConfigurationVisibility(Configurability.Disabled);

        MagicOrbDictionary.Add(Orb1.Prefab, EpicMMOSystem.XPforOrb1.Value);
        MagicOrbDictionary.Add(Orb2.Prefab, EpicMMOSystem.XPforOrb2.Value);
        MagicOrbDictionary.Add(Orb3.Prefab, EpicMMOSystem.XPforOrb3.Value);
        MagicOrbDictionary.Add(Orb4.Prefab, EpicMMOSystem.XPforOrb4.Value);
        MagicOrbDictionary.Add(Orb5.Prefab, EpicMMOSystem.XPforOrb5.Value);
        MagicOrbDictionary.Add(Orb6.Prefab, EpicMMOSystem.XPforOrb6.Value);
        


        OrbsByBiomes.Add(Heightmap.Biome.Meadows, Orb1.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.BlackForest, Orb2.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.Swamp, Orb3.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.Mountain, Orb4.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.Plains, Orb5.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.DeepNorth, Orb6.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.AshLands, Orb6.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.Ocean, Orb3.Prefab);
        OrbsByBiomes.Add(Heightmap.Biome.None, Orb2.Prefab);
    }


    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetTooltip), typeof(ItemDrop.ItemData),typeof(int),typeof(bool), typeof(float) )]
    public class GetTooltipPatch
    {
        public static void Postfix(ItemDrop.ItemData item, bool crafting, ref string __result)
        {
            if (crafting || item == null || !item.m_dropPrefab) return;
            if (MagicOrbDictionary.TryGetValue(item.m_dropPrefab, out var expGain))
            {
                __result = __result + $"Right Mouse Button click to get <color=yellow>{expGain}</color> EXP";
            }
        }
    }

    public static bool contains(string name)
    {
        return dictionary.ContainsKey(name);
    }

    public static int getExp(string name)
    {
        var monster = dictionary[name];
        int exp = Random.Range(monster.minExp, monster.maxExp);
        return exp;
    }

    public static int getMaxExp(string name)
    {
        return dictionary[name].maxExp;
    }

    public static int getLevel(string name)
    {
        return dictionary[name].level;
    }

    private static void createNewDataMonsters(List<string> json)
    {
        dictionary.Clear();

            foreach (var monster2 in json)
            {
            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo($"/n Json loading /n");

                //var temp = JsonUtility.FromJson<Monster[]>(monster2);
                var temp = (fastJSON.JSON.ToObject<global::EpicMMOSystem.Monster[]>(monster2));
                foreach (var monster in temp)
                {
                    if (EpicMMOSystem.extraDebug.Value)
                        EpicMMOSystem.MLLogger.LogInfo($"{monster.name}");

                    dictionary.Add($"{monster.name}(Clone)", monster);
                }
            
        }
     
    }

    public static void Init()
    {
        var versionpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName, $"Version.txt");
        var folderpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName);
        var warningtext = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName, $"If you want to stop from updating.txt");
        var json = "MonsterDB_Default.json";
        var json1 = "MonsterDB_AirAnimals.json";
        var json2 = "MonsterDB_LandAnimals.json";
        var json3 = "MonsterDB-Fantasy-Creatures.json";
        var json4 = "MonsterDB_SeaAnimals.json";
        var json5 = "MonsterDB_MonsterLabZ.json";
        var json6 = "MonsterDB_Outsiders.json";
        var json7 = "MonsterDB_DoorDieMonsters.json";
        var json8 = "MonsterDB_MajesticChickens.json";
        var json9 = "MonsterDB-Monstrum.json";
        var json10 = "MonsterDB-Reforge_Krumpac.json";
        var json11 = "MonsterDB_TeddyBears.json";
        var json12 = "MonsterDB-PungusSouls.json";
        var json13 = "MonsterDB_Jewelcrafting.json";
        var json14 = "MonsterDB_RtDMonsters.json";

        if (!Directory.Exists(folderpath)){
            Directory.CreateDirectory(folderpath);
        }
        var cleartowrite = true;
        if (File.Exists(versionpath))
        {
            //MonsterDB = File.ReadAllText(path);
            var filev = File.ReadAllText(versionpath);
            cleartowrite = false; // default is false because it exists in the first place

            if (filev == "1.4.0")
                cleartowrite = true;
            if (filev == "1.4.1")
                cleartowrite = true;
            if (filev == "1.5.0")
                cleartowrite = true;
            if (filev == "1.5.3")
                cleartowrite = true;
            if (filev == "1.5.4")
                cleartowrite = true;
            if (filev == "1.5.8")
                cleartowrite = true;
            if (filev == "1.6.2")
                cleartowrite = true;
            if (filev == "1.6.3")
                cleartowrite = true;            
            if (filev == "1.6.5")
                cleartowrite = true;
            if (filev == "1.6.7")
                cleartowrite = true;
            if (filev == "1.7.0")
                cleartowrite = true;            
            if (filev == "1.7.3")
                cleartowrite = true;            
            if (filev == "1.7.4")
                cleartowrite = true;            
            if (filev == "1.7.5")
                cleartowrite = true;            
            if (filev == "1.7.6")
                cleartowrite = true;            
            if (filev == "1.7.7")
                cleartowrite = true;
            if (filev == "1.7.8")
                cleartowrite = true;            
            if (filev == "1.7.9")
                cleartowrite = true;
            if (filev == "1.8.7")
                cleartowrite = true;



            if (filev == "1.8.8") // last version to get a DB update
                cleartowrite = false;

            if (filev == "NO" || filev == "no" || filev == "No" || filev == "STOP" || filev == "stop" || filev == "Stop")
            {// don't update
                cleartowrite = false;
            }

        }
        if (cleartowrite)
        {
            //list.Clear();
            File.WriteAllText(versionpath, "1.8.8"); // Write Version file, don't auto update

            File.WriteAllText(warningtext, "Erase numbers in Version.txt and write NO or stop in file. This should stop DB json files from updating on an update. If you make your own custom json file, then that one should never be updated.");

            File.WriteAllText(Path.Combine(folderpath, json), getDefaultJsonMonster(json));

            File.WriteAllText(Path.Combine(folderpath, json1), getDefaultJsonMonster(json1));

            File.WriteAllText(Path.Combine(folderpath, json2), getDefaultJsonMonster(json2));

            File.WriteAllText(Path.Combine(folderpath, json3), getDefaultJsonMonster(json3));

            File.WriteAllText(Path.Combine(folderpath, json4), getDefaultJsonMonster(json4));

            File.WriteAllText(Path.Combine(folderpath, json5), getDefaultJsonMonster(json5));

            File.WriteAllText(Path.Combine(folderpath, json6), getDefaultJsonMonster(json6));

            File.WriteAllText(Path.Combine(folderpath, json7), getDefaultJsonMonster(json7));

            File.WriteAllText(Path.Combine(folderpath, json8), getDefaultJsonMonster(json8));

            File.WriteAllText(Path.Combine(folderpath, json9), getDefaultJsonMonster(json9));

            File.WriteAllText(Path.Combine(folderpath, json10), getDefaultJsonMonster(json10));
            
            File.WriteAllText(Path.Combine(folderpath, json11), getDefaultJsonMonster(json11));

            File.WriteAllText(Path.Combine(folderpath, json12), getDefaultJsonMonster(json12));

            File.WriteAllText(Path.Combine(folderpath, json13), getDefaultJsonMonster(json13));

            File.WriteAllText(Path.Combine(folderpath, json14), getDefaultJsonMonster(json14));


            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo($"Mobs Jsons Written");
        }
        List<string> list = new List<string>();
        foreach (string file in Directory.GetFiles(folderpath, "*.json", SearchOption.AllDirectories))
        { 
            var nam = Path.GetFileName(file);
            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo(nam + " read");

            var temp = File.ReadAllText(file);
            list.Add(temp);
            MonsterDB += temp;
           
        }
        if (EpicMMOSystem.extraDebug.Value)
            EpicMMOSystem.MLLogger.LogInfo($"Mobs Read");


        MonsterDBL = list;
        createNewDataMonsters(list);
        
    }

    private static string getDefaultJsonMonster(string jsonname)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(jsonname));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZrouteMethodsServerFeedback
    {
        private static void Postfix()
        {
            if (EpicMMOSystem._isServer) return;
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} SetMonsterDB",
                new Action<long, List<string>>(SetMonsterDB));
        }
    }

    public static void SetMonsterDB(long peer, List<string> json)
    {
        createNewDataMonsters(json);
    }

    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    private static class ZnetSyncServerInfo
    {
        private static void Postfix(ZRpc rpc)
        {
            if (!EpicMMOSystem._isServer) return; // doesn't work on Coop
            //if (!(ZNet.instance.IsServer() && ZNet.instance.IsDedicated())) return;
            ZNetPeer peer = ZNet.instance.GetPeer(rpc);
            if(peer == null) return;
            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{EpicMMOSystem.ModName} SetMonsterDB", MonsterDBL); //sync list
        }
    }

    // [HarmonyPatch(typeof(Character), nameof(Character.GetHoverName))]
    // [HarmonyPriority(Priority.First)]
    // public static class MonsterColorText
    // {
    //     public static void Postfix(Character __instance, ref string __result)
    //     {
    //         if (!contains(__instance.gameObject.name)) return;
    //         int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
    //         int minLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.minLevelExp.Value;
    //         int monsterLevel = getLevel(__instance.gameObject.name);
    //         if (monsterLevel > maxLevelExp)
    //         {
    //             __result = $"<color=red>{__result} [{monsterLevel}]</color>";
    //         } else if (monsterLevel < minLevelExp)
    //         {
    //             __result = $"<color=#2FFFDC>{__result} [{monsterLevel}]</color>";
    //         }
    //         else
    //         {
    //             __result = $"{__result} [{monsterLevel}]";
    //         }
    //     }
    // }
}