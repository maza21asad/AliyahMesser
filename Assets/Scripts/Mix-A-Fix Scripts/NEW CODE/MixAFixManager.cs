using UnityEngine;
using UnityEngine.UI;

public class MixAFixManager : MonoBehaviour
{
    [Header("Scoop Targets")]
    public RectTransform powder;
    public RectTransform pinkCream;
    public RectTransform yellowCream;
    public RectTransform bowl;

    [Header("Scoop Counts")]
    public int powderScoopCount = 0;
    public int pinkCreamScoopCount = 0;
    public int yellowCreamScoopCount = 0;

    [Header("UI Step Checkmarks")]
    public Image powderStepTick;
    public Image yellowCreamStepTick;

    [Header("Target Scoop Requirements")]
    public int requiredPowderScoops = 2;
    public int requiredYellowCreamScoops = 2;

    private void Start()
    {
        // Make sure tick marks are hidden initially
        if (powderStepTick) powderStepTick.gameObject.SetActive(false);
        if (yellowCreamStepTick) yellowCreamStepTick.gameObject.SetActive(false);
    }

    public bool IsOverPowder(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(powder, spoon.position);
    }

    public bool IsOverPinkCream(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(pinkCream, spoon.position);
    }

    public bool IsOverYellowCream(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(yellowCream, spoon.position);
    }

    public bool IsOverBowl(RectTransform spoon)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bowl, spoon.position);
    }

    public void OnScoopCollected(string type)
    {
        Debug.Log($"🧂 {type} scoop collected and ready to drop!");
    }

    public void DropScoop(string type)
    {
        switch (type)
        {
            case "Powder":
                powderScoopCount++;
                Debug.Log($"✅ Powder dropped! Total powder scoops: {powderScoopCount}");

                if (powderScoopCount >= requiredPowderScoops)
                {
                    if (powderStepTick && !powderStepTick.gameObject.activeSelf)
                    {
                        powderStepTick.gameObject.SetActive(true);
                        Debug.Log("🎯 Step 1 Complete: Powder scoops done!");
                    }
                }
                break;

            case "YellowCream":
                yellowCreamScoopCount++;
                Debug.Log($"✅ Yellow Cream dropped! Total yellow cream scoops: {yellowCreamScoopCount}");

                if (yellowCreamScoopCount >= requiredYellowCreamScoops)
                {
                    if (yellowCreamStepTick && !yellowCreamStepTick.gameObject.activeSelf)
                    {
                        yellowCreamStepTick.gameObject.SetActive(true);
                        Debug.Log("🎯 Step 2 Complete: Yellow cream scoops done!");
                    }
                }
                break;

            case "PinkCream":
                pinkCreamScoopCount++;
                Debug.Log($"✅ Pink Cream dropped! Total pink cream scoops: {pinkCreamScoopCount}");
                break;
        }
    }
}
