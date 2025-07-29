using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    List<CardData> cards = new List<CardData>();

    [SerializeField] private GameObject inventory;

    public void GetCards()
    {
        cards.Clear();
        foreach (Transform child in inventory.transform)
        {
            CardSlot cardSlot = child.GetComponent<CardSlot>();
            if (cardSlot != null && cardSlot.CurrentCard != null)
            {
                print("Card found: " + cardSlot.CurrentCard.cardName);
                cards.Add(cardSlot.CurrentCard);
            }
        }
    }
}
