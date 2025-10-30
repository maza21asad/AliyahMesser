using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public Text feedbackText;
    public Animator elephantAnimator; // optional
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip completeSound;

    public void ShowCorrect()
    {
        feedbackText.text = "Good job!";
        audioSource.PlayOneShot(correctSound);
        elephantAnimator?.SetTrigger("Happy");
    }

    public void ShowWrong()
    {
        feedbackText.text = "Oops! Try again!";
        audioSource.PlayOneShot(wrongSound);
        elephantAnimator?.SetTrigger("Sad");
    }

    public void ShowPartial(int left)
    {
        feedbackText.text = $"Keep going! {left} left!";
    }

    public void ShowGameComplete()
    {
        feedbackText.text = "🎉 All Done!";
        audioSource.PlayOneShot(completeSound);
        elephantAnimator?.SetTrigger("Celebrate");
    }
}
