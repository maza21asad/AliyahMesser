using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PillFill_LevelUI : MonoBehaviour
{
    public static PillFill_LevelUI Instance;

    public TextMeshProUGUI feedbackText;
    public GameObject levelCompleteUI;

    string[] messages = { "Great!", "Nice!", "Perfect!", "Good Job!" };

    public TMP_Text levelCompleteMessage;  // <-- ADD THIS

    private void Awake()
    {
        Instance = this;
        levelCompleteUI.SetActive(false);
        feedbackText.gameObject.SetActive(false);
    }

    public void ShowFeedback()
    {
        string msg = messages[Random.Range(0, messages.Length)];
        StartCoroutine(ShowFeedbackRoutine(msg));
    }

    IEnumerator ShowFeedbackRoutine(string msg)
    {
        feedbackText.text = msg;
        feedbackText.alpha = 1;
        feedbackText.gameObject.SetActive(true);

        // fade out
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            feedbackText.alpha = t;
            yield return null;
        }

        feedbackText.gameObject.SetActive(false);
    }

    public void ShowLevelComplete(int level)
    {
        //levelCompleteUI.SetActive(true);

        //levelCompleteMessage.text = $"Good job!\nLevel {level} Complete!";
        // 1) Hide and prepare starting scale
        levelCompleteUI.SetActive(true);
        levelCompleteUI.transform.localScale = Vector3.zero;

        levelCompleteMessage.text = $"Good job!\nLevel {level} Complete!";

        // 2) Delay 1 second, then popup
        DOVirtual.DelayedCall(1f, () =>
        {
            levelCompleteUI.transform
                .DOScale(Vector3.one, 0.45f)   // scale animation
                .SetEase(Ease.OutBack);        // nice pop-out effect
        });
    }

    public void HideLevelComplete()
    {
        levelCompleteUI.SetActive(false);
    }
}
