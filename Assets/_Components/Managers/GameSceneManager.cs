using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static Action OnGameEnded;

    [SerializeField] private GameObject inventory;

    private void OnEnable()
    {
        OnGameEnded += HandleGameEnded;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyCards()
    {
        DataManager.Instance.SetPlayerSelectedData(GetCardsAsList());

        SceneManager.LoadScene("CharDevScene");
    }

    private List<CharacterData> GetCardsAsList()
    {
        List<CharacterData> selectedCards = new List<CharacterData>();
        foreach (Transform child in inventory.transform)
        {
            CardSlot cardSlot = child.GetComponent<CardSlot>();
            if (cardSlot != null && cardSlot.CurrentCard != null)
            {
                //print("Card found: " + cardSlot.CurrentCard.CardData.cardName);
                selectedCards.Add(new CharacterData(cardSlot.CurrentCard.CardData));
            }
        }
        return selectedCards;
    }

    private void HandleGameEnded()
    {
        SceneManager.LoadScene("CardDevScene");
    }
}
