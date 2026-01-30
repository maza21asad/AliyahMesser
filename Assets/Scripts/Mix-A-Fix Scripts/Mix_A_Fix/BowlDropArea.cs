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
    
        // NEW: Check if we are already busy animating a previous spoon
        if (manager != null && !manager.CanAcceptScoop()) 
            return;

        SpoonDragHandler spoon = eventData.pointerDrag.GetComponent<SpoonDragHandler>();

    if (spoon != null && manager != null)
    {
        bool wasAccepted = manager.DropScoops(spoon.ingredientType);

        if (wasAccepted)
        {
            spoon.placed = true;
            AnimateDropAndReturn(spoon);
            if (MixAFix_DollController.Instance != null)
                MixAFix_DollController.Instance.PlaySuccess();
        }
        else
        {
            // 1. Mark as 'placed' so SpoonDragHandler.OnEndDrag doesn't snap it back home immediately
            spoon.placed = true; 
            
            MixAFix_LevelUI.Instance.ShowFeedback(false);
            
            if (MixAFix_DollController.Instance != null)
                MixAFix_DollController.Instance.PlaySad();

            // 2. Start the Shake-then-Return sequence
            AnimateErrorShake(spoon);
        }
    }
}

private void AnimateErrorShake(SpoonDragHandler spoon)
{
    RectTransform spoonRect = spoon.GetComponent<RectTransform>();
    Vector3 bowlCenter = transform.position;

    Sequence errorSeq = DOTween.Sequence();

    // Step 1: Move to the center of the bowl first
    //errorSeq.Append(spoonRect.DOMove(bowlCenter + new Vector3(0, 5, 0), 0.3f).SetEase(Ease.OutQuad));

    // Step 2: Perform the Shake (+15 to -15 degrees)
    errorSeq.Append(spoonRect.DORotate(new Vector3(0, 0, 15f), 0.05f));
    errorSeq.Append(spoonRect.DORotate(new Vector3(0, 0, -15f), 0.1f).SetLoops(4, LoopType.Yoyo));
    //errorSeq.Append(spoonRect.DORotate(Vector3.zero, 0.05f));

    // Step 3: Pause briefly so the player sees the "No" gesture
    //errorSeq.AppendInterval(0.2f);

    // Step 4: NOW return to the default location
    errorSeq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        errorSeq.OnComplete(() => 
        {
            // Reset the flag and visuals so the spoon can be dragged again
            spoon.placed = false; 
            spoon.ResetVisuals(); 
        });
    }

    private void AnimateDropAndReturn(SpoonDragHandler spoon)
    {
        RectTransform spoonRect = spoon.GetComponent<RectTransform>();
        Vector3 bowlCenter = transform.position;
    
        Sequence seq = DOTween.Sequence();

        // Step 1: Move to bowl
        seq.Append(spoonRect.DOMove(bowlCenter + new Vector3(1, 6, 0), 0.4f).SetEase(Ease.OutBack));
    
        // Step 2: Play animation
        seq.AppendCallback(() => {
            spoon.PlayPourAnimation(); 
        });
    
        // Step 3: Wait for pouring to finish
        seq.AppendInterval(1.0f);

        // Step 4: Return Home
        seq.Append(spoonRect.DOMove(spoon.startPosition, 0.5f).SetEase(Ease.InOutQuad));

        seq.OnComplete(() => 
        {
            spoon.placed = false;
            spoon.ResetVisuals(); 

            if (manager != null)
            {
                manager.ProcessAcceptedScoop();
                // UNLOCK the manager so the next spoon can be dropped
                manager.isProcessingScoop = false; 
            }
        });
    }
}