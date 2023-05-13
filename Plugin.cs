using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ServerSync;
using UnityEngine;
using UnityEngine.Rendering;
using LocalizationManager;
using System.Collections.Generic;
using fastJSON;
using Groups;
using UnityEngine.UI;
using ItemManager;
using System.Xml;
using EpicMMOSystem.MonoScripts;
using EpicMMOSystem.OtherApi;

namespace EpicMMOSystem;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInDependency("org.bepinex.plugins.groups", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("org.bepinex.plugins.creaturelevelcontrol", BepInDependency.DependencyFlags.SoftDependency)] 

public partial class EpicMMOSystem : BaseUnityPlugin
{
    internal const string ModName = "EpicMMOSystem";
    internal const string ModVersion = "1.6.7";
    internal const string Author = "WackyMole";
    private const string ModGUID = Author + "." + ModName;
    private static string ConfigFileName = ModGUID + ".cfg";
    private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
    public static bool _isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    internal static string ConnectionError = "";
    internal static bool NetisActive = false;

    private readonly Harmony _harmony = new(ModGUID);

    public static readonly ManualLogSource MLLogger =
        BepInEx.Logging.Logger.CreateLogSource(ModName);

    public static readonly ConfigSync ConfigSync = new(ModGUID)
    { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

    public static AssetBundle _asset;
    private string folderpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName);

    internal static EpicMMOSystem Instance;
    public static bool CLLCLoaded = false;

    public static Localization localization;
    //Config
    public static ConfigEntry<string> language;
    public static ConfigEntry<bool> extraDebug;
    //LevelSystem
    public static ConfigEntry<int> maxLevel;
    public static ConfigEntry<int> priceResetPoints;
    public static ConfigEntry<int> freePointForLevel;
    public static ConfigEntry<int> startFreePoint;
    public static ConfigEntry<bool> levelexpforeachlevel;
    public static ConfigEntry<int> levelExp;
    public static ConfigEntry<float> multiNextLevel;
    public static ConfigEntry<float> expForLvlMonster;
    public static ConfigEntry<float> rateExp;
    public static ConfigEntry<float> groupExp;
    public static ConfigEntry<float> groupRange;
    public static ConfigEntry<float> playerRange;
    public static ConfigEntry<bool> lossExp;
    public static ConfigEntry<float> minLossExp;
    public static ConfigEntry<float> maxLossExp;
    public static ConfigEntry<int> maxValueAttribute;
    public static ConfigEntry<string> levelsForBinusFreePoint;
    public static ConfigEntry<bool> tamesGiveXP;
    public static ConfigEntry<bool> leftMessageXP;
    public static ConfigEntry<string> XPstring;

    #region Parameters
    //LevelSystem arg property <Strength>
    public static ConfigEntry<float> physicDamage;
    public static ConfigEntry<float> addWeight;
    public static ConfigEntry<float> staminaRegen;

    //LevelSystem arg property <Agility>
    public static ConfigEntry<float> speedAttack;
    public static ConfigEntry<float> staminaReduction;
    public static ConfigEntry<float> addStamina;
    public static ConfigEntry<float> addEitr;

    //LevelSystem arg property <Intellect>
    public static ConfigEntry<float> magicDamage;
    public static ConfigEntry<float> magicArmor;
    public static ConfigEntry<float> MagicEitrRegen;

    //LevelSystem arg property <Body>
    public static ConfigEntry<float> addHp;
    public static ConfigEntry<float> staminaBlock;
    public static ConfigEntry<float> physicArmor;
    public static ConfigEntry<float> regenHp;
    #endregion


    //Creature level control 2
    public static ConfigEntry<bool> enabledLevelControl;
    public static ConfigEntry<bool> removeDropMax;
    public static ConfigEntry<bool> removeDropMin;
    public static ConfigEntry<bool> removeBossDropMax;
    public static ConfigEntry<bool> removeBossDropMin;
    public static ConfigEntry<bool> mentor;
    public static ConfigEntry<bool> curveExp;
    public static ConfigEntry<bool> curveBossExp;
    public static ConfigEntry<bool> lowDamageLevel;
    public static ConfigEntry<int> lowDamageExtraConfig;
    public static ConfigEntry<int> maxLevelExp;
    public static ConfigEntry<int> minLevelExp;
    public static ConfigEntry<string> MobLVLChars;
    public static ConfigEntry<string> XPColor;
    public static ConfigEntry<bool> removeAllDropsFromNonPlayerKills;
    public static ConfigEntry<bool> noExpPastLVL;

    //Reset attributes items
    public static ConfigEntry<String> prefabNameCoins;
    public static ConfigEntry<String> viewTextCoins;

    //Hud
    public static ConfigEntry<bool> oldExpBar;
    public static ConfigEntry<bool> showMaxHp;
    public static ConfigEntry<float> HudBarScale;
    public static ConfigEntry<float> LevelHudGroupScale;
    public static ConfigEntry<string> HudExpBackgroundCol;
    public static ConfigEntry<string> HudPostionCords;
    public static ConfigEntry<bool> HealthIcons;
    

    // Position Saves
    internal static ConfigEntry<Vector2> HudPanelPosition = null!;
    internal static ConfigEntry<Vector2> ExpPanelPosition = null!;
    internal static ConfigEntry<Vector2> HpPanelPosition = null!;
    internal static ConfigEntry<Vector2> StaminaPanelPosition = null!;
    internal static ConfigEntry<Vector2> EitrPanelPosition = null!;
    internal static ConfigEntry<Vector2> MobLevelPosition = null!;
    internal static ConfigEntry<Vector2> BossLevelPosition = null!;
    internal static ConfigEntry<Vector2> LevelPointPosition = null!;
    internal static ConfigEntry<Vector2> LevelNavPosition = null!;

    // HUD Colors
    public static ConfigEntry<string> HpColor;
    public static ConfigEntry<string> StaminaColor;
    public static ConfigEntry<string> EitrColor;
    public static ConfigEntry<string> ExpColor;

    //HUD Scales
    internal static ConfigEntry<Vector3> ExpScale = null!;
    internal static ConfigEntry<Vector3> HPScale = null!;
    internal static ConfigEntry<Vector3> EitrScale = null!;
    internal static ConfigEntry<Vector3> StaminaScale = null!;

    //Optional
    public static ConfigEntry<float> addDefaultHealth;
    public static ConfigEntry<float> addDefaultWeight;

    internal static Localization english = null!;
    internal static Localization russian = null!;
    internal static Localization spanish = null!;
    public void Awake()
    {
        // Localizer.Load(); - Doesn't seem to be working with yml
        // Localizer.AddText("$attributes", "Attributes WM");
        Instance = this;

        string general = "0.General---------------";
        _serverConfigLocked = config(general, "Force Server Config", true, "Force Server Config");
        language = config(general, "Language", "eng", "Language prefix", false);
        extraDebug = config(general, "EnableExtraDebug", false, "Enable Extra Debug mode for Debugging", false);
        string levelSystem = "1.LevelSystem-----------";
        maxLevel = config(levelSystem, "MaxLevel", 100, "Maximum level. Максимальный уровень");
        priceResetPoints = config(levelSystem, "PriceResetPoints", 3, "Reset price per point. Цена сброса за один поинт");
        freePointForLevel = config(levelSystem, "FreePointForLevel", 5, "Free points per level. Свободных поинтов за один уровень");
        startFreePoint = config(levelSystem, "StartFreePoint", 5, "Additional free points start. Дополнительных свободных поинтов");
        levelExp = config(levelSystem, "FirstLevelExperience", 500, "Amount of experience needed per level. Количество опыта необходимого на 1 уровень");
        levelexpforeachlevel = config(levelSystem, "FirstLevelExperience used on each level", true, "By default the calculations per level are (previous_amount * 1.04 + 500) disabled it will be (previous_amount * 1.04) per level ");
        multiNextLevel = config(levelSystem, "MultiplyNextLevelExperience", 1.04f, "Experience multiplier for the next level - Should never go below 1.00. Умножитель опыта для следующего уровня");
        expForLvlMonster = config(levelSystem, "ExpForLvlMonster", 0.25f, "Extra experience (from the sum of the basic experience) for the level of the monster. Доп опыт (из суммы основного опыта) за уровень монстра");
        rateExp = config(levelSystem, "RateExp", 1f, "Experience multiplier. Множитель опыта");
        groupExp = config(levelSystem, "GroupExp", 0.70f, "Experience multiplier that the other players in the group get. Множитель опыта который получают остальные игроки в группе");
        minLossExp = config(levelSystem, "MinLossExp", 0.05f, "Minimum Loss Exp if player death, default 5% loss");
        maxLossExp = config(levelSystem, "MaxLossExp", 0.25f, "Maximum Loss Exp if player death, default 25% loss");
        lossExp = config(levelSystem, "LossExp", true, "Enabled exp loss");
        maxValueAttribute = config(levelSystem, "MaxValueAttribute", 200, "Maximum number of points you can put into one attribute");
        levelsForBinusFreePoint = config(levelSystem, "BonusLevelPoints", "5:5,10:5", "Added bonus point for level. Example(level:points): 5:10,15:20 add all 30 points ");
        groupRange = config(levelSystem, "Group EXP Range", 70f, "The range at which people in a group (Group MOD ONLY) get XP, relative to player who killed mob - only works if the killer gets xp. - Default 70f, a large number like 999999999999f, will probably cover map");
        playerRange = config(levelSystem, "Player EXP Range", 70f, "The range at which a player gets XP");
        tamesGiveXP = config(levelSystem, "Tames give XP on Mob kill", true, "Your tames give players in range XP");
        leftMessageXP = config(levelSystem, "Display XP Received on Left", true, "Display XP Amount on Left Message when mob is killed");
        XPColor = config(levelSystem, "XP death Color", "#fff708", "The Color of XP popup market on a mob death");
        XPstring = config(levelSystem, "XP String", "+@ XP", "@ for XP Received, must include '@'");


        #region ParameterCofig
        string levelSystemStrngth = "1.LevelSystem Strength--";
        physicDamage = config(levelSystemStrngth, "PhysicDamage", 0.20f, "Damage multiplier per point. Умножитель урона за один поинт");
        addWeight = config(levelSystemStrngth, "AddWeight", 2f, "Adds carry weight per point. Добавляет переносимый вес за один поинт");
        staminaRegen = config(levelSystemStrngth, "StaminaReg", 0.4f, "Increase stamina regeneration per point.");

        string levelSystemAgility = "1.LevelSystem Agility---";
        speedAttack = config(levelSystemAgility, "StaminaAttack", 0.1f, "Reduces attack stamina consumption. Уменьшает потребление стамины на атаку");
        staminaReduction = config(levelSystemAgility, "StaminaReduction", 0.15f, "Decrease stamina consumption for running, jumping for one point. Уменьшение расхода выносливости на бег, прыжок за один поинт");
        addStamina = config(levelSystemAgility, "AddStamina", 1f, "One Point Stamina Increase. Увеличение  выносливости за один поинт");
        

        string levelSystemIntellect = "1.LevelSystem Intellect-";
        magicDamage = config(levelSystemIntellect, "MagicAttack", 0.20f, "Increase magic attack per point. Увеличение магической атаки за один поинт");
        magicArmor = config(levelSystemIntellect, "MagicArmor", 0.1f, "Increase magical protection per point. Увеличение магической защиты за один поинт");
        MagicEitrRegen = config(levelSystemIntellect, "MagicEitrReg", 0.3f, "Increase magical Eitr Regeneration per point. Увеличивает регенерацию магического Эйтра на единицу.");
        addEitr = config(levelSystemIntellect, "AddEitr", 0.3f, "Eitr Increase per point ONLY when player has above 1 base Eitr");

        string levelSystemBody = "1.LevelSystem Body------";
        addHp = config(levelSystemBody, "AddHp", 1f, "One Point Health Increase. Увеличение здоровья за один поинт");
        staminaBlock = config(levelSystemBody, "StaminaBlock", 0.2f, "Decrease stamina consumption per unit per point. Уменьшение расхода выносливости на блок за один поинт");
        physicArmor = config(levelSystemBody, "PhysicArmor", 0.15f, "Increase in physical protection per point. Увеличение физической защиты за один поинт");
        regenHp = config(levelSystemBody, "RegenHp", 0.1f, "Increase health regeneration per point. Увеличение регенерации здоровья за один поинт");
        #endregion

        string creatureLevelControl = "2.Creature level control";
        mentor = config(creatureLevelControl, "Mentor", true, "Give full eXP for low level members in Groups");
        enabledLevelControl = config(creatureLevelControl, "Enabled_creature_level", true, "Enable creature Level control - Disable this to remove levels, but still get eXP gain");
        removeDropMax = config(creatureLevelControl, "Remove_creature_drop_max", false, "Monsters after death do not give items if their level is higher than player level + MaxLevel");
        removeDropMin = config(creatureLevelControl, "Remove_creature_drop_min", false, "Monsters after death do not give items if their level is lower than player level - MinLevel");
        removeBossDropMax = config(creatureLevelControl, "Remove_boss_drop_max", false, "Bosses after death do not give items if their level is higher than player level + MaxLevel");
        removeBossDropMin = config(creatureLevelControl, "Remove_boss_drop_min", false, "Bosses after death do not give items if their level is lower than player level - Minlevel");
        curveExp = config(creatureLevelControl, "Curve_creature_exp", true, "Monsters after death will give less exp if player is outside Max or Min Level Range");
        curveBossExp = config(creatureLevelControl, "Curve_Boss_exp", true, "Bosses after death will give less exp if player is outside Max or Min Level Range");
        lowDamageLevel = config(creatureLevelControl, "Low_damage_level", false, "Decreased damage to the monster if the level is insufficient");
        lowDamageExtraConfig = config(creatureLevelControl, "Low_damage_config", 0, "Extra paramater to low damage config - for reference(float)(playerLevel + lowDamageConfig) / monsterLevel; when player is below lvl");
        minLevelExp = config(creatureLevelControl, "MinLevelRange", 10, "Character level - MinLevelRange is less than the level of the monster, then you will receive reduced experience. Уровень персонажа - MinLevelRange меньше уровня монстра, то вы будете получать урезанный опыт");
        maxLevelExp = config(creatureLevelControl, "MaxLevelRange", 10, "Character level + MaxLevelRange is less than the level of the monster, then you will not receive experience. Уровень персонажа + MaxLevelRange меньше уровня монстра, то вы не будете получать опыт");
        MobLevelPosition = config(creatureLevelControl, "LevelBar Position", new Vector2(40, -30), "LevelBar Position for regular mobs - synced");
        BossLevelPosition = config(creatureLevelControl, "Boss LevelBar Position", new Vector2(0, 30), "LevelBar Position for Boss Bars - synced");
        MobLVLChars = config(creatureLevelControl, "Mob Level UI String", "[@]", "[@] uses @ for moblevel, must include '@', but you coudl do 'Level @' or something similar");
        removeAllDropsFromNonPlayerKills = config(creatureLevelControl, "RemoveAllDrops From NonPlayer Kills", true, "Remove all drops from mobs that were not killed by a player or tame -ie no drops from mobs attacking each other");
        noExpPastLVL = config(creatureLevelControl, "U Jerk, NoExpOn Red/Blue", false, "You are a jerk admin if you enable this, this removes all exp on creatures that are above level (RED) or Below (Blue)");

        string resetAttributesItems = "3.Reset attributes items";
        prefabNameCoins = config(resetAttributesItems, "prefabName", "Coins", "Name prefab item");
        viewTextCoins = config(resetAttributesItems, "viewText", "coins or 1 Reset Trophy", "Name item");

        string hud = "4.Hud--------------------";
        oldExpBar = config(hud, "OldXPBar", false, "Use the old eXP Bar only (need restart, not server sync) Does not move or scale - decrepitated UI", false);
        showMaxHp = config(hud, "ShowMaxHp", true, "Show max hp (100 / 100)", false);

        HudPanelPosition = config(hud, "1.0HudGrouplPosition", new Vector2(0, 0), "Position of the Main EpicHudBarBackground in (x,y) - You can drag individual components below", false);
        HudExpBackgroundCol = config(hud, "1.1BackgroundCol", "#2F1600", "Background color in Hex, set to 'none' to make transparent", false);
        HudBarScale = config(hud, "1.2HudGroupScale", .90f, "Scale for HudGroup - exp, background, hp, stamina, eitr - You can do individual below", false);
        ExpPanelPosition = config(hud, "2.0ExpPanelPosition", new Vector2(0, 0), "Position of the Exp panel (x,y)", false);
        ExpColor = config(hud, "2.1ExpColor", "#FFFFFF", "Exp fill color in Hex - White bleeds through with purple, set to 'none' to have no xp bar", false);
        ExpScale = config(hud, "2.2ExpScale", new Vector3(1, 1, 1), "Exp Bar Scale factor", false);
        StaminaPanelPosition = config(hud, "3.0StaminaPanelPosition", new Vector2(0, 0), "Position of the Stamina panel (x,y)", false);
        StaminaColor = config(hud, "3.1StaminaColor", "#986100", "Stamina color in Hex, set to 'none' to make vanilla", false);
        StaminaScale = config(hud, "3.2StaminaScale", new Vector3(1, 1, 1), "Stamina Bar Scale factor", false);
        HpPanelPosition = config(hud, "4.0HpPanelPosition", new Vector2(0, 0), "Position of the Hp panel (x,y)", false);
        HpColor = config(hud, "4.1HPColor", "#870000", "HP color in Hex, set to 'none' to make vanilla", false);
        HPScale = config(hud, "4.2HPScale", new Vector3(1, 1, 1), "HP Bar Scale factor", false);
        EitrPanelPosition = config(hud, "5.0EitrPanelPosition", new Vector2(0, 0), "Position of the Eitr panel (x,y)", false);
        EitrColor = config(hud, "5.1EitrColor", "#84257C", "Eitr color in Hex, set to 'none' to make vanilla", false);
        EitrScale = config(hud, "5.2EitrScale", new Vector3(1, 1, 1), "Eitr Bar Scale factor", false);
        LevelHudGroupScale = config(hud, "6.2LevelHudGroupScale", 1.0f, "LevelHud Group of Objects Scale factor (Nav Bar, Point Hub)", false);
        LevelPointPosition = config(hud, "7.0PointHudPosition", new Vector2(0, 0), "Position of the Point panel (x,y)", false);
        LevelNavPosition = config(hud, "8.0NavBarPosition", new Vector2(0, 100), "Position of the NavBar (x,y)", false);


        // HealthIcons = config(hud, "DisabledDefaultHealth", true, "Default is true, not synced", false);


        string optionalEffect = "5.Optional perk---------";
        addDefaultHealth = config(optionalEffect, "AddDefaultHealth", 0f, "Add health by default");
        addDefaultWeight = config(optionalEffect, "AddDefaultWeight", 0f, "Add weight by default");
        
        _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

        Assembly assembly = Assembly.GetExecutingAssembly();
        _harmony.PatchAll(assembly);
        
        _asset = GetAssetBundle("epicasset");
        itemassets();
        localization = new Localization();
        MyUI.Init();

        if (Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.creaturelevelcontrol")){
            CLLCLoaded = true;
        }
    }

    private static void itemassets()
    {
        Item OrbTest = new("orbs", "magicorb2", "asset");
        OrbTest.Name.English("orbtest");
        OrbTest.Description.English("XP");
        OrbTest.ToggleConfigurationVisibility(Configurability.Disabled);
        OrbTest.Snapshot();
    


        Item ResetTrophy = new("epicmmoitems", "ResetTrophy", "asset");
        ResetTrophy.Name.English("ResetTrophy");
        ResetTrophy.Description.English("A Trophy you can use to reset MMO points. Rare");
        ResetTrophy.ToggleConfigurationVisibility(Configurability.Drop);
        ResetTrophy.Snapshot();

        
    }
    private void Start()
    {
        DataMonsters.Init();
        FriendsSystem.Init();
        SetupWatcher();
        
    }

    private void OnDestroy()
    {
        Config.Save();
    }

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZRouteAwake
    {
        private static void Postfix()
        {
            NetisActive = true;
        }
    }


    private void SetupWatcher()
    { 
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            FileSystemWatcher watcher = new(folderpath); // jsons in config
            watcher.Changed += ReadJsonValues;
            watcher.Created += ReadJsonValues;
            watcher.Renamed += ReadJsonValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;

            FileSystemWatcher watcher2 = new(Paths.ConfigPath, ConfigFileName);
            watcher2.Changed += ReadConfigValues;
            watcher2.IncludeSubdirectories = false;
            watcher2.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher2.EnableRaisingEvents = true;

    }

    private void ReadJsonValues(object sender, FileSystemEventArgs e)
    {
        if (NetisActive)
        {
            if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated()) // only dedicated
            {
                List<string> list = new List<string>();
                foreach (string file in Directory.GetFiles(folderpath, "*.json", SearchOption.AllDirectories))
                {
                    var nam = Path.GetFileName(file);
                    if (EpicMMOSystem.extraDebug.Value)
                        EpicMMOSystem.MLLogger.LogInfo(nam + " read");

                    var temp = File.ReadAllText(file);
                    list.Add(temp);

                }
                if (EpicMMOSystem.extraDebug.Value)
                    EpicMMOSystem.MLLogger.LogInfo($"Mobs Updated on Server");

                DataMonsters.MonsterDBL = list;
                List<ZNetPeer> peers = ZNet.instance.GetPeers();
                foreach (var peer in peers)
                {
                    if (peer == null) return;
                    ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{EpicMMOSystem.ModName} SetMonsterDB", list); //sync list
                }
            }
        }
    }




        internal void ReadConfigValues(object sender = null, FileSystemEventArgs e = null)
    {
        if (!File.Exists(ConfigFileFullPath)) return;

        var updatelvl = LevelSystem.Instance;
        updatelvl.FillLevelsExp(); // updates when people change levelExp,multiNextLeve, maxlevel

        Color tempC;
        MyUI.expPanelRoot.GetComponent<CanvasScaler>().scaleFactor = HudBarScale.Value;
        MyUI.levelSystemPanelRoot.GetComponent<CanvasScaler>().scaleFactor = EpicMMOSystem.LevelHudGroupScale.Value;

        if (EpicMMOSystem.HudExpBackgroundCol.Value == "none")
            MyUI.expPanelBackground.SetActive(false);
        else
        {
            MyUI.expPanelBackground.SetActive(true);
            if (ColorUtility.TryParseHtmlString(HudExpBackgroundCol.Value, out tempC))
                MyUI.expPanelBackground.GetComponent<Image>().color = tempC;
        }
        MyUI.DisableHPBar =  (HpColor.Value == "none");
        MyUI.DisableStaminaBar = (StaminaColor.Value == "none");
        MyUI.DisableEitrBar = (EitrColor.Value == "none");
        MyUI.DisableExpBar = (ExpColor.Value == "none");


        MyUI.hp.gameObject.SetActive(!MyUI.DisableHPBar);
        MyUI.DHpBar.SetActive(MyUI.DisableHPBar);
        MyUI.IconHpBar.SetActive(MyUI.DisableHPBar);
        MyUI.Exp.gameObject.SetActive(!MyUI.DisableExpBar);
        MyUI.stamina.gameObject.SetActive(!MyUI.DisableStaminaBar);


        if (ColorUtility.TryParseHtmlString(HpColor.Value, out tempC ))
            MyUI.hpImage.color = tempC;
        if (ColorUtility.TryParseHtmlString(StaminaColor.Value, out tempC))
            MyUI.staminaImage.color = tempC;
        if (ColorUtility.TryParseHtmlString(EitrColor.Value, out tempC))
            MyUI.EitrImage.color = tempC;
        if (ColorUtility.TryParseHtmlString(ExpColor.Value, out tempC))
        {
            if (ExpColor.Value == "#FFFFFF")
                MyUI.eBarImage.color = tempC;
            else
                MyUI.eBarImage.color = tempC * 2;
        }

        //MyUI.hpImage.color = ColorUtil.GetColorFromHex(HpColor.Value); // maybe  destoryed and color is not beingupdate on static element?
        //MyUI.staminaImage.color = ColorUtil.GetColorFromHex(StaminaColor.Value);
        //MyUI.EitrImage.color = ColorUtil.GetColorFromHex(EitrColor.Value);

        MyUI.Exp.GetComponent<RectTransform>().localScale = ExpScale.Value;
        MyUI.hp.GetComponent<RectTransform>().localScale = HPScale.Value;
        MyUI.stamina.GetComponent<RectTransform>().localScale = StaminaScale.Value;
        MyUI.EitrTran.GetComponent<RectTransform>().localScale = EitrScale.Value;

        try
        {
            if (EpicMMOSystem.extraDebug.Value)
                MLLogger.LogInfo("MMO Manual ReadConfigValues called");
            Config.Reload();
            DragControl.RestoreWindow(MyUI.expPanel.gameObject);
            DragControl.RestoreWindow(MyUI.hp.gameObject);
            DragControl.RestoreWindow(MyUI.Exp.gameObject);
            DragControl.RestoreWindow(MyUI.stamina.gameObject);
            DragControl.RestoreWindow(MyUI.EitrGameObj);
            DragWindowCntrl.RestoreWindow(MyUI.navigationPanel, false);
            DragWindowCntrl.RestoreWindow(MyUI.levelSystemPanel, false);
        }
        catch
        {
            MLLogger.LogError($"There was an issue loading your {ConfigFileName}");
            MLLogger.LogError("Please check your config entries for spelling and format!");
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse3))
    //     {
    //         // LevelSystem.Instance.ResetAllParameter();
    //         Player.m_localPlayer.m_timeSinceDeath = 3000f;
    //     }
    // }


    #region ConfigOptions

    private static ConfigEntry<bool>? _serverConfigLocked;

    private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
        bool synchronizedSetting = true)
    {
        ConfigDescription extendedDescription =
            new(
                description.Description +
                (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                description.AcceptableValues, description.Tags);
        ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
        //var configEntry = Config.Bind(group, name, value, description);

        SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description,
        bool synchronizedSetting = true)
    {
        return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
    }

    private class ConfigurationManagerAttributes
    {
        public bool? Browsable = false;
    }

    #endregion
        
    public static AssetBundle GetAssetBundle(string filename)
    {
        var execAssembly = Assembly.GetExecutingAssembly();

        string resourceName = execAssembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(filename));

        using (var stream = execAssembly.GetManifestResourceStream(resourceName))
        {
            return AssetBundle.LoadFromStream(stream);
        }
    }


}