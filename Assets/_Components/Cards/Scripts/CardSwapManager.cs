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

    public void SwapCards(CardSlot slot1, CardSlot slot2)
    {
        CardData tempCard = slot1.CurrentCard;
        slot1.SetCard(slot2.CurrentCard);
        slot2.SetCard(tempCard);
    }
}
