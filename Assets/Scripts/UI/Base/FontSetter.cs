using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FontSetter
{
    public const string path_BMJUA_Font = "Fonts/BMJUA_TTF SDF";

    [MenuItem("Custom/Set All Fonts to BMJUA(현재 씬 내 모든 폰트를 BMJUA 폰트로 교체)")]
    public static void SetAllFontsToBMJUA()
    {
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var gameObject in rootGameObjects)
        {
            TMP_Text[] allTMPTextComponents = gameObject.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text tmpTextComponent in allTMPTextComponents)
            {
                tmpTextComponent.font = Resources.Load<TMP_FontAsset>(path_BMJUA_Font);
                EditorUtility.SetDirty(tmpTextComponent); // 변경 사항을 저장
            }
        }
    }
}
