using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;


[System.Serializable]
public struct GridConfig
{
    public int rows;
    public int columns;
    public int baseColorCount;
    public int mixedColorCount;



    public GridConfig(int rows, int columns, int baseColorCount, int mixedColorCount)
    {
        this.rows = rows;
        this.columns = columns;
        
        this.baseColorCount = baseColorCount;
        this.mixedColorCount = mixedColorCount;
    }
}

[System.Serializable]
public struct LevelConfig
{
    public GridConfig gridConfig;
    public int time;
    public int level;

    
}

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "ColorFuse/Level Database", order = 1)]
public class LevelDatabase : ScriptableObject
{
    public List<LevelConfig> configs = new List<LevelConfig>();
    // public Dictionary<int, LevelConfig> levels = new Dictionary<int, LevelConfig>();

    

    public GridConfig GetGridConfig(int level)
    {
        return configs.FirstOrDefault(c => c.level == level).gridConfig;
    }

    
}
