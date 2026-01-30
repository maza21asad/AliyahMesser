using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SpoonDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    // Inside SpoonDragHandler.cs
    private Animator animator;

// Create a public method that other scripts can call
    public void PlayPourAnimation()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("Pouring", -1, 0f); 
        }
    }
    
    [Header("Settings")]
    public string ingredientType; 

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public bool placed = false;
    
    public Animator spoonAnimator;
    public string spoonAnimationType;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    
    // We save this once at the start so we never "forget" the true position
    private int defaultSiblingIndex;

private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    
        // Cache the animator reference here
        animator = GetComponent<Animator>();
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        defaultSiblingIndex = transform.GetSiblingIndex();
        startPosition = rectTransform.position; 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1. MAKE BOWL SOLID (So we can drop into it)
        if (MixAFix_Manager.Instance != null)
        {
            MixAFix_Manager.Instance.SetBowlInteractable(true);
        }

        // ... (Standard Drag Logic) ...
        transform.DOKill();
        placed = false;
        
        // Save index just in case, though Start() handles the default
        originalSiblingIndex = transform.GetSiblingIndex(); 
        
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        transform.DOScale(originalScale * 1.15f, 0.2f).SetEase(Ease.OutBack);
    }
    
    // ADDED variable for restore logic
    private int originalSiblingIndex; 

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );
        rectTransform.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 2. MAKE BOWL GHOST AGAIN (So we can click other spoons behind it)
        if (MixAFix_Manager.Instance != null)
        {
            MixAFix_Manager.Instance.SetBowlInteractable(false);
        }

        if (!placed)
        {
            // Return logic
            rectTransform.DOMove(startPosition, 0.3f).SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    transform.SetSiblingIndex(defaultSiblingIndex);
                });

            transform.DOScale(originalScale, 0.2f);
        }
        else
        {
            transform.DOScale(originalScale, 0.2f);
        }
    }

    public void ResetVisuals()
    {
        transform.DOKill();
        transform.localScale = originalScale;
        transform.localRotation = Quaternion.identity; // Ensures it's not tilted on the shelf
        transform.SetSiblingIndex(defaultSiblingIndex); //

        if (animator != null)
        {
            animator.Play("Pouring", -1, 0f); //
            animator.Update(0); //
            animator.enabled = false; //
        }
    }
}