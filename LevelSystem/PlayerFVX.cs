using System;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

public static class PlayerFVX
{

    public static void levelUp()
    {
        Transform parent = Player.m_localPlayer.transform.Find("Visual/Armature/Hips/Spine/Spine1/Spine2");
        var vfx = EpicMMOSystem.Instantiate(ZNetScene.instance.GetPrefab("LevelUpVFX"), parent).transform;
        vfx.localPosition = Vector3.zero;
        vfx.localScale = new Vector3(0.01352632f, 0.01352632f, 0.01352632f);
    }
}

public class CritDmgVFX
{

    public void CriticalVFX(Vector3 position, float damage)
    {
        /*
        if (EpicMMOSystem.criticalHitVFX.get_Value())
        {
            if (EpicMMOSystem.criticalHitShake.get_Value())
            {
                GameCamera.instance.AddShake(Player.m_localPlayer.transform.position, 10f, EpicMMOSystem.criticalHitShakeIntensity.get_Value(), continous: false);
            }
            Vector3 normalized = (GameCamera.instance.transform.position - position).normalized;
            Vector3 position2 = position + Vector3.up * critTextOffsetY + normalized * critTextOffsetTowardsCam;
            GameObject gameObject = UnityEngine.Object.Instantiate(criticalHitText, position2, Quaternion.identity);
            gameObject.GetComponent<CritTextAnim>().SetText(damage, 1);
            GameObject obj = UnityEngine.Object.Instantiate(criticalHitVFX, position, Quaternion.identity);
            UnityEngine.Object.Destroy(obj, 4f);
        }
        */
    }
}
