using UnityEngine;
using UnityEngine.EventSystems;

public class PillBoxDropArea : MonoBehaviour, IDropHandler
{
    public static bool isPillAccepted = false;
    public int totalPillsNeeded = 4;
    private int pillsCollected = 0;

    public void OnDrop(PointerEventData eventData)
    {
        PillDragItem droppedItem = eventData.pointerDrag.GetComponent<PillDragItem>();

        if (droppedItem != null)
        {
            if (droppedItem.itemCategory == "Pill")
            {
                droppedItem.GetComponent<RectTransform>().position = transform.position;
                isPillAccepted = true;

                pillsCollected++;
                Debug.Log($"✅ Pill collected: {pillsCollected}/{totalPillsNeeded}");

                if (pillsCollected >= totalPillsNeeded)
                {
                    OnAllPillsCollected();
                }
            }
            else
            {
                isPillAccepted = false;
                Debug.Log("❌ That’s not a pill!");
            }
        }
    }

    private void OnAllPillsCollected()
    {
        Debug.Log("🎉 All pills collected for Bunny!");
    }
}
