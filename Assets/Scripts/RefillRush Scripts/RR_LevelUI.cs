using TMPro;
using UnityEngine;

public class RR_LevelUI : MonoBehaviour
{
    public static RR_LevelUI Instance;

    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI instructionText;
    public GameObject levelCompletePanel;

    string[] good = { "Correct!", "Nice!", "Well done!" };
    string[] bad = { "Wrong!", "Try again!", "Not here!" };

    private void Awake()
    {
        Instance = this;
    }

    public void SetInstruction(string msg)
    {
        instructionText.text = msg;
    }

    public void ShowFeedback(bool correct)
    {
        feedbackText.text = correct
            ? good[Random.Range(0, good.Length)]
            : bad[Random.Range(0, bad.Length)];

        feedbackText.gameObject.SetActive(true);
        Invoke(nameof(HideFeedback), 1f);
    }

    void HideFeedback()
    {
        feedbackText.gameObject.SetActive(false);
    }

    public void ShowLevelComplete(int level)
    {
        levelCompletePanel.SetActive(true);
        Invoke(nameof(Next), 2f);
    }

    void Next()
    {
        levelCompletePanel.SetActive(false);
        RR_LevelManager.Instance.LoadNextLevel();
    }
}
