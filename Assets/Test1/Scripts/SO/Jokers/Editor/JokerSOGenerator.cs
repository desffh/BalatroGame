using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class JokerSOGenerator
{
    public static void CreateSOFromData(JokerData data)
    {
        JokerDataSO asset = ScriptableObject.CreateInstance<JokerDataSO>();

        asset.jokerName = data.name;
        asset.cost = data.cost;
        asset.multiple = data.multiple;
        asset.require = data.require;

        string folderPath = "Assets/Test1/Scripts/SO/Jokers/JokersSO";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string assetPath = $"{folderPath}/{data.name}_JokerData.asset";
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"ScriptableObject 생성 완료: {assetPath}");
    }
}
#endif
