//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PillFill_DropArea : MonoBehaviour, IDropHandler
//{
//    public string acceptedCategory = "Pill";
//    public int requiredCount = 4;
//    private int collectedCount = 0;

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag == null) return;

//        PillFill_DragItem dropped = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
//        if (dropped == null) return;

//        if (dropped.itemCategory == acceptedCategory)
//        {
//            dropped.GetComponent<RectTransform>().position = transform.position;
//            dropped.placed = true;

//            collectedCount++;

//            if (collectedCount >= requiredCount)
//                Debug.Log("All items collected!");
//        }
//        else
//        {
//            dropped.placed = false;
//        }
//    }
//}

//====================

using UnityEngine;
using UnityEngine.EventSystems;

public class PillFill_DropArea : MonoBehaviour, IDropHandler
{
    //public string acceptedCategory = "Pill";
    //public int requiredCount = 4;
    //private int collectedCount = 0;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        PillFill_DragItem dropped = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
        if (dropped == null) return;

        if (dropped.itemCategory == "Pill")
        {
            dropped.placed = true;
            dropped.GetComponent<RectTransform>().position = transform.position;

            PillFill_LevelManager.Instance.RegisterPillCollected();
        }
        else
        {
            dropped.placed = false;
        }
    }
}
