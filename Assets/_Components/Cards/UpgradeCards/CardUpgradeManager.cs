using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CardUpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public static Action<CharacterData, int> OnSpawnCharacter;

    public List<GameObject> upgradeCards = new List<GameObject>();

    GameState currentGameState = GameState.Upgrade;

    Coroutine upgradeCoroutine;

    int loopCount = 0;

    private void Start()
    {
        //ShowCardUpgradeProp();
        canvas.SetActive(false);
        //StartUpgradeRoutine();
        HandleState();
    }

    private void OnEnable()
    {
        UpgradeCardController.OnUpgradeCardClicked += HandleCardUpgrades;
    }

    private void OnDisable()
    {
        UpgradeCardController.OnUpgradeCardClicked -= HandleCardUpgrades;
    }

    // Selects a random character from the selected characters list
    public CharacterData SelectRandomChar()
    {
        var mgr = CharacterManager.Instance;

        int chosenCharIndex = UnityEngine.Random.Range(0, mgr.SelectedCharacters.Count);

        return mgr.SelectedCharacters[chosenCharIndex];
    }

    //private IEnumerator UpgradeRoutine()
    //{
    //    int loopCount = 0;
    //    while (loopCount < 3)
    //    {
    //        yield return new WaitForSeconds(5f);

    //        if (CharacterManager.Instance.CharacterOrder.Count > 1)
    //            currentGameState = GameState.Upgrade;
            
    //        HandleState();

    //        loopCount++;
    //    }
    //}

    //private void StartUpgradeRoutine()
    //{
    //    if (upgradeCoroutine == null)
    //    {
    //        upgradeCoroutine = StartCoroutine(UpgradeRoutine());
    //    }
    //}

    private void HandleState()
    {
        switch (currentGameState)
        {
            case GameState.Upgrade:
                HandleUpgradeState();
                break;
            case GameState.Fight:
                HandleFightState();
                break;
            default:
                Debug.LogWarning("Unhandled game state!");
                break;
        }
    }

    private void HandleUpgradeState()
    {
        //StopCoroutine(upgradeCoroutine);
        ShowCardUpgradeProp();
        canvas.SetActive(true);
        loopCount++;
    }

    private void HandleFightState()
    {
        loopCount = 0;
        //StartUpgradeRoutine();
        canvas.SetActive(false);
    }

    private void HandleCardUpgrades(UpgradeCardData upgradeCardData)
    {
        switch(upgradeCardData.upgradeType)
        {
            case UpgradeType.Doubler:
                DoubleChar(upgradeCardData.charData);
                break;
            case UpgradeType.Upgrader:
                UpgradeChar(upgradeCardData.charData);
                break;
            case UpgradeType.Spawner:
                SpawnChar(upgradeCardData.charData);
                break;
            default:
                Debug.LogWarning("Unhandled upgrade type!");
                break;
        }

        // After handling the upgrade third times, we can reset the game state to Fight
        if(loopCount == 3)
        {
            currentGameState = GameState.Fight;
            print("Upgrade handled, switching to Fight state.");

        }
        HandleState();

    }

    // Selects an upgrade type based on character data
    public UpgradeType SelectUpgradeType(CharacterData charData)
    {
        List<UpgradeType> availableOptions = new List<UpgradeType>();

        if (CheckDoubleValidity(charData))
            availableOptions.Add(UpgradeType.Doubler);

        // Spawner her zaman ekleniyor gibi görünüyor
        availableOptions.Add(UpgradeType.Spawner);

        if (CheckUpgradeValidity(charData))
            availableOptions.Add(UpgradeType.Upgrader);

        if (availableOptions.Count == 0)
        {
            Debug.LogWarning("No available upgrade options. Defaulting to Spawner.");
            return UpgradeType.Spawner;
        }

        print(availableOptions.Count + " upgrade options available for character: " + charData.charName);
        int randomIndex = UnityEngine.Random.Range(0, availableOptions.Count);

        print($"Selected upgrade type: {availableOptions[randomIndex]} for character: {charData.charName}");
        return availableOptions[randomIndex];
    }

    // Checks if the character can be upgraded based on its level
    private bool CheckUpgradeValidity(CharacterData charData)
    {
        if (charData.charLevel >= 3 || !CheckCharExistence(charData))
        {
            Debug.LogWarning("Character is already at max level!");
            return false;
        }
        return true;
    }

    // Checks if there is at least one character with the same name in the game scene
    private bool CheckCharExistence(CharacterData charData)
    {
        var mgr = CharacterManager.Instance;
        foreach (var unit in mgr.CharacterOrder)
        {
            if (charData.charName == unit.charName)
            {
                return true; // At least one character with that name exists in game scene
            }
        }
        return false;
    }

    // Checks if there is at least one character with the same name in the selected characters
    private bool CheckDoubleValidity(CharacterData charData)
    {
        if (CheckCharExistence(charData))
        {
            return true; // Character can be doubled
        }

        return false;
    }

    // Displays upgrade cards with random character data and upgrade types
    private void ShowCardUpgradeProp()
    {
        foreach (var card in upgradeCards)
        {
            CharacterData charData = SelectRandomChar();
            
            UpgradeCardData upgradeCardData = new UpgradeCardData
            {
                charImage = charData.charImage,
                charName = charData.charName,
                charData = charData,
                upgradeType = SelectUpgradeType(charData)
            };

            card.GetComponent<UpgradeCardController>().SetUpgradeCard(upgradeCardData);
        }
    }

    private void DoubleChar(CharacterData charData)
    {
        var mgr = CharacterManager.Instance;

        if (!mgr.UnitGroups.TryGetValue(charData.charName, out var units))
        {
            Debug.LogWarning($"No units found for character: {charData.charName}");
            return;
        }

        OnSpawnCharacter?.Invoke(charData, units.Count * 2);
    }

    private void UpgradeChar(CharacterData charData)
    {
        UpgradeCharactersByName(charData.charName);
    }

    //Reaches CharacterSpawner.cs to spawn characters
    private void SpawnChar(CharacterData charData)
    {
        CharacterSpawner.Instance.SpawnCharacter(charData, charData.spawnCount);
    }

    public void UpgradeCharactersByName(string charName)
    {
        var mgr = CharacterManager.Instance;

        if (!mgr.UnitGroups.TryGetValue(charName, out var units))
        {
            Debug.LogWarning($"No units found for character: {charName}");
            return;
        }

        foreach (var unit in units)
        {
            // Assuming each unit has a method to upgrade itself
            Character characterUnit = unit.GetComponent<Character>();
            if (characterUnit != null)
            {
                characterUnit.UpgradeChar();
            }
            else
            {
                Debug.LogWarning($"No CharacterUnit component found on unit: {unit.name}");
            }
        }
    }
}
public enum UpgradeType
{
    Doubler,
    Upgrader,
    Spawner,
}

public enum GameState
{
    Upgrade,
    Fight,
}

