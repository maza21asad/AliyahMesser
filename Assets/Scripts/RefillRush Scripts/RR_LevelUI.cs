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

    [Header("Instruction")]
    public TextMeshProUGUI instructionText;

    [Header("Level Complete Panel")]
    public GameObject levelCompletePanel;
    public TMP_Text levelCompleteText;

    string[] good = { "Correct!", "Nice!", "Well done!" };
    string[] bad = { "Wrong!", "Try again!", "Not here!" };

    private void Awake()
    {
        Instance = this;

        //levelCompletePanel.SetActive(false);
        //feedbackText.gameObject.SetActive(false);
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

    //void HideFeedback()
    //{
    //    feedbackText.gameObject.SetActive(false);
    //}

    // ------------------ LEVEL COMPLETE ------------------
    public void ShowLevelComplete(int level)
    {
        levelCompletePanel.SetActive(true);
        //Invoke(nameof(Next), 2f);
        Transform panel = levelCompletePanel.transform;

        panel.localScale = Vector3.zero;
        levelCompleteText.text = $"Level {level}\nComplete!";

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(1f); // small delay
        seq.Append(panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack));
        seq.AppendInterval(1.5f); // visible time
        seq.Append(panel.DOScale(0f, 0.35f).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            levelCompletePanel.SetActive(false);
            RR_LevelManager.Instance.LoadNextLevel();
        });
    }

    //void Next()
    //{
    //    levelCompletePanel.SetActive(false);
    //    RR_LevelManager.Instance.LoadNextLevel();
    //}
}
