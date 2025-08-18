using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    private CardController currentCard;

    public CardController CurrentCard => currentCard;

    private void Start()
    {
        SetChildrenToCurrentCard();
        //print(CurrentCard.CardData.cardName);
    }

    void SetChildrenToCurrentCard()
    {
        SetCurrentCard(transform.GetComponentInChildren<CardController>());
    }

    //Need an animation
    public void SetCurrentCard(CardController newCard)
    {
        //print("Setting current card: " + newCard.CardData.cardName + " to " + this.name);
        currentCard = newCard;
        newCard.transform.SetParent(transform);
        newCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        newCard.SetCurrentSlot(this);
    }

    public bool HasCard()
    {
        return currentCard != null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
