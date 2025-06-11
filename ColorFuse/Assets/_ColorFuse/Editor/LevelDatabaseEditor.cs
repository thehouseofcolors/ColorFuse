using UnityEditor;
using UnityEngine;

// --- ORTAK ÜRETİM METODU ---


public static class LevelGeneratorUtility
{
    public static void GenerateNextLevel(LevelDatabase db)
    {
        int level = db.configs.Count > 0 ? db.configs[^1].level + 1 : 1;

        int rows = Mathf.Min(5 + (level / 10), db.maxRows);
        int cols = Mathf.Min(5 + (level / 10), db.maxCols);
        int comboCount = Mathf.Clamp(2 + (level / 10), 2, rows * cols); // örnek artış

        GridConfig gridConfig = new GridConfig(rows, cols, comboCount);

        LevelConfig config = new LevelConfig
        {
            level = level,
            time = db.defaultTime,
            isLocked = true,
            gridConfig = gridConfig
        };

        db.configs.Add(config);

        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
        Debug.Log($"Seviye {level} başarıyla eklendi.");
    }
}

// --- INSPECTOR BUTONU ---
[CustomEditor(typeof(LevelDatabase))]
public class LevelDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelDatabase db = (LevelDatabase)target;

        if (GUILayout.Button("10 Seviye Üret (Inspector)"))
        {
            LevelGeneratorUtility.GenerateNextLevel(db);
        }
    }
}

// --- MENÜDEN PENCERE ---
public class LevelGeneratorWindow : EditorWindow
{
    private LevelDatabase levelDatabase;
    private int generateCount = 10;

    [MenuItem("ColorFuse/Seviye Üretici")]
    public static void ShowWindow()
    {
        GetWindow<LevelGeneratorWindow>("Seviye Üretici");
    }

    void OnGUI()
    {
        GUILayout.Label("Seviye Veritabanı", EditorStyles.boldLabel);
        levelDatabase = (LevelDatabase)EditorGUILayout.ObjectField("Database", levelDatabase, typeof(LevelDatabase), false);

        generateCount = EditorGUILayout.IntSlider("Kaç seviye üretilsin?", generateCount, 1, 50);

        if (levelDatabase != null)
        {
            if (GUILayout.Button($"{generateCount} Seviye Üret"))
            {
                LevelGeneratorUtility.GenerateNextLevel(levelDatabase);
            }
        }
    }
}
