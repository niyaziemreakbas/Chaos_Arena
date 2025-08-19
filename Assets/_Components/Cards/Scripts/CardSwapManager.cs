using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSwapManager : SingletonMonoBehaviour<CardSwapManager>
{
    public CardController currentCard;

    public bool SwapCompleted = false;

    public void SelectCurrentCard(CardController card)
    {
        currentCard = card;
    }

    public void SwapCards(CardController UpComingCard, CardController OnDeckCard)
    {
        UpComingCard.gameObject.tag = "OnDeck";
        OnDeckCard.gameObject.tag = "Untagged";

        CardSlot slot1 = UpComingCard.CurrentSlot;
        CardSlot slot2 = OnDeckCard.CurrentSlot;

        slot1.SetCurrentCard(OnDeckCard);
        slot2.SetCurrentCard(UpComingCard);

        SwapCompleted = true;

        currentCard = null;

        print($"Swapped cards: {UpComingCard.CardData.cardName} {UpComingCard.CurrentSlot.name} with {OnDeckCard.CardData.cardName} {OnDeckCard.CurrentSlot.name}");

    }

    //public bool HasSwapped(CardView card)
    //{
    //    print($"Checking if swapped card: {card.CardData.cardName} is the last swapped card: {lastSwappedCard?.CardData.cardName}");
    //    return lastSwappedCard == card;
    //}

    public bool CanSwap()
    {
        return currentCard != null;
    }
}
