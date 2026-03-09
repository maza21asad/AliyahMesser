//using UnityEngine;

//public class MixAFix_DollController : MonoBehaviour
//{
//    public static MixAFix_DollController Instance;

//    private Animator animator;

//    private void Awake()
//    {
//        Instance = this;
//        animator = GetComponent<Animator>();
//    }

//    // Triggered when a correct ingredient is accepted
//    public void PlaySuccess()
//    {
//        //if (animator != null) animator.Play("Success", -1, 0f);
//        if (animator != null) animator.Play("Ele_Yes", -1, 0f);
//        //Debug.Log("Success");
//        Debug.Log("Ele_Yes");
//    }

//    // Triggered when a wrong ingredient is dropped
//    public void PlaySad()
//    {
//        //if (animator != null) animator.Play("Sad", -1, 0f);
//        if (animator != null) animator.Play("Ele_NO", -1, 0f);
//        //Debug.Log("Sad");
//        Debug.Log("Ele_NO");
//    }

//    // Triggered when the entire level is finished
//    public void PlayHappyDance()
//    {
//        if (animator != null) animator.Play("HappyDance", -1, 0f);
//        Debug.Log("HappyDance");
//    }

//    // Optionally call this to force return to Idle, 
//    // though transitions with "Exit Time" in the Animator are better.
//    public void PlayIdle()
//    {
//        //if (animator != null) animator.Play("Idle");
//        if (animator != null) animator.Play("Ele_Idle");
//        //Debug.Log("Idle");
//        Debug.Log("Ele_Idle");
//    }
//}


using UnityEngine;
using System.Collections;

public class MixAFix_DollController : MonoBehaviour
{
    public static MixAFix_DollController Instance;

    private Animator animator;

    [Header("Animation Names")]
    public string idleAnimation = "Ele_Idle";
    public string successAnimation = "Ele_Yes";
    public string sadAnimation = "Ele_No";
    public string happyDanceAnimation = "Ele_Yes";

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    // Triggered when a correct ingredient is accepted
    public void PlaySuccess()
    {
        if (animator != null)
            StartCoroutine(PlayThenIdle(successAnimation));
    }

    // Triggered when a wrong ingredient is dropped
    public void PlaySad()
    {
        if (animator != null)
            StartCoroutine(PlayThenIdle(sadAnimation));
    }

    // Triggered when the entire level is finished
    public void PlayHappyDance()
    {
        if (animator != null)
            //animator.Play(happyDanceAnimation, -1, 0f);
            animator.Play(happyDanceAnimation);
    }

    public void PlayIdle()
    {
        if (animator != null)
            animator.Play(idleAnimation);
    }

    private IEnumerator PlayThenIdle(string animName)
    {
        //animator.Play(animName, -1, 0f);
        animator.Play(animName);

        yield return null; // wait 1 frame so Animator updates

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        animator.Play(idleAnimation);
    }
}