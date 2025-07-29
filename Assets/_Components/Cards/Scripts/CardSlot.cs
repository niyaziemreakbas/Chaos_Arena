using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    private CardData currentCard;

    public CardData CurrentCard => currentCard;

    private void Start()
    {
        SetChildrenToCurrentCard();
        print(CurrentCard.cardName);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Bir þey buraya býrakýldý: " + eventData.pointerDrag.name);

        var card = eventData.pointerDrag.GetComponent<CardDragHandler>();

        if (HasCard()) 
        {
            CardSwapManager.Instance.SwapCards(GetComponent<CardSlot>(), eventData.pointerDrag.GetComponent<CardDragHandler>().CurrentSlot);
        }

        if (card != null)
        {

            card.transform.SetParent(transform);
            card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void SetChildrenToCurrentCard()
    {
        SetCard(transform.GetComponentInChildren<Card>().CardData);
    }

    public void SetCard(CardData newCard)
    {
        currentCard = newCard;
    }

    public bool HasCard()
    {
        return GetComponentInChildren<Card>() != null;
    }


}
