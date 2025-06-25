using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public int level;
    public int tiles_in_a_row;
    public int tiles_in_a_column;
    public int targetWhiteTiles;
    public float timeLimit;
    public int moveLimit;
    public string seed;
}


public class LevelConfigHolder : MonoBehaviour
{
    public LevelConfig config;
}
