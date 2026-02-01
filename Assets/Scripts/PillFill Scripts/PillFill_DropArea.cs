using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;   // <<--- IMPORTANT

public class PillFill_DropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        PillFill_DragItem dropped = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
        if (dropped == null) return;

        if (dropped.itemCategory == "Pill")
        {
            dropped.placed = true;

            RectTransform pillRect = dropped.GetComponent<RectTransform>();
            RectTransform boxRect = GetComponent<RectTransform>();

            AnimatePillIntoBox(pillRect, boxRect);

            PillFill_LevelUI.Instance.ShowFeedback(true);   // <-- GOOD
            PillFill_LevelManager.Instance.RegisterPillCollected();
            PillFill_LevelManager.Instance.PlayCowYes();
        }
        else
        {
            PillFill_LevelUI.Instance.ShowFeedback(false);  // <-- BAD
            dropped.placed = false;
            PillFill_LevelManager.Instance.PlayCowNo();

            SoundManager.instance.PlaySFX("NoSound");
        }

    }

    private void AnimatePillIntoBox(RectTransform pill, RectTransform box)
    {
        Vector3 boxPos = box.position;

        // Start above the box by some pixels
        Vector3 abovePos = boxPos + new Vector3(0, 4f, 0);

        Sequence seq = DOTween.Sequence();

        seq
            // Move up first (as if preparing to drop)
            .Append(pill.DOMove(abovePos, 0.5f).SetEase(Ease.OutQuad))

            // Then drop down into the box
            .Append(pill.DOMove(boxPos, 0.75f).SetEase(Ease.InQuad))

            // Scale down INSIDE the box (disappear)
            .Append(pill.DOScale(0f, 0.5f).SetEase(Ease.InBack))

            .OnComplete(() =>
            {
                pill.gameObject.SetActive(false);
            });
    }
}
