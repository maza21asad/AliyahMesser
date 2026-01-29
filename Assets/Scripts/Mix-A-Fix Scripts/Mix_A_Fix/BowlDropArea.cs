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
            bool wasAccepted = manager.DropScoops(spoon.ingredientType);

            if (wasAccepted)
            {
                spoon.placed = true;
                AnimateDropAndReturn(spoon);
            }
        }
    }

    // Inside BowlDropArea.cs
    private void AnimateDropAndReturn(SpoonDragHandler spoon)
    {
        RectTransform spoonRect = spoon.GetComponent<RectTransform>();
        Vector3 bowlCenter = transform.position;
    
        Sequence seq = DOTween.Sequence();

        // Step 1: Move to bowl
        seq.Append(spoonRect.DOMove(bowlCenter + new Vector3(1, 4, 0), 0.4f).SetEase(Ease.OutBack));
    
        // Step 2: Tell the spoon to play its animation
        seq.AppendCallback(() => {
            spoon.PlayPourAnimation(); 
        });
    
        // Step 3: Wait and return
        seq.AppendInterval(1.0f);
        seq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        seq.OnComplete(() => 
        {
            spoon.placed = false;
            spoon.ResetVisuals(); // This now also resets the animator
        });
    }
}