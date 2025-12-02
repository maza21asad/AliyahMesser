using UnityEngine;
using UnityEngine.EventSystems;

public class PillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject glow;  // assign in Inspector
    public RectTransform targetBox; // where the pill must be dropped
    public float dropRange = 100f; // distance tolerance

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        glow.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        glow.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        glow.SetActive(false);

        if (Vector2.Distance(transform.position, targetBox.position) < dropRange)
        {
            // Correct drop
            PillFill_LevelUI.Instance.ShowFeedback(true); // GOOD! NICELY DONE!
            PillFill_LevelManager.Instance.RegisterPillCollected();

            Destroy(gameObject); // remove pill after collecting
        }
        else
        {
            // wrong → return pill
            transform.position = startPos;
        }
    }
}
