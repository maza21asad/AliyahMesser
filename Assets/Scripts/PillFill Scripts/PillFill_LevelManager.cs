using UnityEngine;

public class PillFill_LevelManager : MonoBehaviour
{
    public static PillFill_LevelManager Instance;

    public PillFill_LevelData[] levels;

    private int currentLevelIndex = 0;

    private int collectedPills = 0;
    private int requiredPills = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0); // Start Level 1
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;

        PillFill_LevelData data = levels[index];

        collectedPills = 0;
        requiredPills = data.requiredPillCount;

        // spawn pills + obstacles
        PillFill_ItemSpawner.Instance.SpawnLevel(data);

        //Debug.Log("Loaded Level: " + data.levelName);
    }

    public void RegisterPillCollected()
    {
        collectedPills++;

        if (collectedPills >= requiredPills)
        {
            OnLevelCompleted();
        }
    }

    private void OnLevelCompleted()
    {
        Debug.Log("LEVEL COMPLETED: " + (currentLevelIndex + 1));

        //LoadNextLevel();
        PillFill_LevelUI.Instance.ShowLevelComplete(currentLevelIndex + 1);

        Invoke(nameof(LoadNextLevel), 2f); // wait 2 sec before next level
    }

    private void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;

        if (nextIndex >= levels.Length)
        {
            Debug.Log("All 5 levels completed!");
            return;
        }

        LoadLevel(nextIndex);
    }
}
