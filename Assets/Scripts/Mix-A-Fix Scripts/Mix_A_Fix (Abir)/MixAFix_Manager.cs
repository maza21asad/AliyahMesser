using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MixAFix_Manager : MonoBehaviour
{
    [Header("Event Trigger")]
    public UnityEvent onDropToBowl; 

    [Header("Targets")]
    public RectTransform bowl; // Assign the Bowl UI here

    [Header("Scoop Counts")]
    public int powderScoopCount = 0;
    public int pinkCreamScoopCount = 0;
    public int yellowCreamScoopCount = 0;
    public int dropperScoopCount = 0;

    [Header("UI Feedback")]
    public int requiredPowderScoops = 2;
    public Image powderStepTick;

    private void Start()
    {
        if (powderStepTick) powderStepTick.gameObject.SetActive(false);
    }

    // Only one check needed now
    public bool IsOverBowl(RectTransform tool)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(bowl, tool.position);
    }

    public void DropScoops(string type)
    {
        // Trigger the generic event (Sound, Particle, etc.)
        if (onDropToBowl != null) onDropToBowl.Invoke();

        // Handle Ingredient Logic
        switch (type)
        {
            case "Powder":
                powderScoopCount++;
                Debug.Log($"� Powder Added! Total: {powderScoopCount}");
                
                // Check if step is complete
                if (powderScoopCount >= requiredPowderScoops && powderStepTick)
                {
                    powderStepTick.gameObject.SetActive(true);
                }
                break;

            case "YellowCream":
                yellowCreamScoopCount++;
                Debug.Log($"💛 Yellow Cream Added! Total: {yellowCreamScoopCount}");
                break;

            case "PinkCream":
                pinkCreamScoopCount++;
                Debug.Log($"🟥 Pink Cream Added! Total: {pinkCreamScoopCount}");
                break;
            
            case "Dropper":
                dropperScoopCount++;
                Debug.Log($"💧 Dropper Added! Total: {dropperScoopCount}");
                break;
        }
    }
}