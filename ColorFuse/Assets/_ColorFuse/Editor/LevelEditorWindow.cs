using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelEditorWindow : EditorWindow
{
    List<LevelConfig> levels = new List<LevelConfig>();

    int newLevelNumber = 1;
    int gridWidth = 5;
    int gridLength = 5;
    int whites = 1;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Create New Level", EditorStyles.boldLabel);

        newLevelNumber = EditorGUILayout.IntField("Level Number", newLevelNumber);
        gridWidth = EditorGUILayout.IntField("Grid Width", gridWidth);
        gridLength = EditorGUILayout.IntField("Grid Length", gridLength);
        whites = EditorGUILayout.IntField("Whites", whites);

        if (GUILayout.Button("Add Level"))
        {
            LevelConfig newLevel = new LevelConfig
            {
                level = newLevelNumber,
                gridWidth = gridWidth,
                gridLength = gridLength,
                whites = whites,
                seed = SeedGenerator.GenerateSeed(whites)
            };
            levels.Add(newLevel);
        }

        GUILayout.Space(10);

        
        if (GUILayout.Button("Save Levels to JSON"))
        {
            string json = JsonUtility.ToJson(new LevelConfigListWrapper { levels = levels }, true);

            // StreamingAssets içine kaydet
            string folderPath = Path.Combine(Application.dataPath, "StreamingAssets");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string path = Path.Combine(folderPath, "levels.json");
            File.WriteAllText(path, json);
            
            Debug.Log("Levels saved to " + path);
            AssetDatabase.Refresh(); // Unity'de anında görünür
        }


        GUILayout.Space(10);
        GUILayout.Label("Levels in Editor:", EditorStyles.boldLabel);

        foreach (var lvl in levels)
        {
            GUILayout.Label($"Level {lvl.level}: Grid({lvl.gridWidth}x{lvl.gridLength}), Whites: {lvl.whites}, Seed: {lvl.seed}");
        }
    }

    [System.Serializable]
    public class LevelConfigListWrapper
    {
        public List<LevelConfig> levels;
    }
}

