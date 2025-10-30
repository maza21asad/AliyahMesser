using UnityEngine;
using UnityEngine.EventSystems;

public class PillBoxDropArea : MonoBehaviour, IDropHandler
{
    [Header("What this box accepts (match itemCategory string exactly)")]
    public string acceptedCategory = "Pill";

    [Header("Collection")]
    public int requiredCount = 4;
    private int collectedCount = 0;

    // NOTE: here we use instance logic (no static flags)
    public void OnDrop(PointerEventData eventData)
    {
        // pointerDrag is the GameObject being dragged
        if (eventData.pointerDrag == null) return;

        PillDragItem dropped = eventData.pointerDrag.GetComponent<PillDragItem>();
        if (dropped == null) return;

        // check for exact match (case-sensitive). If you want case-insensitive:
        // if (string.Equals(dropped.itemCategory, acceptedCategory, StringComparison.OrdinalIgnoreCase))
        if (dropped.itemCategory == acceptedCategory)
        {
            // Accept it: snap to box and mark as placed
            dropped.GetComponent<RectTransform>().position = transform.position;
            dropped.placed = true;

            collectedCount++;
            Debug.Log($"✅ Collected {collectedCount}/{requiredCount} {acceptedCategory}(s).");

            if (collectedCount >= requiredCount)
            {
                OnAllCollected();
            }
        }
        else
        {
            // Not accepted: leave placed=false so item returns on OnEndDrag
            dropped.placed = false;
            Debug.Log($"❌ Wrong item. This box accepts: {acceptedCategory}. Dropped was: {dropped.itemCategory}");
        }
    }

    private void OnAllCollected()
    {
        Debug.Log("🎉 Required pills collected! Win condition reached.");
        // Add your win logic here (score, next level, etc.)
    }
}

