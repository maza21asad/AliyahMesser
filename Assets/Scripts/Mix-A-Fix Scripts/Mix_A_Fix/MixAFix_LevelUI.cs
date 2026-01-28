using DG.Tweening;
using System.Collections;
using System.Text; // Required for StringBuilder
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixAFix_LevelUI : MonoBehaviour
{
    public static MixAFix_LevelUI Instance;
    
    public Sprite completeImage;
    public Sprite incompleteImage;
    
    [Header("Configuration")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    [Header("Feedback Popups")]
    public TextMeshProUGUI feedbackText; 
    
    string[] goodMsgs = { "Tasty!", "Yum!", "Perfect Mix!", "Good Job!" };
    string[] badMsgs = { "Not that!", "Wrong ingredient!", "Oops!", "Try again!" };

    [Header("Instruction Panel")]
    public TextMeshProUGUI instructionText; 

    // ... (Keep Level Complete UI variables here) ...
    [Header("Level Complete Panel")]
    public GameObject levelCompleteUI;       
    public TMP_Text levelCompleteMessage;    
    public TextMeshProUGUI tapToContinueText; 

    [Header("All Levels Complete")]
    public GameObject allLevelsCompletePanel;

    private bool waitForPlayerTouch = false;
    private Tween tapBlinkTween;

    private void Awake()
    {
        Instance = this;
        // ... (Keep existing Awake logic) ...
        if (levelCompleteUI) levelCompleteUI.SetActive(false);
        if (feedbackText) feedbackText.gameObject.SetActive(false);
        if (allLevelsCompletePanel) allLevelsCompletePanel.SetActive(false);
        if (tapToContinueText) tapToContinueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleTouch();
    }

    // ---------------------------------------------------------
    //  NEW: DYNAMIC INSTRUCTION LIST
    // ---------------------------------------------------------
    public void UpdateIngredients(int pCur, int pReq, int piCur, int piReq, int yCur, int yReq, int dCur, int dReq)
    {
        StringBuilder sb = new StringBuilder();

        // 1. POWDER
        if (pReq > 0)
        {
            string label = pReq == 1 ? "SCOOP OF POWDER" : "SCOOPS OF POWDER";
            AppendInstructionLine(sb, pCur, pReq, label, "#4CAF50"); // Greenish text
        }

        // 2. PINK CREAM (Matches "Red Cream" in your image)
        if (piReq > 0)
        {
            string label = piReq == 1 ? "SCOOP OF PINK CREAM" : "SCOOPS OF PINK CREAM";
            AppendInstructionLine(sb, piCur, piReq, label, "#E91E63"); // Pink/Red text
        }

        // 3. YELLOW CREAM
        if (yReq > 0)
        {
            string label = yReq == 1 ? "SCOOP OF YELLOW CREAM" : "SCOOPS OF YELLOW CREAM";
            AppendInstructionLine(sb, yCur, yReq, label, "#FFC107"); // Gold/Yellow text
        }

        // 4. DROPPER (Matches "Blue Drops" in your image)
        if (dReq > 0)
        {
            string label = dReq == 1 ? "BLUE DROP" : "BLUE DROPS"; // Custom label for dropper
            AppendInstructionLine(sb, dCur, dReq, label, "#2196F3"); // Blue text
        }

        instructionText.text = sb.ToString();
    }

    // Helper to format the line with Checkmarks/Colors
    private void AppendInstructionLine(StringBuilder sb, int current, int required, string itemName, string todoColorHex)
    {
        bool isComplete = current >= required;

        if (isComplete)
        {
            // STYLE: Green Checkmark + Green Text + Strikethrough (optional) or just bold
            // Format: ☑ ADD {N} {ITEM}
            sb.AppendLine($"<color=#4CAF50><b>✓  ADD {required} {itemName}</b></color>"); 
            
        }
        else
        {
            // STYLE: Empty Circle + Specific Color Text
            // Format: ○ ADD {N} {ITEM}
            // using "todoColorHex" allows "Blue Drops" to be blue text, etc.
            sb.AppendLine($"<color=#9E9E9E>○</color> <color={todoColorHex}><b>ADD {required} {itemName}</b></color>");
        }
    }

    // ... (Keep ShowFeedback, ShowLevelComplete, StartTapToContinue, HandleTouch, ShowAllLevelsComplete) ...
    
    // Paste the rest of your existing functions here:
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
        if (isCorrect) feedbackText.color = correctColor;
        else feedbackText.color = wrongColor;

        feedbackText.alpha = 1;
        feedbackText.gameObject.SetActive(true);
        feedbackText.transform.localScale = Vector3.zero;
        feedbackText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.7f);
        feedbackText.DOFade(0, 0.4f);
        yield return new WaitForSeconds(0.4f);
        feedbackText.gameObject.SetActive(false);
    }

    public void ShowLevelComplete(int levelIndex)
    {
        levelCompleteUI.SetActive(true);
        Transform panel = levelCompleteUI.transform;
        panel.localScale = Vector3.zero;
        levelCompleteMessage.text = $"Mix {levelIndex}\nComplete!";

        panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack)
             .OnComplete(() => { waitForPlayerTouch = true; StartTapToContinue(); });
    }

    void StartTapToContinue()
    {
        tapToContinueText.gameObject.SetActive(true);
        tapBlinkTween = tapToContinueText.DOFade(0.3f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    void HandleTouch()
    {
        if (!waitForPlayerTouch) return;
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            waitForPlayerTouch = false;
            if (tapBlinkTween != null) tapBlinkTween.Kill();
            tapToContinueText.gameObject.SetActive(false);
            levelCompleteUI.transform.DOScale(0f, 0.35f).SetEase(Ease.InBack)
                .OnComplete(() => { levelCompleteUI.SetActive(false); MixAFix_Manager.Instance.LoadNextLevel(); });
        }
    }

    public void ShowAllLevelsComplete()
    {
        allLevelsCompletePanel.SetActive(true);
        allLevelsCompletePanel.transform.localScale = Vector3.zero;
        allLevelsCompletePanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
}