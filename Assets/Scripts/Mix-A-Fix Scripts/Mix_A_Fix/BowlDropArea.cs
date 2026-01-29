using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BowlDropArea : MonoBehaviour, IDropHandler
{
    [Header("Manager Connection")]
    public MixAFix_Manager manager;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        
        SpoonDragHandler spoon = eventData.pointerDrag.GetComponent<SpoonDragHandler>();
        
        if (spoon != null && manager != null)
        {
            // 1. ASK THE MANAGER: Is this ingredient correct?
            bool wasAccepted = manager.DropScoops(spoon.ingredientType);

            if (wasAccepted)
            {
                // SUCCESS: Mark as placed so DragHandler doesn't interfere
                spoon.placed = true;
                Debug.Log($"🥣 {spoon.ingredientType} accepted. Playing animation.");
                
                // Play the fancy "Pour" animation
                AnimateDropAndReturn(spoon);
            }
            else
            {
                // FAIL: Do NOT set spoon.placed = true.
                // The SpoonDragHandler's OnEndDrag will see that 'placed' is false.
                // It will automatically fly the spoon back to start (Rejection behavior).
                Debug.Log($"❌ {spoon.ingredientType} rejected. Returning immediately.");
            }
        }
    }

    private void AnimateDropAndReturn(SpoonDragHandler spoon)
    {
        // 1. Get the Animator component from the spoon
        Animator spoonAnimator = spoon.GetComponent<Animator>();
        RectTransform spoonRect = spoon.GetComponent<RectTransform>();
        Vector3 bowlCenter = transform.position;

        // 2. Start a Sequence for the movement logic
        Sequence seq = DOTween.Sequence();

        // Step A: Move to the center of the bowl (Pour Position)
        seq.Append(spoonRect.DOMove(bowlCenter + new Vector3(1, 4, 0), 0.4f).SetEase(Ease.OutBack));

        // Step B: Trigger the "Pouring" animation from your screenshot
        seq.AppendCallback(() => 
        {
            if (spoonAnimator != null)
            {
                spoonAnimator.Play("Pouring");
            }
        });

        // Step C: Wait for the animation to play (Adjust time based on your clip length)
        seq.AppendInterval(1.0f); 

        // Step D: Return Home
        seq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        seq.OnComplete(() => 
        {
            spoon.placed = false;
            spoon.ResetVisuals();
        });
    }
}