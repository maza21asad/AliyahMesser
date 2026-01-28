using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SpoonDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public string ingredientType; 

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public bool placed = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    
    // We save this once at the start so we never "forget" the true position
    private int defaultSiblingIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        // This permanently remembers where the spoon belongs in the hierarchy.
        defaultSiblingIndex = transform.GetSiblingIndex();
        
        // Also capture start position here to be safe
        startPosition = rectTransform.position; 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Stop any active animations immediately.
        // This prevents the "Return" animation from fighting your new Drag.
        transform.DOKill();

        placed = false;
        canvasGroup.blocksRaycasts = false;
        
        // Bring to front
        transform.SetAsLastSibling();

        transform.DOScale(originalScale * 1.15f, 0.2f).SetEase(Ease.OutBack);
    }

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
            // Return to start
            rectTransform.DOMove(startPosition, 0.3f).SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    // Restore to the SAFETY index captured in Start()
                    transform.SetSiblingIndex(defaultSiblingIndex);
                });

            transform.DOScale(originalScale, 0.2f);
        }
        else
        {
            transform.DOScale(originalScale, 0.2f);
        }
    }
    
    // Called by the BowlDropArea when it's done pouring
    public void ResetVisuals()
    {
        transform.DOKill(); // Stop any leftover animations
        transform.localScale = originalScale;
        transform.SetSiblingIndex(defaultSiblingIndex); // Always go back to correct layer
    }
}