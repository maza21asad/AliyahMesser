using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MixAFix_SliderProgress : MonoBehaviour
{
    [Header("References")]
    public Slider progressSlider;

    public void UpdateBar(float targetFill)
    {
        if (progressSlider == null) return;

        // Smoothly animate the slider value to the new percentage
        progressSlider.DOValue(targetFill, 0.5f).SetEase(Ease.OutQuad);
    }
}