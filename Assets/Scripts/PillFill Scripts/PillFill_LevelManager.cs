//using UnityEngine;

//public class PillFill_LevelManager : MonoBehaviour
//{
//    public static PillFill_LevelManager Instance;

//    public PillFill_LevelData[] levels;

//    private int currentLevelIndex = 0;

//    private int collectedPills = 0;
//    private int requiredPills = 0;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    private void Start()
//    {
//        LoadLevel(0); // Start Level 1
//    }

//    public void LoadLevel(int index)
//    {
//        currentLevelIndex = index;

//        PillFill_LevelData data = levels[index];

//        collectedPills = 0;
//        requiredPills = data.requiredPillCount;

//        // spawn pills + obstacles
//        PillFill_ItemSpawner.Instance.SpawnLevel(data);

//        //Debug.Log("Loaded Level: " + data.levelName);
//    }

//    public void RegisterPillCollected()
//    {
//        collectedPills++;

//        if (collectedPills >= requiredPills)
//        {
//            OnLevelCompleted();
//        }
//    }

//    private void OnLevelCompleted()
//    {
//        Debug.Log("LEVEL COMPLETED: " + (currentLevelIndex + 1));

//        //LoadNextLevel();
//        PillFill_LevelUI.Instance.ShowLevelComplete(currentLevelIndex + 1);

//        Invoke(nameof(LoadNextLevel), 3.5f); // wait 2 sec before next level
//    }

//    public void LoadNextLevel()
//    {
//        int nextIndex = currentLevelIndex + 1;

//        if (nextIndex >= levels.Length)
//        {
//            Debug.Log("All 5 levels completed!");
//            return;
//        }

//        // HIDE the previous level popup
//        //PillFill_LevelUI.Instance.HideLevelComplete();

//        LoadLevel(nextIndex);
//    }
//}


using UnityEngine;
using System.Collections;

public class PillFill_LevelManager : MonoBehaviour
{
    public static PillFill_LevelManager Instance;

    [Header("Level Data")]
    public PillFill_LevelData[] levels;

    [Header("Rabbit Animations")]
    public Animator rabbitAnimator;

    // Idle animation per level (size = number of levels)
    public string[] rabbit_idleAnimations;

    // Revive animation between levels (size = number of levels - 1)
    public string[] rabbit_reviveAnimations;

    public string rabbit_happyAnimation;

    private int currentLevelIndex = 0;

    private int collectedPills = 0;
    private int requiredPills = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;

        PillFill_LevelData data = levels[index];

        collectedPills = 0;
        requiredPills = data.requiredPillCount;

        // Spawn items
        PillFill_ItemSpawner.Instance.SpawnLevel(data);

        // Play this level's rabbit idle animation
        PlayRabbitIdle(currentLevelIndex);
    }

    private void PlayRabbitIdle(int levelIndex)
    {
        if (levelIndex < rabbit_idleAnimations.Length)
        {
            string anim = rabbit_idleAnimations[levelIndex];
            rabbitAnimator.Play(anim);
        }
    }

    public void RegisterPillCollected()
    {
        collectedPills++;

        if (collectedPills >= requiredPills)
        {
            StartCoroutine(HandleLevelComplete());
        }
    }

    private IEnumerator HandleLevelComplete()
    {
        // Step 1: Play revive animation (except after last level)
        if (currentLevelIndex < rabbit_reviveAnimations.Length)
        {

            yield return new WaitForSeconds(1f);
            string reviveAnim = rabbit_reviveAnimations[currentLevelIndex];
            rabbitAnimator.Play(reviveAnim);

            //// Wait revive animation time
            //yield return new WaitForSeconds(
            //    rabbitAnimator.GetCurrentAnimatorStateInfo(0).length
            //);
            ////rabbitAnimator.Play(reviveAnim);
            ///

            // Wait 1 frame so Animator updates to reviveAnim
            yield return null;

            // Read correct revive animation length
            float reviveLength = rabbitAnimator.GetCurrentAnimatorStateInfo(0).length;

            // Play revive animation fully (no loop)
            yield return new WaitForSeconds(reviveLength);
        }

        // Step 2: Show level complete popup (UI handles the animation)
        PillFill_LevelUI.Instance.ShowLevelComplete(currentLevelIndex + 1);

        // Step 3: Wait until popup animation ends
        yield return new WaitForSeconds(2.2f);

        // Step 4: Load next level
        //LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;

        if (nextIndex >= levels.Length)
        {
            Debug.Log("All levels finished!");

            rabbitAnimator.Play(rabbit_happyAnimation);

            return;
        }

        LoadLevel(nextIndex);
    }
}
