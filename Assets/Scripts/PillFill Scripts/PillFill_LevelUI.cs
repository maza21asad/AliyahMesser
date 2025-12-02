//using DG.Tweening;
//using System.Collections;
//using TMPro;
//using UnityEngine;

//public class PillFill_LevelUI : MonoBehaviour
//{
//    public static PillFill_LevelUI Instance;

//    public TextMeshProUGUI feedbackText;
//    public GameObject levelCompleteUI;

//    string[] messages = { "Great!", "Nice!", "Perfect!", "Good Job!" };

//    public TMP_Text levelCompleteMessage;  // <-- ADD THIS

//    private void Awake()
//    {
//        Instance = this;
//        levelCompleteUI.SetActive(false);
//        feedbackText.gameObject.SetActive(false);
//    }

//    public void ShowFeedback()
//    {
//        string msg = messages[Random.Range(0, messages.Length)];
//        StartCoroutine(ShowFeedbackRoutine(msg));
//    }

//    IEnumerator ShowFeedbackRoutine(string msg)
//    {
//        feedbackText.text = msg;
//        feedbackText.alpha = 1;
//        feedbackText.gameObject.SetActive(true);

//        // fade out
//        for (float t = 1; t > 0; t -= Time.deltaTime)
//        {
//            feedbackText.alpha = t;
//            yield return null;
//        }

//        feedbackText.gameObject.SetActive(false);
//    }

//    public void ShowLevelComplete(int level)
//    {
//        // Activate & reset scale
//        levelCompleteUI.SetActive(true);
//        Transform panel = levelCompleteUI.transform;

//        panel.localScale = Vector3.zero;
//        levelCompleteMessage.text = $"Good job!\nLevel {level} Complete!";

//        // SEQUENCE FOR SMOOTH TRANSITION
//        Sequence seq = DOTween.Sequence();

//        seq.AppendInterval(1f);   // Delay before showing panel

//        // POP-IN
//        seq.Append(panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack));

//        seq.AppendInterval(1.5f); // Keep panel visible for a moment

//        // POP-OUT (smooth)
//        seq.Append(panel.DOScale(0f, 0.35f).SetEase(Ease.InBack));

//        // When pop-out finishes → go to next level
//        seq.OnComplete(() =>
//        {
//            levelCompleteUI.SetActive(false);
//            PillFill_LevelManager.Instance.LoadNextLevel();
//        });
//    }

//    public void HideLevelComplete()
//    {
//        levelCompleteUI.SetActive(false);
//    }
//}

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PillFill_LevelUI : MonoBehaviour
{
    public static PillFill_LevelUI Instance;

    public TextMeshProUGUI feedbackText;
    public GameObject levelCompleteUI;

    public TMP_Text levelCompleteMessage;

    string[] goodMsgs = { "Great!", "Nice!", "Perfect!", "Good Job!" };
    string[] badMsgs = { "No!", "Drop pill!", "Not that!", "Try again!" };

    private void Awake()
    {
        Instance = this;
        levelCompleteUI.SetActive(false);
        feedbackText.gameObject.SetActive(false);
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

    public void ShowLevelComplete(int level)
    {
        levelCompleteUI.SetActive(true);
        Transform panel = levelCompleteUI.transform;

        panel.localScale = Vector3.zero;
        levelCompleteMessage.text = $"Good job!\nLevel {level} Complete!";

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(1f);
        seq.Append(panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack));
        seq.AppendInterval(1.5f);
        seq.Append(panel.DOScale(0f, 0.35f).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            levelCompleteUI.SetActive(false);
            PillFill_LevelManager.Instance.LoadNextLevel();
        });
    }
}
