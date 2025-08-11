using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private List<CharacterData> playerSelectedCharacters = new List<CharacterData>();
    public List<CharacterData> PlayerSelectedCharacters => playerSelectedCharacters;

    public List<CardSO> cardSOList = new List<CardSO>();
    private List<CardData> cardDataList = new List<CardData>();
    private List<CharacterData> characterDataList = new List<CharacterData>();
    public List<CharacterData> CharDataList => characterDataList;

    protected override void ChildAwake()
    {
        //DontDestroyOnLoad(gameObject);

        GenerateCardData();
        GenerateCharData();
    }

    void GenerateCardData()
    {
        foreach (var cardSO in cardSOList)
        {
            CardData cardData = new CardData(cardSO);
            cardDataList.Add(cardData);
        }
    }
    void GenerateCharData()
    {
        foreach (var cardData in cardDataList)
        {
            CharacterData characterData = new CharacterData(cardData);
            characterDataList.Add(characterData);
        }
    }
    CharacterData RandomlySelectCharData()
    {
        if (characterDataList.Count == 0)
        {
            Debug.LogWarning("Character data list is empty. Cannot select a character.");
            return null;
        }
        int randomIndex = Random.Range(0, characterDataList.Count);
        CharacterData selectedCharacterData = characterDataList[randomIndex];
        return selectedCharacterData;
    }

    public List<CharacterData> RandomlySelectDeck()
    {
        int deckSize = 4; // Example deck size
        List<CharacterData> selectedDeck = new List<CharacterData>();

        for (int i = 0; i < deckSize; i++)
        {
            if (characterDataList.Count == 0)
            {
                Debug.LogWarning("Character data list is empty. Cannot select a character for the deck.");
                return new List<CharacterData>();
            }

            selectedDeck.Add(RandomlySelectCharData());
        }

        return selectedDeck;

    }

    public void SetPlayerSelectedData(List<CharacterData> characterDatas)
    {
        playerSelectedCharacters.Clear();

        foreach (var characterData in characterDatas)
        {
            playerSelectedCharacters.Add(characterData);
        }
    }

    //void LoadCardSOs()
    //{
    //    cardSOs.Clear();
    //    var loadedCardSOs = Resources.LoadAll<CardSO>("Cards");
    //    foreach (var cardSO in loadedCardSOs)
    //    {
    //        cardSOs.Add(cardSO);
    //    }
    //}
}
