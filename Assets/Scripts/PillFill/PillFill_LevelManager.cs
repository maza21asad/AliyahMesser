using UnityEngine;

public class PillFill_LevelManager : MonoBehaviour
{
    public static PillFill_LevelManager Instance;

    public PillFill_LevelData[] levels;
    public int currentLevelIndex = 0;

    private int collected = 0;
    private int required;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        PillFill_LevelData data = levels[index];

        collected = 0;
        required = data.requiredPillCount;

        // UI can update here
        //if (data.background != null)
        //    UIManager.Instance.SetBackground(data.background);

        //UIManager.Instance.SetInstruction(data.instructionText);

        // spawn pills + obstacles
        PillFill_ItemSpawner.Instance.SpawnLevel(data);
    }

    public void RegisterPillCollected()
    {
        collected++;

        if (collected >= required)
        {
            Debug.Log("🎉 Level Complete!");
            NextLevel();
        }
    }

    private void NextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("All levels finished!");
            return;
        }

        LoadLevel(currentLevelIndex);
    }
}
