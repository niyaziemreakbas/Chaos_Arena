using System.Collections.Generic;
using UnityEngine;

public class UnitRegistry
{
    //Selected characters from the card selection
    public List<CharacterData> SelectedCharacters { get; set; } = new List<CharacterData>();

    // List for characters that are ordered by priority level, can be used for showing spawned character data types
    public List<CharacterData> SpawnedCharData { get; private set; } = new List<CharacterData>();

    // Dictionary to hold unit gameObjects grouped by character name, used for repositioning characters 
    public List<UnitGroup> UnitGroups { get; private set; } = new List<UnitGroup>();

    public void AddUnitGroup(UnitGroup group)
    {
        UnitGroups.Add(group);
    }

    public bool HasUnitGroup(string charName)
    {
        foreach (var item in UnitGroups)
        {
            if (item.characterName == charName)
            {
                return true;
            }
        }
        return false;
    }

    public UnitGroup ReturnUnitGroup(string CharName)
    {
        foreach (var item in UnitGroups)
        {
            if (item.characterName == CharName)
            {
                return item;
            }
        }
        return null;
    }

    // Dictinoary to hold parent transforms for each character name, used for organizing units in the hierarchy
    public Dictionary<string, Transform> UnitParents { get; private set; } = new Dictionary<string, Transform>();

    // List to hold all spawned character gameObjects, used for detecting targets by enemies.
    public List<GameObject> SpawnedCharacters { get; private set; } = new List<GameObject>();
}
