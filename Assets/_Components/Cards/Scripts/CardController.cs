using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardSO cardSO;

    private CardData cardData; 
    public CardData CardData => cardData;

    private CardSlot currentSlot;
    public CardSlot CurrentSlot => currentSlot;

    private CardView view;

    private void Awake()
    {
        cardData = new CardData(cardSO);
        view = GetComponent<CardView>();
        view.SetCardData(cardData);
    }

    public void SetCurrentSlot(CardSlot slot)
    {
        currentSlot = slot;
    }

    public void UpgradeStats()
    {
        cardData.UpgradeStats();
    }
}

