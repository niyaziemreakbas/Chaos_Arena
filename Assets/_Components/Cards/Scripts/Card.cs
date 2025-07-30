using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
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

public class CardData{

    public string cardName;

    public int priorityLevel;

    public int health;

    public int damage;

    public float range;

    public int movementSpeed;

    public int attackSpeed;

    public Sprite cardImage;

    public event Action onStatsChanged;

    public CardData(CardSO cardSO)
    {
        priorityLevel = cardSO.priorityLevel;
        cardName = cardSO.cardName;
        health = cardSO.health;
        damage = cardSO.damage;
        range = cardSO.range;
        movementSpeed = cardSO.movementSpeed;
        attackSpeed = cardSO.attackSpeed;
        cardImage = cardSO.cardImage;
    }

    public void UpgradeStats()
    {
        health += 10;
        damage += 5;

        onStatsChanged?.Invoke();
    }
}
