using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemType; // e.g. "Medicine", "Bandage", "AidKit"
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false; // allow raycast to detect slots below
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!DropSlot.correctPlacement)
        {
            // Wrong slot: return to start position
            rectTransform.position = startPosition;
            Debug.Log("❌ Wrong place!");
        }
        else
        {
            Debug.Log("✅ Correct!");
        }

        DropSlot.correctPlacement = false; // reset after each drag
    }
}
