using UnityEngine;
using UnityEngine.EventSystems;

public class MixingBowlUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        IngredientUI ingredient = eventData.pointerDrag.GetComponent<IngredientUI>();

        if (ingredient != null)
        {
            InstructionManager.Instance.IngredientAdded(ingredient.ingredientType);
        }
    }
}
