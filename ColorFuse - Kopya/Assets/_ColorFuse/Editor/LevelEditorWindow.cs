// using UnityEditor;
// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;

// public class LevelEditorWindow : EditorWindow
// {
//     List<LevelConfig> levels = new List<LevelConfig>();

//     int newLevelNumber = 1;
//     int gridWidth = 5;
//     int gridLength = 5;
//     int whites = 1;

//     [MenuItem("Tools/Level Editor")]
//     public static void ShowWindow()
//     {
//         GetWindow<LevelEditorWindow>("Level Editor");
//     }

//     void OnGUI()
//     {
//         GUILayout.Label("Create New Level", EditorStyles.boldLabel);

//         newLevelNumber = EditorGUILayout.IntField("Level Number", newLevelNumber);
//         gridWidth = EditorGUILayout.IntField("Grid Width", gridWidth);
//         gridLength = EditorGUILayout.IntField("Grid Length", gridLength);
//         whites = EditorGUILayout.IntField("Whites", whites);

//         if (GUILayout.Button("Add Level"))
//         {
//             LevelConfig newLevel = new LevelConfig
//             {
//                 level = newLevelNumber,
//                 gridWidth = gridWidth,
//                 gridLength = gridLength,
//                 whites = whites,
//                 seed = SeedGenerator.GenerateSeed(whites)
//             };
//             levels.Add(newLevel);
//         }

//         GUILayout.Space(10);

        
//         if (GUILayout.Button("Save Levels to JSON"))
//         {
//             string json = JsonUtility.ToJson(new LevelConfigListWrapper { levels = levels }, true);

//             // StreamingAssets içine kaydet
//             string folderPath = Path.Combine(Application.dataPath, "StreamingAssets");
//             if (!Directory.Exists(folderPath))
//                 Directory.CreateDirectory(folderPath);

//             string path = Path.Combine(folderPath, "levels.json");
//             File.WriteAllText(path, json);
            
//             Debug.Log("Levels saved to " + path);
//             AssetDatabase.Refresh(); // Unity'de anında görünür
//         }


//         GUILayout.Space(10);
//         GUILayout.Label("Levels in Editor:", EditorStyles.boldLabel);

//         foreach (var lvl in levels)
//         {
//             GUILayout.Label($"Level {lvl.level}: Grid({lvl.gridWidth}x{lvl.gridLength}), Whites: {lvl.whites}, Seed: {lvl.seed}");
//         }
//     }

//     [System.Serializable]
//     public class LevelConfigListWrapper
//     {
//         public List<LevelConfig> levels;
//     }
// }

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelEditorWindow : EditorWindow
{
    private enum SaveFormat { JSON, ScriptableObject }
    
    private List<LevelConfig> levels = new List<LevelConfig>();
    private SaveFormat saveFormat = SaveFormat.JSON;
    private Vector2 scrollPosition;

    // New level defaults
    private int newLevelNumber = 1;
    private int tiles_in_a_column = 5;
    private int tiles_in_a_row = 5;
    private int targetWhiteTiles = 3;
    private float timeLimit = 60f;
    private int moveLimit = 25;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Level Editor");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        saveFormat = (SaveFormat)EditorGUILayout.EnumPopup("Save Format", saveFormat);
        
        DrawLevelCreationSection();
        EditorGUILayout.Space(10);
        DrawLevelListSection();
        EditorGUILayout.Space(10);
        DrawSaveButtons();
    }

    private void DrawLevelCreationSection()
    {
        GUILayout.Label("Create New Level", EditorStyles.boldLabel);
        
        newLevelNumber = EditorGUILayout.IntField("Level Number", newLevelNumber);
        tiles_in_a_row = EditorGUILayout.IntField("Grid Width", tiles_in_a_row);
        tiles_in_a_column = EditorGUILayout.IntField("Grid Height", tiles_in_a_column);
        targetWhiteTiles = EditorGUILayout.IntField("Target White Tiles", targetWhiteTiles);
        timeLimit = EditorGUILayout.FloatField("Time Limit (seconds)", timeLimit);
        moveLimit = EditorGUILayout.IntField("Move Limit", moveLimit);

        if (GUILayout.Button("Add Level", GUILayout.Height(30)))
        {
            AddNewLevel();
        }
    }

    private void AddNewLevel()
    {
        if (levels.Exists(l => l.level == newLevelNumber))
        {
            EditorUtility.DisplayDialog("Error", $"Level {newLevelNumber} already exists!", "OK");
            return;
        }

        levels.Add(new LevelConfig
        {
            level = newLevelNumber,
            tiles_in_a_column= tiles_in_a_column,
            tiles_in_a_row=tiles_in_a_row,
            targetWhiteTiles = targetWhiteTiles,
            timeLimit = timeLimit,
            moveLimit = moveLimit,
            seed = SeedGenerator.GenerateSeed(targetWhiteTiles)
        });

        // Auto-increment for next level
        newLevelNumber++;
    }

    private void DrawLevelListSection()
    {
        GUILayout.Label("Levels in Editor", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
        
        for (int i = 0; i < levels.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            var level = levels[i];
            EditorGUILayout.LabelField($"Level {level.level}: {level.tiles_in_a_row}x{level.tiles_in_a_column}", 
                                      GUILayout.Width(150));
            EditorGUILayout.LabelField($"Whites: {level.targetWhiteTiles}", GUILayout.Width(80));
            EditorGUILayout.LabelField($"Seed: {level.seed}", GUILayout.Width(120));
            
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                levels.RemoveAt(i);
                i--;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();
    }

    private void DrawSaveButtons()
    {
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Save Levels", GUILayout.Height(30)))
        {
            if (saveFormat == SaveFormat.JSON)
            {
                SaveAsJson();
            }
            else
            {
                SaveAsScriptableObjects();
            }
        }
        
        if (GUILayout.Button("Load Existing", GUILayout.Height(30)))
        {
            LoadExistingLevels();
        }
        
        if (GUILayout.Button("Clear All", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Confirm", "Clear all levels?", "Yes", "No"))
            {
                levels.Clear();
            }
        }
        
        GUILayout.EndHorizontal();
    }

    private void SaveAsJson()
    {
        string folderPath = GamePaths.LevelsJsonPath;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string path = Path.Combine(folderPath, "levels.json");
        string json = JsonUtility.ToJson(new LevelConfigListWrapper { levels = levels }, true);
        File.WriteAllText(path, json);
        
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Levels saved to {path}", "OK");
    }

    private void SaveAsScriptableObjects()
    {
        string folderPath = "Assets/Resources/LevelConfigs";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "LevelConfigs");
        }

        foreach (var level in levels)
        {
            var asset = ScriptableObject.CreateInstance<LevelConfigSO>();
            asset.config = level;
            
            string assetPath = Path.Combine(folderPath, $"Level_{level.level}.asset");
            AssetDatabase.CreateAsset(asset, assetPath);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Saved {levels.Count} level SOs", "OK");
    }

    private void LoadExistingLevels()
    {
        levels.Clear();
        
        if (saveFormat == SaveFormat.JSON)
        {
            string path = Path.Combine(Application.dataPath, "StreamingAssets/Levels/levels.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var wrapper = JsonUtility.FromJson<LevelConfigListWrapper>(json);
                levels = wrapper.levels;
            }
        }
        else
        {
            var configs = Resources.LoadAll<LevelConfigSO>("LevelConfigs");
            foreach (var config in configs)
            {
                levels.Add(config.config);
            }
        }
        
        if (levels.Count > 0)
        {
            newLevelNumber = levels[levels.Count - 1].level + 1;
        }
    }

    [System.Serializable]
    private class LevelConfigListWrapper
    {
        public List<LevelConfig> levels;
    }
}

// ScriptableObject version
[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    public LevelConfig config;
}