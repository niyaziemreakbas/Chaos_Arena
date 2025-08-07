using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviourSingleton<CharacterManager>
{
    //Selected characters from the card selection
    public List<CharacterData> SelectedCharacters { get; private set; } = new List<CharacterData>();

    // List for characters that are ordered by priority level, can be used for showing spawned characters
    public List<CharacterData> CharacterOrder { get; private set; } = new List<CharacterData>();

    // Dictionary to hold unit gameObjects grouped by character name
    public Dictionary<string, List<GameObject>> UnitGroups { get; private set; } = new Dictionary<string, List<GameObject>>();

    // Dictionary to hold colors for each character name, using for testing
    public Dictionary<string, Color> UnitColors { get; private set; } = new Dictionary<string, Color>();

    // Dictinoary to hold parent transforms for each character name, used for organizing units in the hierarchy
    public Dictionary<string, Transform> UnitParents { get; private set; } = new Dictionary<string, Transform>();

    public List<GameObject> PlayerCharacters { get; private set; } = new List<GameObject>();

    public List<GameObject> EnemyCharacters { get; private set; } = new List<GameObject>();

}
