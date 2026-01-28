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
        
        // Pause
        seq.AppendInterval(0.3f);

        // Return Home
        seq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        seq.OnComplete(() => 
        {
            spoon.placed = false;
            spoon.ResetVisuals();
        });
    }
}