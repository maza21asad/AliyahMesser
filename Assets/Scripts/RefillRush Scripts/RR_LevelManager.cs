using UnityEngine;

public class RR_LevelManager : MonoBehaviour
{
    public static RR_LevelManager Instance;

    public RR_LevelData[] levels;
    private int currentLevel;
    private int correctCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int index)
    {
        currentLevel = index;
        correctCount = 0;

        RR_ItemSpawner.Instance.Spawn(levels[index]);
        RR_LevelUI.Instance.SetInstruction(levels[index].instructionText);
    }

    public void RegisterCorrectPlacement()
    {
        correctCount++;

        if (correctCount >= levels[currentLevel].requiredCorrectPlacements)
        {
            RR_LevelUI.Instance.ShowLevelComplete(currentLevel + 1);
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevel + 1 >= levels.Length)
        {
            Debug.Log("All RefillRush levels complete!");
            return;
        }

        LoadLevel(currentLevel + 1);
    }
}
