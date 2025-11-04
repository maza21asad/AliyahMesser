using UnityEngine;

public class MixAFixManager : MonoBehaviour
{
    public RectTransform powder;
    public RectTransform pinkCream;
    public RectTransform yellowCream;
    public RectTransform bowl;

    public int powderScoopCount = 0;
    public int pinkCreamScoopCount = 0;
    public int yellowCreamScoopCount = 0;

    private RectTransform lastTarget;

    // Check if spoon is over powder or cream or bowl
    public bool IsOverPowder(RectTransform spoon)
    {
        lastTarget = powder;
        return RectTransformUtility.RectangleContainsScreenPoint(powder, spoon.position);
    }

    public bool IsOverPinkCream(RectTransform spoon)
    {
        lastTarget = pinkCream;
        return RectTransformUtility.RectangleContainsScreenPoint(pinkCream, spoon.position);
    }

    public bool IsOverYellowCream(RectTransform spoon)
    {
        lastTarget = yellowCream;
        return RectTransformUtility.RectangleContainsScreenPoint(yellowCream, spoon.position);
    }

    public bool IsOverBowl(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bowl, spoon.position);
    }

    // Called when any scoop is collected
    public void OnScoopCollected(string type)
    {
        Debug.Log($"🧂 {type} scoop collected and ready to drop!");
    }

    // Called when a scoop is dropped
    public void DropScoop(string type)
    {
        switch (type)
        {
            case "Powder":
                powderScoopCount++;
                Debug.Log($"✅ Powder dropped! Total powder scoops: {powderScoopCount}");
                break;
            case "PinkCream":
                pinkCreamScoopCount++;
                Debug.Log($"✅ Pink Cream dropped! Total pink cream scoops: {pinkCreamScoopCount}");
                break;
            case "YellowCream":
                yellowCreamScoopCount++;
                Debug.Log($"✅ Yellow Cream dropped! Total yellow cream scoops: {yellowCreamScoopCount}");
                break;
        }
    }
}

