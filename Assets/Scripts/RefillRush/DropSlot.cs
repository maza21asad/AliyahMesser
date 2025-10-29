using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string acceptedType; // e.g. "Medicine", "Bandage", "AidKit"
    public static bool correctPlacement = false;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggedItem != null)
        {
            if (draggedItem.itemType == acceptedType)
            {
                // Correct match
                draggedItem.GetComponent<RectTransform>().position = transform.position;
                correctPlacement = true;
            }
            else
            {
                correctPlacement = false;
            }
        }
    }
}
