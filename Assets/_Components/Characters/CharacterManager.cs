using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    //Selected characters from the card selection
    public List<CharacterData> SelectedCharacters { get; private set; } = new List<CharacterData>();

    // List for characters that are ordered by priority level, can be used for showing spawned characters
    public List<CharacterData> CharacterOrder { get; private set; } = new List<CharacterData>();

    // Dictionary to hold unit gameObjects grouped by character name
    public Dictionary<string, List<GameObject>> UnitGroups { get; private set; } = new Dictionary<string, List<GameObject>>();

    // Dictionary to hold colors for each character name, using for testing
    public Dictionary<string, Color> UnitColors { get; private set; } = new Dictionary<string, Color>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Define selected characters from CardSelectManager
        if (CardSelectManager.Instance != null)
        {
            foreach (var card in CardSelectManager.Instance.SelectedCards)
            {
                SelectedCharacters.Add(new CharacterData(card));
            }
        }
    }
}
