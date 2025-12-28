using DG.Tweening;
using TMPro;
using UnityEngine;

public class RR_LevelUI : MonoBehaviour
{
    public static RR_LevelUI Instance;

    [Header("Feedback")]
    public TextMeshProUGUI feedbackText;
    public Color correctColor = new Color(0.2f, 0.8f, 0.3f);
    public Color wrongColor = new Color(0.9f, 0.2f, 0.2f);

    public string[] good = { "Correct!", "Nice!", "Well done!" };
    public string[] bad = { "Wrong!", "Try again!", "Not here!" };

    [Header("Instruction")]
    public TextMeshProUGUI instructionText;

    [Header("Level Complete Panel")]
    public GameObject levelCompletePanel;
    public TMP_Text levelCompleteText;
    public TextMeshProUGUI tapToContinueText;

    [Header("All Levels Complete")]
    public GameObject allLevelsCompletePanel;
    public TextMeshProUGUI allLevelsCompleteText;

    private Tween tapBlinkTween;

    private bool waitForPlayerTouch = false;

    private void Awake()
    {
        Instance = this;

        levelCompletePanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        allLevelsCompletePanel.SetActive(false);
    }

    private void Update()
    {
        HandleLevelCompleteTouch();
    }

    // ------------------ INSTRUCTION ------------------
    public void SetInstruction(string msg)
    {
        instructionText.text = msg;
    }

    // ------------------ FEEDBACK ------------------
    public void ShowFeedback(bool correct)
    {
        feedbackText.text = correct
            ? good[Random.Range(0, good.Length)]
            : bad[Random.Range(0, bad.Length)];

        feedbackText.color = correct ? correctColor : wrongColor;

        feedbackText.gameObject.SetActive(true);
        //Invoke(nameof(HideFeedback), 1f);
        feedbackText.alpha = 1f;

        feedbackText.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(feedbackText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.6f);
        seq.Append(feedbackText.DOFade(0f, 0.3f));
        seq.OnComplete(() =>
        {
            feedbackText.gameObject.SetActive(false);
        });
    }

    public void ShowLevelComplete(int level)
    {
        levelCompletePanel.SetActive(true);

        Transform panel = levelCompletePanel.transform;
        panel.localScale = Vector3.zero;

        levelCompleteText.text = $"Level {level} Complete!";

        // APPEAR animation (keep DOTween)
        panel.DOScale(1f, 0.45f)
             .SetEase(Ease.OutBack)
             .OnComplete(() =>
             {
                 // Now wait for player input
                 waitForPlayerTouch = true;

                 StartTapToContinue();
             });
    }

    void StartTapToContinue()
    {
        tapToContinueText.gameObject.SetActive(true);

        tapBlinkTween = tapToContinueText
            .DOFade(0.2f, 0.7f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }


    void HandleLevelCompleteTouch()
    {
        if (!waitForPlayerTouch) return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            waitForPlayerTouch = false;

            if (tapBlinkTween != null)
                tapBlinkTween.Kill();

            Transform panel = levelCompletePanel.transform;

            panel.DOScale(0f, 0.35f)
                 .SetEase(Ease.InBack)
                 .OnComplete(() =>
                 {
                     levelCompletePanel.SetActive(false);
                     RR_LevelManager.Instance.LoadNextLevel();
                 });
        }
    }

    public void ShowAllLevelsComplete()
    {
        allLevelsCompletePanel.SetActive(true);

        Transform panel = allLevelsCompletePanel.transform;
        panel.localScale = Vector3.zero;

        allLevelsCompleteText.text =
            "Congratulations!\nYou have completed all the levels";

        panel.DOScale(1f, 0.5f)
             .SetEase(Ease.OutBack);
    }

}
