using DG.Tweening;
using System.Collections;
using System.Text; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixAFix_LevelUI : MonoBehaviour
{
    public static MixAFix_LevelUI Instance;
    
    [Header("Icons (Assign Sprite Assets)")]
    public TMP_SpriteAsset completeImage;
    public TMP_SpriteAsset incompleteImage;
    
    [Header("Configuration")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    [Header("Feedback Popups")]
    public TextMeshProUGUI feedbackText; 
    
    string[] goodMsgs = { "Tasty!", "Yum!", "Perfect Mix!", "Good Job!" };
    string[] badMsgs = { "Not that!", "Wrong ingredient!", "Oops!", "Try again!" };

    [Header("Instruction Panel")]
    public TextMeshProUGUI instructionText; 

    [Header("Level Complete Panel")]
    public GameObject levelCompleteUI;       
    public TMP_Text levelCompleteMessage;    
    public TextMeshProUGUI tapToContinueText; 

    [Header("All Levels Complete")]
    public GameObject allLevelsCompletePanel;

    private bool waitForPlayerTouch = false;
    private Tween tapBlinkTween;
    
    [Header("Star System")]
    public RectTransform[] stars; // Assign your 5 Star Image transforms here

    private void Awake()
    {
        Instance = this;

        if (levelCompleteUI) levelCompleteUI.SetActive(false);
        if (feedbackText) feedbackText.gameObject.SetActive(false);
        if (allLevelsCompletePanel) allLevelsCompletePanel.SetActive(false);
        if (tapToContinueText) tapToContinueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleTouch();
    }
    
    private void Start()
    {
        // Ensure all stars are invisible at the very start of the game
        if (stars != null)
        {
            foreach (RectTransform star in stars)
            {
                if (star != null) star.localScale = Vector3.zero;
            }
        }
    }

    public void AnimateStar(int levelIndex)
    {
        // Level 1 = Index 0, Level 2 = Index 1, etc.
        int starIndex = levelIndex - 1;

        if (stars != null && starIndex >= 0 && starIndex < stars.Length)
        {
            RectTransform targetStar = stars[starIndex];
        
            // Kill any existing tweens to prevent conflicts
            targetStar.DOKill();
        
            // Pop-up Animation: Scale from 0 to 1.2 for a "bounce" then settle at 1.0
            targetStar.DOScale(1.2f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
                targetStar.DOScale(1.0f, 0.2f);
            });
        }
    }

    public void UpdateIngredients(int pCur, int pReq, int piCur, int piReq, int yCur, int yReq, int dCur, int dReq)
    {
        StringBuilder sb = new StringBuilder();

        // 1. POWDER
        if (pReq > 0)
        {
            string label = pReq == 1 ? "SCOOP OF POWDER" : "SCOOPS OF POWDER";
            AppendInstructionLine(sb, pCur, pReq, label, "#4CAF50"); 
        }

        // 2. PINK CREAM 
        if (piReq > 0)
        {
            string label = piReq == 1 ? "SCOOP OF PINK CREAM" : "SCOOPS OF PINK CREAM";
            AppendInstructionLine(sb, piCur, piReq, label, "#E91E63"); 
        }

        // 3. YELLOW CREAM
        if (yReq > 0)
        {
            string label = yReq == 1 ? "SCOOP OF YELLOW CREAM" : "SCOOPS OF YELLOW CREAM";
            AppendInstructionLine(sb, yCur, yReq, label, "#FFC107"); 
        }

        // 4. DROPPER 
        if (dReq > 0)
        {
            string label = dReq == 1 ? "BLUE DROP" : "BLUE DROPS"; 
            AppendInstructionLine(sb, dCur, dReq, label, "#2196F3"); 
        }

        instructionText.text = sb.ToString();
    }

    private void AppendInstructionLine(StringBuilder sb, int current, int required, string itemName, string todoColorHex)
    {
        bool isComplete = current >= required;

        // 1. Pick the correct Asset
        TMP_SpriteAsset targetAsset = isComplete ? completeImage : incompleteImage;

        // 2. Safety Check: If you forgot to drag it in, don't crash
        if (targetAsset == null) 
        {
            // Fallback to text circle/checkmark if asset is missing
            string symbol = isComplete ? "V" : "O"; 
            sb.AppendLine($"<color={todoColorHex}><b>{symbol}  ADD {required} {itemName}</b></color>");
            return;
        }

        // 3. Get the name of the first sprite in that asset
        string spriteName = "0"; 
        if (targetAsset.spriteCharacterTable.Count > 0)
        {
            spriteName = targetAsset.spriteCharacterTable[0].name;
        }
        
        // 2. Apply the variable offset
        string spriteTag = $"<sprite name=\"{spriteName}\">";

        if (isComplete)
        {
            sb.AppendLine($"{spriteTag} <color=#4CAF50><b>ADD {required} {itemName}</b></color>"); 
        }
        else
        {
            sb.AppendLine($"{spriteTag} <color={todoColorHex}><b>ADD {required} {itemName}</b></color>");
        }
    }

    // ... (Keep the rest of your functions: ShowFeedback, etc.) ...
    
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