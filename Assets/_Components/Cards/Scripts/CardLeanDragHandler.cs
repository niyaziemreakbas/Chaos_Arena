using Lean.Common;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(LeanSelectable))]
public class CardLeanDragHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;

    [Header("Drag Parent Area")]
    public Transform draggingArea; // sürüklerken taþýnacaðý alan (örn. sahnede boþ UI)

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        if (draggingArea == null)
        {
            GameObject areaObj = GameObject.FindWithTag("OnDragCards");
            if (areaObj != null)
                draggingArea = areaObj.transform;
            else
                Debug.LogError("CardLeanDragHandler: draggingArea atanmadý ve sahnede OnDragCards tag'li bir obje bulunamadý.");
        }

        var selectable = GetComponent<LeanSelectable>();
        //selectable.OnSelect.AddListener(OnSelect);
        //selectable.OnDeselect.AddListener(OnDeselect);
    }

    void OnSelect()
    {
        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(draggingArea, true);
        canvasGroup.blocksRaycasts = false;
    }

    void OnDeselect()
    {
        ReturnToOriginalSlot();
    }

    public void ReturnToOriginalSlot()
    {
        transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalAnchoredPosition;
        canvasGroup.blocksRaycasts = true;
    }
}
