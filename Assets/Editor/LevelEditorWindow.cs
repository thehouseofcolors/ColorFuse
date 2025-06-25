#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class LevelPrefabGeneratorEditor : EditorWindow
{
    public List<LevelConfig> levelConfigs = new List<LevelConfig>();
    private string savePath = "Assets/GeneratedLevels";
    private GameObject prefabTemplate;

    [MenuItem("Tools/Level Prefab Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelPrefabGeneratorEditor>("Level Prefab Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Prefab Generator", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty listProp = so.FindProperty("levelConfigs");
        EditorGUILayout.PropertyField(listProp, new GUIContent("Level Configs"), true);
        so.ApplyModifiedProperties();

        savePath = EditorGUILayout.TextField("Save Path", savePath);
        prefabTemplate = (GameObject)EditorGUILayout.ObjectField("Optional Template Prefab", prefabTemplate, typeof(GameObject), false);

        if (GUILayout.Button("Generate Prefabs"))
        {
            GeneratePrefabs();
        }
    }

    private void GeneratePrefabs()
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        foreach (var config in levelConfigs)
        {
            // Seed oluştur
            config.seed = SeedGenerator.GenerateSeed(config.level);

            // Prefab GameObject oluştur
            string prefabName = $"Level_{config.level}";
            GameObject go = prefabTemplate != null ? Instantiate(prefabTemplate) : new GameObject(prefabName);
            go.name = prefabName;

            // ConfigHolder ekle
            var holder = go.GetComponent<LevelConfigHolder>();
            if (holder == null)
                holder = go.AddComponent<LevelConfigHolder>();

            holder.config = config;

            // Prefab kaydet
            string prefabPath = Path.Combine(savePath, $"{prefabName}.prefab").Replace("\\", "/");
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            DestroyImmediate(go);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Level prefabs generated.");
    }
}
#endif
