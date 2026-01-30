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
            // 1. Check with Manager
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
                // 2. WRONG INGREDIENT: Play Doll reaction and Spoon Error
                MixAFix_LevelUI.Instance.ShowFeedback(false);
            
                
                if (MixAFix_DollController.Instance != null)
                    MixAFix_DollController.Instance.PlaySad();
            }
        }
    }

    // Inside BowlDropArea.cs
    // Inside BowlDropArea.cs

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
            spoon.ResetVisuals(); // Resets animator/sprite

            // NEW: Now that the spoon is home, update the progress bar and check for win
            if (manager != null)
            {
                manager.ProcessAcceptedScoop();
            }
        });
    }
}