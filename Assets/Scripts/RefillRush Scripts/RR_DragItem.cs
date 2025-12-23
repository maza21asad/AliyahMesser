using UnityEngine;
using UnityEngine.EventSystems;

public class RR_DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemType;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private bool placed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        placed = false;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!placed)
            rectTransform.position = startPos;
    }

    public void MarkPlaced()
    {
        placed = true;
    }
}
