using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static Action<CharacterData, int> OnSpawnCharacter;

    public void CreateRandomCard()
    {
        var mgr = CharacterManager.Instance;

        int chosenCharIndex = UnityEngine.Random.Range(0, mgr.CharacterOrder.Count);

        SelectUpgradeType(mgr.CharacterOrder[chosenCharIndex]);
    }

    // Not a modular system needs a change
    public void SelectUpgradeType(CharacterData charData)
    {
        // Randomly select an upgrade type
        int random = UnityEngine.Random.Range(0, 3); // 0, 1, 2

        switch (random)
        {
            case 0:
                UpgradeChar(charData);
                break;

            case 1:
                DoubleChar(charData);
                break;

            case 2:
                SpawnChar(charData);
                break;

            default:
                Debug.LogWarning("Unhandled random value!");
                break;
        }
    }

    private void SpawnChar(CharacterData charData)
    {
        OnSpawnCharacter?.Invoke(charData, charData.spawnCount);
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

public class UpgradeCard
{

}
