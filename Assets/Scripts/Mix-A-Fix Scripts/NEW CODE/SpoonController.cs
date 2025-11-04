using UnityEngine;
using UnityEngine.EventSystems;

public class SpoonController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MixAFixManager manager;
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform originalParent;
    private int originalSiblingIndex;

    private bool isHoldingScoop = false;
    private string heldType = "";
    private bool isHovering = false;
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

        isHovering = false;
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

        // Check if released on any ingredient
        if (manager.IsOverPowder(rectTransform))
        {
            Debug.Log("🧂 Released on Powder");
            StartHover("Powder");
        }
        else if (manager.IsOverPinkCream(rectTransform))
        {
            Debug.Log("🍓 Released on Pink Cream");
            StartHover("PinkCream");
        }
        else if (manager.IsOverYellowCream(rectTransform))
        {
            Debug.Log("💛 Released on Yellow Cream");
            StartHover("YellowCream");
        }
        else if (manager.IsOverBowl(rectTransform))
        {
            if (isHoldingScoop)
            {
                Debug.Log($"🥣 Dropped {heldType} into bowl!");
                manager.DropScoop(heldType);
                isHoldingScoop = false;
                heldType = "";
            }
            else
            {
                Debug.Log("⚠️ Spoon empty, nothing to drop!");
            }
        }
        else
        {
            Debug.Log("🪄 Released elsewhere — spoon stays put.");
        }
    }

    private void StartHover(string type)
    {
        isHovering = true;
        hoverTimer = 0f;
        heldType = type;
    }

    private void Update()
    {
        if (isHovering)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= 1.5f)
            {
                isHovering = false;
                isHoldingScoop = true;
                manager.OnScoopCollected(heldType);
                Debug.Log($"✅ {heldType} scoop taken!");
            }
        }
    }
}
