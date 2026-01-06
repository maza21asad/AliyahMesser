using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PillFill_LevelUI : MonoBehaviour
{
    public static PillFill_LevelUI Instance;

    [Header("Feedback")]
    public TextMeshProUGUI feedbackText;

    string[] goodMsgs = { "Great!", "Nice!", "Perfect!", "Good Job!" };
    string[] badMsgs = { "No!", "Drop pill!", "Not that!", "Try again!" };

    [Header("Instruction")]
    public TextMeshProUGUI pillInstructionText;

    [Header("Level Complete Panel")]
    public GameObject levelCompleteUI;
    public TMP_Text levelCompleteMessage;
    public TextMeshProUGUI tapToContinueText;

    [Header("All Levels Complete")]
    public GameObject allLevelsCompletePanel;
    public TextMeshProUGUI allLevelsCompleteText;

    private bool waitForPlayerTouch = false;
    private Tween tapBlinkTween;

    private void Awake()
    {
        Instance = this;
        levelCompleteUI.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        allLevelsCompletePanel.SetActive(false);
        tapToContinueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleTouch();
    }

    public void UpdatePillInstruction(int required)
    {
        //pillInstructionText.text = $"Please collect {required} pills";
        pillInstructionText.text = required == 1 ? "Collect 1 pill for Bunny" : $"Collect {required} pills for Bunny";
    }


    public void ShowFeedback(bool isCorrect)
    {
        string msg = isCorrect
            ? goodMsgs[Random.Range(0, goodMsgs.Length)]
            : badMsgs[Random.Range(0, badMsgs.Length)];

        StartCoroutine(ShowFeedbackRoutine(msg));
    }

    IEnumerator ShowFeedbackRoutine(string msg)
    {
        feedbackText.text = msg;
        feedbackText.alpha = 1;
        feedbackText.gameObject.SetActive(true);

        // pop animation
        feedbackText.transform.localScale = Vector3.zero;
        feedbackText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        // fade out
        yield return new WaitForSeconds(0.7f);
        feedbackText.DOFade(0, 0.4f);

        yield return new WaitForSeconds(0.4f);
        feedbackText.gameObject.SetActive(false);
    }

    //public void ShowLevelComplete(int level)
    //{
    //    levelCompleteUI.SetActive(true);
    //    Transform panel = levelCompleteUI.transform;

    //    panel.localScale = Vector3.zero;
    //    levelCompleteMessage.text = $"Dose {level}\nComplete!";

    //    Sequence seq = DOTween.Sequence();

    //    seq.AppendInterval(1f);
    //    seq.Append(panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack));
    //    seq.AppendInterval(1.5f);
    //    seq.Append(panel.DOScale(0f, 0.35f).SetEase(Ease.InBack));

    //    seq.OnComplete(() =>
    //    {
    //        levelCompleteUI.SetActive(false);
    //        PillFill_LevelManager.Instance.LoadNextLevel();
    //    });
    //}

    public void ShowLevelComplete(int level)
    {
        levelCompleteUI.SetActive(true);

        Transform panel = levelCompleteUI.transform;
        panel.localScale = Vector3.zero;

        levelCompleteMessage.text = $"Dose {level}\nComplete!";

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

        tapBlinkTween = tapToContinueText.DOFade(0.3f, 1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void HandleTouch()
    {
        if (!waitForPlayerTouch) return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            waitForPlayerTouch = false;

            if (tapBlinkTween != null)
                tapBlinkTween.Kill();

            tapToContinueText.gameObject.SetActive(false);

            levelCompleteUI.transform
                .DOScale(0f, 0.35f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    levelCompleteUI.SetActive(false);
                    PillFill_LevelManager.Instance.LoadNextLevel();
                });
        }
    }

    public void ShowAllLevelsComplete()
    {
        allLevelsCompletePanel.SetActive(true);

        Transform panel = allLevelsCompletePanel.transform;
        panel.localScale = Vector3.zero;

        allLevelsCompleteText.text =
            "Congratulations!\nYou completed all levels";

        panel.DOScale(1f, 0.5f)
             .SetEase(Ease.OutBack);
    }


}
