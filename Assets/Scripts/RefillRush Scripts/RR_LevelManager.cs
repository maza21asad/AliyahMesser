using System.Collections;
using UnityEngine;

public class RR_LevelManager : MonoBehaviour
{
    public static RR_LevelManager Instance;

    [Header("Level Data")]
    public RR_LevelData[] levels;

    [Header("Character Animations")]
    public Animator racAnimator;
    public string racIdleAnimation;
    public string racYesAnimation;
    public string racNoAnimation;

    private int currentLevel;
    private int correctCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0);
        PlayIdle();   //=========
    }

    public void LoadLevel(int index)
    {
        currentLevel = index;
        correctCount = 0;

        RR_ItemSpawner.Instance.Spawn(levels[index]);
        RR_LevelUI.Instance.SetInstruction(levels[index].instructionText);

        PlayIdle(); //=========
    }

    // ---------------- REGISTER RESULT ----------------
    public void RegisterCorrectPlacement()
    {
        correctCount++;
        PlayYes(); //=========

        if (correctCount >= levels[currentLevel].requiredCorrectPlacements)
        {
            RR_LevelUI.Instance.ShowLevelComplete(currentLevel + 1);
        }
    }

    //=========
    public void RegisterWrongPlacement()
    {
        PlayNo();
    }

    // ---------------- ANIMATIONS ----------------
    void PlayIdle()
    {
        racAnimator.Play(racIdleAnimation);
    }

    void PlayYes()
    {
        StartCoroutine(PlayThenIdle(racYesAnimation));
    }

    void PlayNo()
    {
        StartCoroutine(PlayThenIdle(racNoAnimation));
    }

    IEnumerator PlayThenIdle(string animName)
    {
        racAnimator.Play(animName);

        yield return null; // wait 1 frame so animator updates

        float length = racAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        racAnimator.Play(racIdleAnimation);
    }

    // ---------------- NEXT LEVEL ----------------
    //public void LoadNextLevel()
    //{
    //    if (currentLevel + 1 >= levels.Length)
    //    {
    //        Debug.Log("All RefillRush levels complete!");
    //        return;
    //    }

    //    LoadLevel(currentLevel + 1);
    //}
    public void LoadNextLevel()
    {
        int nextLevel = currentLevel + 1;

        if (nextLevel >= levels.Length)
        {
            Debug.Log("All RefillRush levels complete!");

            // SHOW FINAL PANEL
            RR_LevelUI.Instance.ShowAllLevelsComplete();
            return;
        }

        LoadLevel(nextLevel);
    }

}