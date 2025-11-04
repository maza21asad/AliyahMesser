using UnityEngine;

public class DropperManager : MonoBehaviour
{
    public RectTransform bottle;
    public RectTransform bowl;

    public int dropCount = 0;
    private bool isHoldingDrop = false;

    public bool IsOverBottle(RectTransform dropper)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bottle, dropper.position);
    }

    public bool IsOverBowl(RectTransform dropper)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bowl, dropper.position);
    }

    public void OnDropCollected()
    {
        isHoldingDrop = true;
        Debug.Log("💧 Drop collected from bottle!");
    }

    public void DropIntoBowl()
    {
        if (isHoldingDrop)
        {
            dropCount++;
            Debug.Log($"✅ Drop released into bowl! Total drops: {dropCount}");
            isHoldingDrop = false;
        }
        else
        {
            Debug.Log("⚠️ Dropper empty — nothing to drop!");
        }
    }

    public bool HasDrop() => isHoldingDrop;
}
