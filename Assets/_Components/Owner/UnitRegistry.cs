using System.Collections.Generic;
using UnityEngine;

public class UnitRegistry
{
    //Selected characters from the card selection
    public List<CharacterData> SelectedCharacters { get; set; } = new List<CharacterData>();

    // List for characters that are ordered by priority level, can be used for showing spawned character data types
    public List<CharacterData> CharacterOrder { get; private set; } = new List<CharacterData>();

    // Dictionary to hold unit gameObjects grouped by character name, used for repositioning characters 
    public Dictionary<string, List<GameObject>> UnitGroups { get; private set; } = new Dictionary<string, List<GameObject>>();

    // Dictinoary to hold parent transforms for each character name, used for organizing units in the hierarchy
    public Dictionary<string, Transform> UnitParents { get; private set; } = new Dictionary<string, Transform>();

    // List to hold all spawned character gameObjects, used for detecting targets by enemies.
    public List<GameObject> SpawnedCharacters { get; private set; } = new List<GameObject>();
}
