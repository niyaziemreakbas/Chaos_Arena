using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class UpgradeManager : SingletonMonoBehaviour<UpgradeManager>
{
    public static Action<CharacterData, int, Owner> OnSpawnCharacter;

    [SerializeField] UpgradeWeightConfig upgradeWeightConfig;

    // 
    public bool HandleCardUpgrades(UpgradeCardData upgradeCardData, Owner owner)
    {
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
                return false; // Unhandled upgrade type

        }
        print($"Upgrade performed: {upgradeCardData.upgradeType} for character: {upgradeCardData.charName} by {owner.OwnerName}");
        return true;
    }

    // Selects a random character from the selected characters list
    public CharacterData SelectRandomChar(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        int chosenCharIndex = UnityEngine.Random.Range(0, mgr.SelectedCharacters.Count);

        return mgr.SelectedCharacters[chosenCharIndex];
    }

    // Selects an upgrade type based on character data
    public UpgradeType SelectRandomUpgradeType(CharacterData charData, Owner owner)
    {
        List<(UpgradeType type, float weight)> availableOptions = new();

        if (CheckDoubleValidity(charData, owner))
            availableOptions.Add((UpgradeType.Doubler, upgradeWeightConfig.GetWeight(UpgradeType.Doubler)));

        if (CheckUpgradeValidity(charData, owner))
            availableOptions.Add((UpgradeType.Upgrader, upgradeWeightConfig.GetWeight(UpgradeType.Upgrader)));

        availableOptions.Add((UpgradeType.Spawner, upgradeWeightConfig.GetWeight(UpgradeType.Spawner)));

        if (availableOptions.Count == 0)
            return UpgradeType.Spawner;

        // Adds weights to totalWeight which is in the available options
        float totalWeight = 0;
        foreach (var option in availableOptions)
            totalWeight += option.weight;

        float randomValue = UnityEngine.Random.value * totalWeight;
        float cumulative = 0;

        foreach (var option in availableOptions)
        {
            cumulative += option.weight;
            if (randomValue <= cumulative)
                return option.type;
        }

        print("Upgrade Type could not be selected, an error occurred.");
        return availableOptions[0].type; // fallback
    }

    // Creates a random upgrade card based on the selected owner
    public List<UpgradeCardData> ReturnRandomUpgradeList(Owner owner, int selectCount)
    {
        var selectedCards = new List<UpgradeCardData>();
        var usedUpgradePairs = new Dictionary<string, HashSet<UpgradeType>>();

        int attempts = 0;
        int maxAttempts = selectCount * 9;

        while (selectedCards.Count != selectCount && attempts < maxAttempts)
        {
            attempts++;

            CharacterData charData = SelectRandomChar(owner);
            UpgradeType upgradeType = SelectRandomUpgradeType(charData, owner);

            // First selecting this char
            if (!usedUpgradePairs.ContainsKey(charData.charName))
                usedUpgradePairs[charData.charName] = new HashSet<UpgradeType>();

            // If we have same pair continue trying
            if (usedUpgradePairs[charData.charName].Contains(upgradeType))
                continue;

            // Save this combination
            usedUpgradePairs[charData.charName].Add(upgradeType);

            UpgradeCardData upgradeCardData = new UpgradeCardData
            {
                charImage = charData.charImage,
                charName = charData.charName,
                charData = charData,
                upgradeType = upgradeType
            };

            selectedCards.Add(upgradeCardData);
        }
        return selectedCards;
    }

    // Returns a random upgrade card for the given owner
    public UpgradeCardData ReturnRandomUpgradeCard(Owner owner)
    {
        CharacterData charData = SelectRandomChar(owner);
        UpgradeType upgradeType = SelectRandomUpgradeType(charData, owner);

        UpgradeCardData upgradeCardData = new UpgradeCardData
        {
            charImage = charData.charImage,
            charName = charData.charName,
            charData = charData,
            upgradeType = upgradeType
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


        CharacterSpawner.Instance.SpawnCharacter(charData, units.Count, owner);
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
               // print($"Upgrading character: {unit.gameObject.name} to level {charData.charLevel + 1}");
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

