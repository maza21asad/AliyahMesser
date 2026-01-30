using UnityEngine;

public class MixAFix_DollController : MonoBehaviour
{
    public static MixAFix_DollController Instance;

    private Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    // Triggered when a correct ingredient is accepted
    public void PlaySuccess()
    {
        if (animator != null) animator.Play("Success", -1, 0f);
        Debug.Log("Success");
    }

    // Triggered when a wrong ingredient is dropped
    public void PlaySad()
    {
        if (animator != null) animator.Play("Sad", -1, 0f);
        Debug.Log("Sad");
    }

    // Triggered when the entire level is finished
    public void PlayHappyDance()
    {
        if (animator != null) animator.Play("HappyDance", -1, 0f);
        Debug.Log("HappyDance");
    }

    // Optionally call this to force return to Idle, 
    // though transitions with "Exit Time" in the Animator are better.
    public void PlayIdle()
    {
        if (animator != null) animator.Play("Idle");
        Debug.Log("Idle");
    }
}