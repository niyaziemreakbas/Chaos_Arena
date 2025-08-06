using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public bool dragOnSurfaces = true;
    private CanvasGroup canvasGroup;

   // private GameObject m_DraggingIcon;
    private RectTransform draggingPlane;

    private Transform draggingArea;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            print("CardDragHandler: No CanvasGroup component found on the GameObject. Dragging will not work.");
        }
        //else
        //{
        //    m_CanvasGroup.blocksRaycasts = false; // Ensure raycast blocking is disabled
        //}
    }

    void Start()
    {
        draggingArea = GameObject.FindWithTag("OnDragCards").transform;

        if (draggingArea == null)
            print(draggingArea + " draggingArea is null. Make sure you have a GameObject with tag 'OnDragCards' in the scene.");

    }

    public void ReturnToOriginalSlot()
    {
        CardSlot currentSlot = GetComponent<CardController>().CurrentSlot;
        if (currentSlot != null)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero; // Reset position to slot's position
            currentSlot.SetCurrentCard(GetComponent<CardController>());
        }
        else
        {
            Debug.LogWarning("CardDragHandler: Current slot is null. Cannot return to original slot.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            print("CardDragHandler: No canvas found in parent hierarchy. Dragging will not work.");
            return;
        }

        canvasGroup.blocksRaycasts = false;

        draggingPlane = canvas.transform as RectTransform;

        //if (dragOnSurfaces)
        //    m_DraggingPlane = transform as RectTransform;
        //else
        //    m_DraggingPlane = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        transform.SetParent(draggingArea, true);
        SetDraggedPosition(data);
        //if (m_DraggingIcon != null)
            
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if(data == null)
           print("CardDragHandler: data or m_DraggingIcon is null. Cannot set dragged position.");
        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            draggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = draggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject target = eventData.pointerEnter;
       // Debug.Log("OnEndDrag: " + target?.name);

        if (target == null)
        {
            Debug.Log("Hiçbir þeyin üstüne býrakýlmadý.");
            ReturnToOriginalSlot();
            return;
        }

        // Parent zincirinde arýyoruz
        CardController targetCard = target.GetComponentInParent<CardController>();
        CardSlot targetSlot = target.GetComponentInParent<CardSlot>();

        if (targetCard != null)
        {
            Debug.Log("Kartýn üstüne býrakýldý: " + targetCard.name);
            CardSwapManager.Instance.SwapCards(GetComponent<CardController>(), targetCard);
        }
        else if (targetSlot != null)
        {
            Debug.Log("Slot'un üstüne býrakýldý: " + targetSlot.name);
            targetSlot.SetCurrentCard(GetComponent<CardController>());
        }
        else
        {
            Debug.Log("Ne kart ne slot bulundu, eski konuma dön.");
            ReturnToOriginalSlot();
        }
    }

}
