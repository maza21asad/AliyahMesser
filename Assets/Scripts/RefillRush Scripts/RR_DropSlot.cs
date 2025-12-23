using UnityEngine;
using UnityEngine.EventSystems;

public class RR_DropSlot : MonoBehaviour, IDropHandler
{
    public string acceptedType;
    private bool isFilled = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (isFilled) return;

        RR_DragItem item = eventData.pointerDrag.GetComponent<RR_DragItem>();
        if (item == null) return;

        if (item.itemType == acceptedType)
        {
            item.GetComponent<RectTransform>().position = transform.position;
            item.MarkPlaced();

            isFilled = true;

            RR_LevelUI.Instance.ShowFeedback(true);
            RR_LevelManager.Instance.RegisterCorrectPlacement();
        }
        else
        {
            RR_LevelUI.Instance.ShowFeedback(false);
        }
    }
}
