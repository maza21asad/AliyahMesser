using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BowlDropArea : MonoBehaviour, IDropHandler
{
    [Header("Manager Connection")]
    public MixAFix_Manager manager; // Drag your Manager here

    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dropped item is a Spoon
        if (eventData.pointerDrag == null) return;
        
        SpoonDragHandler spoon = eventData.pointerDrag.GetComponent<SpoonDragHandler>();
        
        if (spoon != null)
        {
            // Mark as placed so the Spoon script doesn't snap back immediately
            spoon.placed = true;

            // Game Logic
            if (manager != null)
            {
                Debug.Log($"🥣 {spoon.ingredientType} added to bowl!");
                manager.DropScoops(spoon.ingredientType);
            }

            // Play the Animation Sequence
            AnimateDropAndReturn(spoon);
        }
    }

    private void AnimateDropAndReturn(SpoonDragHandler spoon)
    {
        RectTransform spoonRect = spoon.GetComponent<RectTransform>();
        Vector3 bowlCenter = transform.position;
        
        // Create a Sequence (Like PillFill_DropArea)
        Sequence seq = DOTween.Sequence();

        // Step A: Move to the center of the bowl (Pour Position)
        seq.Append(spoonRect.DOMove(bowlCenter + new Vector3(0, 5, 0), 0.4f).SetEase(Ease.OutBack));
        
        // @DOTO::
        // "Pour" animation (Scale down slightly or rotate)
        ///////////   Animation will Here   //////////////
        //////////////////////////////////////////////////
        
        // Step C: Wait a moment for the player to see it
        seq.AppendInterval(0.3f);

        // Move BACK to the original Spoon holder
        seq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        // Step E: Reset flag so it can be dragged again
        seq.OnComplete(() => 
        {
            spoon.placed = false;
            spoon.ResetVisuals();
        });
    }
}