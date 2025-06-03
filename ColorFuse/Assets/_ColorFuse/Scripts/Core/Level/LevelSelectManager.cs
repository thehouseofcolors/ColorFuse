using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelDatabase levelDatabase;

    private void Start()
    {
        if (!ValidateFields())
        {
            Debug.LogError("LevelSelectManager: Missing required references!");
            return;
        }

        CreateLevelButtons();
    }

    private bool ValidateFields()
    {
        return levelButtonPrefab != null && contentParent != null && levelDatabase != null;
    }

    private void CreateLevelButtons()
    {
        if (levelDatabase.configs == null || levelDatabase.configs.Count == 0)
        {
            Debug.LogWarning("LevelSelectManager: No level configs found in database.");
            return;
        }

        foreach (LevelConfig config in levelDatabase.configs)
        {
            if (config.level <= 0)
            {
                Debug.LogWarning($"Skipping invalid level config with level number: {config.level}");
                continue;
            }

            GameObject buttonObj = Instantiate(levelButtonPrefab, contentParent);
            LevelButton levelButton = buttonObj.GetComponent<LevelButton>();

            if (levelButton != null)
            {
                levelButton.Setup(config);
            }
            else
            {
                Debug.LogError("LevelButton component missing on prefab.");
            }
        }
    }
}
