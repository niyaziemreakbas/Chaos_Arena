using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSwapManager : MonoBehaviour
{
    public static CardSwapManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SwapCards(CardController card1, CardController card2)
    {
        print($"Swapping cards: {card1.CardData.cardName} {card1.CurrentSlot.name} with {card2.CardData.cardName} {card2.CurrentSlot.name}");

        CardSlot slot1 = card1.CurrentSlot;
        CardSlot slot2 = card2.CurrentSlot;

        slot1.SetCurrentCard(card2);
        slot2.SetCurrentCard(card1);

        print($"Swapped cards: {card1.CardData.cardName} {card1.CurrentSlot.name} with {card2.CardData.cardName} {card2.CurrentSlot.name}");

    }
}
