using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RR_DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemType;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private bool placed;

    // Visual feedback
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalScale = transform.localScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        placed = false;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        transform.DOScale(originalScale * 1.12f, 0.18f)
                 .SetEase(Ease.OutBack);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.position = eventData.position;

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
        //canvasGroup.blocksRaycasts = true;

        //if (!placed)
        //    rectTransform.position = startPos;

        canvasGroup.blocksRaycasts = true;

        if (!placed)
        {
            // Smooth return
            rectTransform.DOMove(startPos, 0.25f)
                         .SetEase(Ease.OutQuad);

            transform.DOScale(originalScale, 0.15f);
        }
        else
        {
            // Small success pop
            transform.DOScale(originalScale * 1.2f, 0.15f)
                     .SetEase(Ease.OutBack)
                     .OnComplete(() =>
                     {
                         transform.DOScale(originalScale, 0.15f);
                     });
        }
    }

    public void MarkPlaced()
    {
        placed = true;
        canvasGroup.blocksRaycasts = false;
    }
}

