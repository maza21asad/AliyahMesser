using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixAFix_LevelUI : MonoBehaviour
{
    public static MixAFix_LevelUI Instance;
    
    [Header("Configuration")]
    public Color correctColor = Color.green; // Set this in Inspector
    public Color wrongColor = Color.red;     // Set this in Inspector

    [Header("Feedback Popups")]
    public TextMeshProUGUI feedbackText; // Assign a center-screen text
    
    // Cooking-themed messages
    string[] goodMsgs = { "Tasty!", "Yum!", "Perfect Mix!", "Good Job!" };
    string[] badMsgs = { "Not that!", "Wrong ingredient!", "Oops!", "Try again!" };

    [Header("Instruction Panel")]
    public TextMeshProUGUI instructionText; // Assign top-screen text

    [Header("Level Complete Panel")]
    public GameObject levelCompleteUI;       // The panel background
    public TMP_Text levelCompleteMessage;    // "Level 1 Complete!"
    public TextMeshProUGUI tapToContinueText; // "Tap to continue"

    [Header("All Levels Complete")]
    public GameObject allLevelsCompletePanel;

    private bool waitForPlayerTouch = false;
    private Tween tapBlinkTween;

    private void Awake()
    {
        Instance = this;
        
        // Hide screens initially
        if (levelCompleteUI) levelCompleteUI.SetActive(false);
        if (feedbackText) feedbackText.gameObject.SetActive(false);
        if (allLevelsCompletePanel) allLevelsCompletePanel.SetActive(false);
        if (tapToContinueText) tapToContinueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleTouch();
    }

    // --- 1. Instruction Logic ---
    public void UpdateIngredients(int pCur, int pReq, int piCur, int piReq, int yCur, int yReq, int dCur, int dReq)
    {
        // Simple format: "Powder: 1/2  Pink: 0/1  Yellow: 1/1"
        // You can use icons (sprites) in TextMeshPro if you want to get fancy later
        instructionText.text =
            $"Powder Cream : {pCur}/{pReq}\n" +
            $"Pink Cream: {piCur}/{piReq}\n" +
            $"Yellow Cream: {yCur}/{yReq}\n" +
            $"Dropper: {dCur}/{dReq}";
    }

    // --- 2. Feedback "Juice" (Copied from PillFill) ---
    public void ShowFeedback(bool isCorrect)
    {
        string msg = isCorrect
            ? goodMsgs[Random.Range(0, goodMsgs.Length)]
            : badMsgs[Random.Range(0, badMsgs.Length)];

        StartCoroutine(ShowFeedbackRoutine(msg, isCorrect));
    }

    IEnumerator ShowFeedbackRoutine(string msg, bool isCorrect)
    {
        feedbackText.text = msg;
    
        // Logic: If correct, use default color (e.g., White or Green). 
        // If wrong, use Red.
        if (isCorrect)
        {
            feedbackText.color = correctColor; // Or Color.green, or your original font color
        }
        else
        {
            feedbackText.color = wrongColor;
        }

        feedbackText.alpha = 1;
        feedbackText.gameObject.SetActive(true);

        // Pop Scale Animation (Copied from your previous script)
        feedbackText.transform.localScale = Vector3.zero;
        feedbackText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.7f);
    
        // Fade Out
        feedbackText.DOFade(0, 0.4f);
        yield return new WaitForSeconds(0.4f);
    
        feedbackText.gameObject.SetActive(false);
    }

    // Level Complete Logic (Copied from PillFill) ---
    public void ShowLevelComplete(int levelIndex)
    {
        levelCompleteUI.SetActive(true);
        Transform panel = levelCompleteUI.transform;
        
        // Reset scale
        panel.localScale = Vector3.zero;
        levelCompleteMessage.text = $"Mix {levelIndex}\nComplete!";

        // Animate Panel In
        panel.DOScale(1f, 0.45f)
             .SetEase(Ease.OutBack)
             .OnComplete(() =>
             {
                 waitForPlayerTouch = true;
                 StartTapToContinue();
             });
    }

    void StartTapToContinue()
    {
        tapToContinueText.gameObject.SetActive(true);
        
        // Blinking Text Effect
        tapBlinkTween = tapToContinueText.DOFade(0.3f, 1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void HandleTouch()
    {
        if (!waitForPlayerTouch) return;

        // Detect click or tap
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            waitForPlayerTouch = false;

            if (tapBlinkTween != null) tapBlinkTween.Kill();
            tapToContinueText.gameObject.SetActive(false);

            // Animate Panel Out
            levelCompleteUI.transform
                .DOScale(0f, 0.35f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    levelCompleteUI.SetActive(false);
                    // TRIGGER NEXT LEVEL
                    MixAFix_Manager.Instance.LoadNextLevel();
                });
        }
    }

    public void ShowAllLevelsComplete()
    {
        allLevelsCompletePanel.SetActive(true);
        allLevelsCompletePanel.transform.localScale = Vector3.zero;
        
        allLevelsCompletePanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
}