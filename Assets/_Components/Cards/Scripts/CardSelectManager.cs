using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardSelectManager : MonoBehaviour
{
    public static CardSelectManager Instance { get; private set; }

    public List<CardData> SelectedCards { get; private set; } = new List<CardData>();

    [SerializeField] private GameObject inventory;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyCards()
    {
        SelectedCards.Clear();

        foreach (Transform child in inventory.transform)
        {
            CardSlot cardSlot = child.GetComponent<CardSlot>();
            if (cardSlot != null && cardSlot.CurrentCard != null)
            {
                print("Card found: " + cardSlot.CurrentCard.CardData.cardName);
                SelectedCards.Add(cardSlot.CurrentCard.CardData);
            }
        }

        SceneManager.LoadScene("CharDevScene");
    }
}
