using UnityEngine;
using UnityEngine.EventSystems;

public class SpoonController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MixAFixManager manager;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform originalParent;
    private int originalSiblingIndex;

    private bool isHoldingPowder = false;
    private bool isHoveringOverPowder = false;
    private float hoverTimer = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Remember where the spoon was before dragging
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Move spoon to the top of the canvas so it renders above everything
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        isHoveringOverPowder = false;
        hoverTimer = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Move it back to its original parent so hierarchy stays clean
        transform.SetParent(originalParent, true);
        transform.SetSiblingIndex(originalSiblingIndex);

        if (manager == null) return;

        // Check drop locations
        if (manager.IsOverPowder(rectTransform))
        {
            Debug.Log("🧂 Spoon released on powder...");
            isHoveringOverPowder = true;
            hoverTimer = 0f;
        }
        else if (manager.IsOverBowl(rectTransform))
        {
            if (isHoldingPowder)
            {
                Debug.Log("🥣 Scoop dropped into bowl!");
                manager.DropScoop();
                isHoldingPowder = false;
            }
            else
            {
                Debug.Log("⚠️ Spoon is empty! Nothing to drop.");
            }
        }
        else
        {
            Debug.Log("🪄 Spoon released outside powder/bowl — staying in place.");
        }
    }

    private void Update()
    {
        // Handle powder pickup delay
        if (isHoveringOverPowder)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= 1.5f)
            {
                isHoveringOverPowder = false;
                isHoldingPowder = true;
                manager.OnPowderCollected();
                Debug.Log("✅ Scoop taken!");
            }
        }
    }
}
