using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;

[System.Serializable]
public struct GridConfig
{
    public int rows;
    public int columns;
    public int WhiteComboCount;



    public GridConfig(int rows, int columns, int whiteComboCount)
    {
        this.rows = rows;
        this.columns = columns;

        this.WhiteComboCount = whiteComboCount;
    }
}

[System.Serializable]
public struct LevelConfig
{
    public GridConfig gridConfig;
    public int time;
    public int level;
    public int shuffleCount;

    public bool isLocked;
}

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "ColorFuse/Level Database", order = 1)]
public class LevelDatabase : ScriptableObject
{
    public int maxRows = 10;
    public int maxCols = 10;
    public int maxWhiteCombos = 6;
    public int defaultTime = 120;

    public List<LevelConfig> configs = new List<LevelConfig>();
    


    public LevelConfig GetLevelConfig(int level)
    {
        return configs.FirstOrDefault(c => c.level == level);
    }
    public bool TryGetGridConfig(int level, out GridConfig gridConfig)
    {
        var config = configs.FirstOrDefault(c => c.level == level);

        if (configs.All(c => c.level != level))
        {
            gridConfig = default;
            return false;
        }

        gridConfig = config.gridConfig;
        return true;
    }

    
}
