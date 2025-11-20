//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PillFill_DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
//{
//    public string itemCategory;

//    [HideInInspector] public Vector3 startPosition;
//    [HideInInspector] public bool placed = false;

//    private RectTransform rectTransform;
//    private CanvasGroup canvasGroup;

//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        startPosition = rectTransform.position;
//        placed = false;
//        canvasGroup.blocksRaycasts = false;
//        transform.SetAsLastSibling();
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        Vector2 localPos;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            rectTransform.parent as RectTransform,
//            eventData.position,
//            eventData.pressEventCamera,
//            out localPos
//        );
//        rectTransform.localPosition = localPos;
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        canvasGroup.blocksRaycasts = true;

//        if (!placed)
//            rectTransform.position = startPosition;
//    }
//}


using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PillFill_DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemCategory;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public bool placed = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Visual feedback
    private Vector3 originalScale;
    private Tween hoverTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

        originalScale = transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        placed = false;

        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        // Small pickup scale animation
        transform.DOScale(originalScale * 1.12f, 0.18f).SetEase(Ease.OutBack);
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
            // Not placed → return smoothly
            rectTransform.DOMove(startPosition, 0.25f).SetEase(Ease.OutQuad);
            transform.DOScale(originalScale, 0.15f);
        }
        else
        {
            // Successfully placed → small confirmation pop
            transform.DOScale(originalScale * 1.2f, 0.15f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        transform.DOScale(originalScale, 0.15f);
                    });
        }
    }

    // --- Called from your DropBox script when entered and exited ----
    public void OnHoverEnter()
    {
        // Glow effect or scale effect
        if (hoverTween != null) hoverTween.Kill();

        hoverTween = transform.DOScale(originalScale * 1.15f, 0.2f)
                              .SetEase(Ease.OutBack);
    }

    public void OnHoverExit()
    {
        if (hoverTween != null) hoverTween.Kill();

        hoverTween = transform.DOScale(originalScale, 0.2f)
                              .SetEase(Ease.OutQuad);
    }
}
