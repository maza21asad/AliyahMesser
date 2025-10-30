using UnityEngine;
using UnityEngine.EventSystems;

public class PillDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Type of Item (Pill / Bandage / Medicine)")]
    public string itemCategory;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!PillBoxDropArea.isPillAccepted)
        {
            rectTransform.position = initialPosition;
            Debug.Log("❌ Wrong item placed!");
        }
        else
        {
            Debug.Log("✅ Correct pill dropped!");
        }

        PillBoxDropArea.isPillAccepted = false;
    }
}

