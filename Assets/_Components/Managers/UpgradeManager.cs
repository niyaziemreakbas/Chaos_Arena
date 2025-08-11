using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class UpgradeManager : SingletonMonoBehaviour<UpgradeManager>
{
    public static Action<CharacterData, int, Owner> OnSpawnCharacter;

    GameState currentGameState = GameState.Upgrade;


    private void OnEnable()
    {
        UpgradeCard.OnUpgradeCardClicked += HandleCardUpgrades;
    }

    private void OnDisable()
    {
        UpgradeCard.OnUpgradeCardClicked -= HandleCardUpgrades;
    }

    // 
    public void HandleCardUpgrades(UpgradeCardData upgradeCardData, Owner owner)
    {
        print($"Handling upgrade for {upgradeCardData.charName} of type {upgradeCardData.upgradeType} for owner {owner.ownerName}");

        switch (upgradeCardData.upgradeType)
        {
            case UpgradeType.Doubler:
                DoubleChar(upgradeCardData.charData, owner);
                break;
            case UpgradeType.Upgrader:
                LevelUpCharsByName(upgradeCardData.charData, owner);
                break;
            case UpgradeType.Spawner:
                CharacterSpawner.Instance.SpawnCharacter(upgradeCardData.charData, upgradeCardData.charData.spawnCount, owner);
                break;
            default:
                Debug.LogWarning("Unhandled upgrade type!");
                break;
        }
        owner.OnUpgradePerformedFunc();
    }

    // Selects a random character from the selected characters list
    public CharacterData SelectRandomChar(Owner owner)
    {

       // print(owner.ownerName + " is selecting a random character from number of : ");

        var mgr = owner.UnitRegistry;

        int chosenCharIndex = UnityEngine.Random.Range(0, mgr.SelectedCharacters.Count);

        return mgr.SelectedCharacters[chosenCharIndex];
    }

    // Selects an upgrade type based on character data
    public UpgradeType SelectUpgradeType(CharacterData charData, Owner owner)
    {
        List<UpgradeType> availableOptions = new List<UpgradeType>();

        if (CheckDoubleValidity(charData, owner))
            availableOptions.Add(UpgradeType.Doubler);

        // Spawner her zaman ekleniyor gibi görünüyor
        availableOptions.Add(UpgradeType.Spawner);

        if (CheckUpgradeValidity(charData, owner))
            availableOptions.Add(UpgradeType.Upgrader);

        if (availableOptions.Count == 0)
        {
           // Debug.LogWarning("No available upgrade options. Defaulting to Spawner.");
            return UpgradeType.Spawner;
        }

//        print(availableOptions.Count + " upgrade options available for character: " + charData.charName);
        int randomIndex = UnityEngine.Random.Range(0, availableOptions.Count);

       // print($"Selected upgrade type: {availableOptions[randomIndex]} for character: {charData.charName}");
        return availableOptions[randomIndex];
    }

    // Creates a random upgrade card based on the selected owner
    public UpgradeCardData SelectRandomUpgradeCard(Owner owner)
    {
        CharacterData charData = SelectRandomChar(owner);

        UpgradeCardData upgradeCardData = new UpgradeCardData
        {
            charImage = charData.charImage,
            charName = charData.charName,
            charData = charData,
            upgradeType = SelectUpgradeType(charData, owner)
        };
        return upgradeCardData;
    }

    // Checks if the character can be upgraded based on its level
    private bool CheckUpgradeValidity(CharacterData charData, Owner owner)
    {
        if (charData.charLevel >= 3)
        {
            //Debug.LogWarning("Character is already at max level!");
            return false;
        }
        if(!CheckCharExistence(charData, owner))
        {
           // Debug.Log($"No character with name {charData.charName} exists in the game scene.");
            return false; // Character does not exist in the game scene
        }
        return true;
    }

    // Checks if there is at least one character with the same name in the game scene
    private bool CheckCharExistence(CharacterData charData, Owner owner)
    {
        var mgr = owner.UnitRegistry;
        
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
    private bool CheckDoubleValidity(CharacterData charData, Owner owner)
    {
        if (CheckCharExistence(charData, owner))
        {
            return true; // Character can be doubled
        }

        return false;
    }



    private bool DoubleChar(CharacterData charData, Owner owner)
    {
        var mgr = owner.UnitRegistry;

        if (!mgr.UnitGroups.TryGetValue(charData.charName, out var units))
        {
            Debug.LogWarning($"No units found for character: {charData.charName}");
            return false;
        }

        //OnSpawnCharacter?.Invoke(charData, units.Count * 2);
        CharacterSpawner.Instance.SpawnCharacter(charData, units.Count * 2, owner);
        return true;
    }

    public bool LevelUpCharsByName(CharacterData charData, Owner owner)
    {
        var mgr = owner.UnitRegistry;
        string charName = charData.charName;

        if (!mgr.UnitGroups.TryGetValue(charName, out var units))
        {
            Debug.LogWarning($"No units found for character: {charName}");
            return false;
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
        return true;
    }
}
public enum UpgradeType
{
    Doubler,
    Upgrader,
    Spawner,
}

