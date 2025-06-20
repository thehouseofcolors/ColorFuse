using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    // public int level;
    // public int tiles_in_a_row;
    // public int tiles_in_a_column;
    // public int targetWhiteTiles;
    // public float timeLimit;
    // public int moveLimit;
    // public int seed;
    public List<LevelConfig> levelConfigs;
}
