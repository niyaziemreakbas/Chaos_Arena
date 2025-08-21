using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    private CardController currentCard;

    public CardController CurrentCard => currentCard;

    private Button slotButton;

    private void Start()
    {
        slotButton = GetComponent<Button>();
        SetChildrenToCurrentCard();
        //print(CurrentCard.CardData.cardName);

        slotButton.onClick.AddListener(() => {
            CardSwapManager.Instance.HandleSlotClicked(this);
        });
    }

    void SetChildrenToCurrentCard()
    {
        CardController card = transform.GetComponentInChildren<CardController>();
        SetCurrentCard(card);
        card.SetCurrentSlot(this);
    }

    //Need an animation
    public void SetCurrentCard(CardController newCard)
    {
        newCard.tag = gameObject.tag;
        currentCard = newCard;
        newCard.transform.SetParent(transform);
        newCard.SetCurrentSlot(this);
    }

    public void ClearCurrentCard()
    {
        if (currentCard != null)
        {
            currentCard.transform.SetParent(null);
            currentCard.SetCurrentSlot(null);
            currentCard = null;
        }
    }

    public bool HasCard()
    {
        return currentCard != null;
    }
}
