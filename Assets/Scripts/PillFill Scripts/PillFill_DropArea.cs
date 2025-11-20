//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PillFill_DropArea : MonoBehaviour, IDropHandler
//{
//    //public string acceptedCategory = "Pill";
//    //public int requiredCount = 4;
//    //private int collectedCount = 0;

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag == null) return;

//        PillFill_DragItem dropped = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
//        if (dropped == null) return;

//        if (dropped.itemCategory == "Pill")
//        {
//            dropped.placed = true;
//            dropped.GetComponent<RectTransform>().position = transform.position;

//            PillFill_LevelManager.Instance.RegisterPillCollected();
//        }
//        else
//        {
//            dropped.placed = false;
//        }
//    }
//}


//=============================

//using DG.Tweening;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PillFill_DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
//{
//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag == null) return;

//        PillFill_DragItem item = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
//        if (item != null)
//            item.OnHoverEnter();
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag == null) return;

//        PillFill_DragItem item = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
//        if (item != null)
//            item.OnHoverExit();
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (eventData.pointerDrag == null) return;

//        PillFill_DragItem dropped = eventData.pointerDrag.GetComponent<PillFill_DragItem>();
//        if (dropped == null) return;

//        if (dropped.itemCategory == "Pill")
//        {
//            dropped.placed = true;

//            // Smooth snap using DOTween
//            dropped.transform.DOMove(transform.position, 0.2f).SetEase(Ease.OutQuad);

//            // Count pill
//            PillFill_LevelManager.Instance.RegisterPillCollected();
//        }
//        else
//        {
//            dropped.placed = false;
//        }
//    }
//}

//============================

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

            // Stop dragging
            dropped.GetComponent<CanvasGroup>().blocksRaycasts = true;

            // 🔥 Animate into the box
            AnimatePillIntoBox(pillRect, boxRect);

            // Count it
            PillFill_LevelManager.Instance.RegisterPillCollected();
        }
        else
        {
            dropped.placed = false;
        }
    }

    private void AnimatePillIntoBox(RectTransform pill, RectTransform box)
    {
        //Vector3 targetPos = box.position;

        //Sequence seq = DOTween.Sequence();

        //seq.Append(pill.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad))
        //   .Join(pill.DOScale(0.7f, 0.3f))	// shrink a bit
        //   .Append(pill.DOScale(0f, 0.2f))  // disappear inside
        //   .OnComplete(() =>
        //   {
        //       pill.gameObject.SetActive(false);  // hide after entering
        //   });

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
