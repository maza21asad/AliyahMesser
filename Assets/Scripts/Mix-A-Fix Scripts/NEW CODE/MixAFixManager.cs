using UnityEngine;

public class MixAFixManager : MonoBehaviour
{
    public RectTransform powderArea;
    public RectTransform bowlArea;

    [Header("Scoop Tracking")]
    public int scoopCount = 0;
    public int totalScoopsNeeded = 2;

    public bool IsOverPowder(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(powderArea, spoon.position);
    }

    public bool IsOverBowl(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bowlArea, spoon.position);
    }

    public void OnPowderCollected()
    {
        Debug.Log("💧 Powder collected — ready to drop!");
    }

    public void DropScoop()
    {
        scoopCount++;
        Debug.Log($"✅ Scoop dropped! Current scoops: {scoopCount}/{totalScoopsNeeded}");

        if (scoopCount >= totalScoopsNeeded)
        {
            Debug.Log("🎉 All scoops completed!");
        }
    }
}
