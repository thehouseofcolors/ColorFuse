using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;



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
    
    
}

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "ColorFuse/Level Database", order = 1)]
public class LevelDatabase : ScriptableObject
{
    public List<GridConfig> levels = new List<GridConfig>();

    public GridConfig GetLevel(int level) => levels[level - 1];

    
}
