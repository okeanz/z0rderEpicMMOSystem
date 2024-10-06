using System.Collections.Generic;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem.LevelSystem.Monster;

[HarmonyPatch(typeof(EnemyHud), nameof(EnemyHud.ShowHud))]
[HarmonyPriority(1)] //almost last
public static class MonsterColorTexts
{
    public static void Postfix(EnemyHud __instance, Character c, Dictionary<Character, EnemyHud.HudData> ___m_huds, bool __state)
    {
        try { if (c.m_tamed) return; } catch { } // might remove this in future so tames can give xp ect
        if (!EpicMMOSystem.enabledLevelControl.Value) return;
        if (!DataMonsters.contains(c.gameObject.name)) return;
        if (___m_huds[c].m_gui.transform.Find("Name/Name(Clone)")) return;
            
            
        int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
        int minLevelExp = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;

        var monsterLevel = c.GetMMOLevel();
        GameObject component = ___m_huds[c].m_gui.transform.Find("Name").gameObject;
        //var textspace = component.GetComponent<Text>().text;
        //component.GetComponent<Text>().text = " "+ textspace + " "; // add some spacing for single letter names
        GameObject levelName = Object.Instantiate(component, component.transform);
        levelName.GetComponent<RectTransform>().anchoredPosition = EpicMMOSystem.MobLevelPosition.Value;
        if (c.m_boss)
        {
            levelName.GetComponent<RectTransform>().anchoredPosition = EpicMMOSystem.BossLevelPosition.Value;
        }
        string stringtolvl = EpicMMOSystem.MobLVLChars.Value;
        string moblvlstring = monsterLevel.ToString();
        Color color = monsterLevel > maxLevelExp ? Color.red : Color.white;
            
        if (monsterLevel < minLevelExp) color = Color.cyan;
        if (monsterLevel == 0)
        {
            moblvlstring = "???";
            color = Color.yellow;
        }
        stringtolvl = stringtolvl.Replace("@", moblvlstring); // not sure how fast this is
        // levelName.GetComponent<TextMeshProUGUI>().horizontalOverflow = UnityEngine.HorizontalWrapMode.Overflow; 
        levelName.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Overflow;
        levelName.AddComponent<ContentSizeFitter>().SetLayoutHorizontal();
        levelName.GetComponent<TextMeshProUGUI>().text = stringtolvl;
        component.GetComponent<TextMeshProUGUI>().color = color;
        levelName.GetComponent<TextMeshProUGUI>().color = color;
        if (___m_huds[c].m_gui.transform.Find("extraeffecttext"))
        {
            ___m_huds[c].m_gui.transform.Find("extraeffecttext").TryGetComponent<TextMeshProUGUI>(out var hi);
            if (hi != null)
            {
                hi.color = color;
            }
        }

    }
        
    [HarmonyPatch(typeof(EnemyHud), nameof(EnemyHud.UpdateHuds))]
    public static class StarVisibilityMMO
    {
        private static void Postfix(Dictionary<Character, EnemyHud.HudData> ___m_huds)
        {
            if (___m_huds == null) return;
            //if (EpicMMOSystem.CLLCLoaded) return;
            if (!EpicMMOSystem.enabledLevelControl.Value) return;
            foreach (KeyValuePair<Character, EnemyHud.HudData> keyValuePair in ___m_huds)
            {
                var character = keyValuePair.Key;
                var hudData = keyValuePair.Value;
                    
                if (character.IsTamed()) return;
                if (character != null && hudData.m_gui)
                {
                    if (!DataMonsters.contains(character.gameObject.name)) return;
                    int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
                    int minLevelExp = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
                        
                    int monsterLevel = character.GetMMOLevel();
                    string mobLevelString = monsterLevel.ToString();
                    Color color = monsterLevel > maxLevelExp ? Color.red : Color.white;
                    if (monsterLevel < minLevelExp) color = Color.cyan;
                    if (character.GetMMOLevel() == 0)
                    {
                        mobLevelString = "???";
                        color = Color.yellow;
                    }
                    Transform transform = hudData.m_gui.transform.Find("Name/Name(Clone)");
                    if (transform != null)
                    {
                        transform.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject component = hudData.m_gui.transform.Find("Name").gameObject;
                        transform = Object.Instantiate(component, component.transform).transform;
                        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(37, -30);
                        transform.GetComponent<TextMeshProUGUI>().fontSize = 13;
                        transform.GetComponent<TextMeshProUGUI>().text = $"[{mobLevelString}]";
                    }
                    transform.GetComponent<TextMeshProUGUI>().color = color;
                    hudData.m_gui.transform.Find("Name").GetComponent<TextMeshProUGUI>().color = color;
                    if (hudData.m_gui.transform.Find("extraeffecttext")) // for cllc extra components
                    {
                        hudData.m_gui.transform.Find("extraeffecttext").TryGetComponent<TextMeshProUGUI>(out var hi);
                        if (hi != null) // null check if not set
                        {
                            hi.color = color;
                        }
                    }
                }
            }
        }
    }
}