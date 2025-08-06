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

    //public void OnDrop(PointerEventData eventData)
    //{

    //    Card droppedCard = eventData.pointerDrag.GetComponent<Card>();

    //    Debug.Log("Bir þey buraya býrakýldý: " + droppedCard.CardData.cardName);


    //    if (HasCard()) 
    //    {
    //        print($"CardSlot: {currentCard.CardData.cardName} Has current card, swapping with dropped card.");
    //        if (droppedCard == null)
    //        {
    //            Debug.LogWarning("Dropped  Card. null");
    //            return;
    //        }
    //        if(currentCard == null)
    //        {
    //            Debug.LogWarning("current card null.");
    //            return;
    //        }
    //        CardSwapManager.Instance.SwapCards(currentCard, droppedCard);
    //    }
    //    else if (droppedCard != null)
    //    {
    //        SetCurrentCard(droppedCard);
    //    }
    //}



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
