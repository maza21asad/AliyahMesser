using UnityEngine;
using UnityEngine.Events;

public class MixAFix_Manager : MonoBehaviour
{
    public static MixAFix_Manager Instance;

    [Header("Level Data")]
    public MixAFix_LevelData[] levels; 
    private int currentLevelIndex = 0;
    private MixAFix_LevelData currentData;

    // Tracking
    private int collectedPowder = 0;
    private int collectedPink = 0;
    private int collectedYellow = 0;
    private int collectedDropper = 0;

    [Header("Events")]
    public UnityEvent onDropToBowl;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Give UI a moment to initialize before loading level
        Invoke(nameof(StartFirstLevel), 0.1f);
    }

    void StartFirstLevel() => LoadLevel(0);

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;

        if (currentLevelIndex >= levels.Length)
        {
            MixAFix_LevelUI.Instance.ShowAllLevelsComplete();
            return;
        }

        currentData = levels[currentLevelIndex];

        // Reset
        collectedPowder = 0;
        collectedPink = 0;
        collectedYellow = 0;
        collectedDropper = 0;

        Debug.Log($"Load Level {index + 1}");
        
        // Update UI Text
        UpdateUI();
    }

    public bool DropScoops(string type)
    {
        bool isNeeded = false;

        switch (type)
        {
            case "Powder":
                if (collectedPowder < currentData.requiredPowder) 
                { 
                    collectedPowder++; 
                    isNeeded = true; 
                }
                break;
            case "PinkCream":
                if (collectedPink < currentData.requiredPinkCream) 
                { 
                    collectedPink++; 
                    isNeeded = true; 
                }
                break;
            case "YellowCream":
                if (collectedYellow < currentData.requiredYellowCream) 
                { 
                    collectedYellow++; 
                    isNeeded = true; 
                }
                break;
            case "Dropper":
                if (collectedDropper < currentData.requiredDrop)
                {
                    collectedDropper++;
                    isNeeded = true; 
                }
                break;
        }

        if (isNeeded)
        {
            // SUCCESS
            if (onDropToBowl != null) onDropToBowl.Invoke();
            MixAFix_LevelUI.Instance.ShowFeedback(true); // "Tasty!"
            UpdateUI();
            CheckLevelCompletion();
            return true;
        }
        else
        {
            // FAILURE (Wrong ingredient or already full)
            Debug.Log("❌ Ingredient not needed!");
            MixAFix_LevelUI.Instance.ShowFeedback(false); // "Not that!"
            return false;
        }
    }

    private void UpdateUI()
    {
        MixAFix_LevelUI.Instance.UpdateIngredients(
            collectedPowder, currentData.requiredPowder,
            collectedPink, currentData.requiredPinkCream,
            collectedYellow, currentData.requiredYellowCream,
            collectedDropper, currentData.requiredDrop
        );
    }

    private void CheckLevelCompletion()
    {
        bool pDone = collectedPowder >= currentData.requiredPowder;
        bool piDone = collectedPink >= currentData.requiredPinkCream;
        bool yDone = collectedYellow >= currentData.requiredYellowCream;
        bool dDone = collectedDropper >= currentData.requiredDrop;

        if (pDone && piDone && yDone && dDone)
        {
            Debug.Log("Level Complete");
            // Show the UI Popup (It handles the tap-to-continue logic)
            MixAFix_LevelUI.Instance.ShowLevelComplete(currentLevelIndex + 1);
        }
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }
}