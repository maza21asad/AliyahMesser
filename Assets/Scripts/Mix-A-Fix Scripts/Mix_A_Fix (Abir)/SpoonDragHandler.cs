using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SpoonDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public string ingredientType; 

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public bool placed = false;
    
    private int originalSiblingIndex;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // FIX 1: Auto-add CanvasGroup if missing (Like PillFill)
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        
        originalScale = transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        placed = false;
        
        originalSiblingIndex = transform.GetSiblingIndex();
        
        canvasGroup.blocksRaycasts = false;
        
        // Bring to front so it renders over everything else
        transform.SetAsLastSibling();

        // "Juice": Scale up slightly
        transform.DOScale(originalScale * 1.15f, 0.2f).SetEase(Ease.OutBack);
    }

    // Drag Logic
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );

        rectTransform.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!placed)
        {
            // Reset position
            rectTransform.DOMove(startPosition, 0.3f).SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    // Restore the original layer order after animation finishes
                    transform.SetSiblingIndex(originalSiblingIndex);
                });

            transform.DOScale(originalScale, 0.2f);
        }
        else
        {
            // Reset scale if placed successfully
            transform.DOScale(originalScale, 0.2f);
        }
    }
    
    public void ResetVisuals()
    {
        transform.localScale = originalScale;
        // Ensure it goes back to correct layer when returning from bowl
        transform.SetSiblingIndex(originalSiblingIndex);
    }
}