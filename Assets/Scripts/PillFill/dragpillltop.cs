using UnityEngine;
using UnityEngine.EventSystems;

public class PillDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Category (e.g. \"Pill\", \"Bandage\", \"Medicine\")")]
    public string itemCategory;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public bool placed = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Ensure a CanvasGroup exists so raycasts can be toggled
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
        placed = false; // reset when starting a new drag
        canvasGroup.blocksRaycasts = false; // allow drop areas to receive the drop
        // optionally bring to front:
        //canvasGroup.alpha = 1f;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!placed)
        {
            // not accepted by a valid drop area -> return to start
            ReturnToStartImmediate();
            Debug.Log("❌ Wrong place or not accepted -> returning.");
        }
        else
        {
            // already snapped by the drop area
            Debug.Log("✅ Placed correctly.");
            // optionally disable further dragging:
            // this.enabled = false;
        }
    }

    // Simple immediate return helper
    public void ReturnToStartImmediate()
    {
        rectTransform.position = startPosition;
    }

    // If you want smooth return, you can implement a coroutine-based lerp here.
}
