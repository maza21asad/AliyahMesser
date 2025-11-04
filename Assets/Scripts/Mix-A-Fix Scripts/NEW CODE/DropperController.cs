using UnityEngine;
using UnityEngine.EventSystems;

public class DropperController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DropperManager manager;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform originalParent;
    private int originalSiblingIndex;

    private bool isHoveringBottle = false;
    private float hoverTimer = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        isHoveringBottle = false;
        hoverTimer = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, true);
        transform.SetSiblingIndex(originalSiblingIndex);

        if (manager == null) return;

        if (manager.IsOverBottle(rectTransform))
        {
            Debug.Log("🧴 Dropper placed on bottle — collecting drop...");
            isHoveringBottle = true;
            hoverTimer = 0f;
        }
        else if (manager.IsOverBowl(rectTransform))
        {
            if (manager.HasDrop())
            {
                manager.DropIntoBowl();
            }
            else
            {
                Debug.Log("⚠️ Dropper is empty!");
            }
        }
        else
        {
            Debug.Log("💧 Dropper released elsewhere — stays where it is.");
        }
    }

    private void Update()
    {
        if (isHoveringBottle)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= 1.5f)
            {
                isHoveringBottle = false;
                manager.OnDropCollected();
            }
        }
    }
}
