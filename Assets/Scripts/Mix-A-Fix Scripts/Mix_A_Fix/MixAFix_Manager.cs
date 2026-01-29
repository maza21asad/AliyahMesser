using UnityEngine;
using UnityEngine.Events;

public class MixAFix_Manager : MonoBehaviour
{
    public static MixAFix_Manager Instance;

    [Header("Bowl Settings")]
    public CanvasGroup bowl1CanvasGroup;
    public CanvasGroup bowl2CanvasGroup;
    public CanvasGroup bowl3CanvasGroup;
    public CanvasGroup bowl4CanvasGroup;
    public CanvasGroup bowl5CanvasGroup;

    [Header("Level Data")]
    public MixAFix_LevelData[] levels; 
    private int currentLevelIndex = 0;
    private MixAFix_LevelData currentData;

    // Tracking
    private int collectedPowder = 0;
    private int collectedPink = 0;
    private int collectedYellow = 0;
    private int collectedDropper = 0;

    [Header("UI Connections")]
    public MixAFix_SliderProgress sliderBar; // Drag the Slider object here
    public UnityEvent onDropToBowl;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetBowlInteractable(false);
        Invoke(nameof(StartFirstLevel), 0.1f);
    }
    
    public void SetBowlInteractable(bool state)
    {
        if (bowl1CanvasGroup != null) bowl1CanvasGroup.blocksRaycasts = state;
        if (bowl2CanvasGroup != null) bowl2CanvasGroup.blocksRaycasts = state;
        if (bowl3CanvasGroup != null) bowl3CanvasGroup.blocksRaycasts = state;
        if (bowl4CanvasGroup != null) bowl4CanvasGroup.blocksRaycasts = state;
        if (bowl5CanvasGroup != null) bowl5CanvasGroup.blocksRaycasts = state;
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

        // Reset local ingredients
        collectedPowder = 0;
        collectedPink = 0;
        collectedYellow = 0;
        collectedDropper = 0;

        UpdateUI();
        UpdateProgress(); // Reset bar position for the start of the level
    }

    public bool DropScoops(string type)
    {
        bool isNeeded = false;

        switch (type)
        {
            case "Powder":
                if (collectedPowder < currentData.requiredPowder) { collectedPowder++; isNeeded = true; }
                break;
            case "PinkCream":
                if (collectedPink < currentData.requiredPinkCream) { collectedPink++; isNeeded = true; }
                break;
            case "YellowCream":
                if (collectedYellow < currentData.requiredYellowCream) { collectedYellow++; isNeeded = true; }
                break;
            case "Dropper":
                if (collectedDropper < currentData.requiredDrop) { collectedDropper++; isNeeded = true; }
                break;
        }

        if (isNeeded)
        {
            if (onDropToBowl != null) onDropToBowl.Invoke();
            MixAFix_LevelUI.Instance.ShowFeedback(true);
            
            UpdateProgress(); // Update slider fill
            CheckLevelCompletion();
            UpdateUI();
            return true;
        }
        else
        {
            MixAFix_LevelUI.Instance.ShowFeedback(false);
            return false;
        }
    }

    private void UpdateProgress()
    {
        if (sliderBar == null || currentData == null) return;

        // 1. Calculate total items needed for THIS level
        int totalNeeded = currentData.requiredPowder + currentData.requiredPinkCream + 
                          currentData.requiredYellowCream + currentData.requiredDrop;
        
        // 2. Calculate total collected currently
        int totalCollected = collectedPowder + collectedPink + collectedYellow + collectedDropper;

        // 3. Math for 5 Levels
        // How much of the current 20% chunk is filled?
        float levelProgress = totalNeeded > 0 ? (float)totalCollected / totalNeeded : 0;
        
        float levelChunk = 1.0f / 5.0f; // Each level is 0.2 of the bar
        float baseProgress = currentLevelIndex * levelChunk;
        
        float finalFillAmount = baseProgress + (levelProgress * levelChunk);

        sliderBar.UpdateBar(finalFillAmount);
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
            MixAFix_LevelUI.Instance.ShowLevelComplete(currentLevelIndex + 1);
        }
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }
}