using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public bool dragOnSurfaces = true;
    private CanvasGroup canvasGroup;

    CardSlot currentSlot;
    public CardSlot CurrentSlot => currentSlot;

    Transform startParent;
    // public getters of StartPArent

   // private GameObject m_DraggingIcon;
    private RectTransform draggingPlane;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        currentSlot = GetComponentInParent<CardSlot>();

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

        if (transform.parent == startParent)
        {
            transform.SetParent(startParent);
            currentSlot = GetComponentInParent<CardSlot>();
            transform.localPosition = Vector3.zero;
            print("CardDragHandler: Card returned to its original parent.");
        }

        //if (m_DraggingIcon != null)
        //    Destroy(m_DraggingIcon);
    }

}
