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

    private void Start()
    {
        //ShowCardUpgradeProp();
        canvas.SetActive(false);
        StartUpgradeRoutine();

    }

    private void OnEnable()
    {
        UpgradeCardController.OnUpgradeCardClicked += HandleCardUpgrades;
    }

    private void OnDisable()
    {
        UpgradeCardController.OnUpgradeCardClicked -= HandleCardUpgrades;
    }


    public CharacterData SelectRandomChar()
    {
        var mgr = CharacterManager.Instance;

        int chosenCharIndex = UnityEngine.Random.Range(0, mgr.CharacterOrder.Count);

        return mgr.CharacterOrder[chosenCharIndex];
    }


    private IEnumerator UpgradeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (CharacterManager.Instance.CharacterOrder.Count > 1)
                currentGameState = GameState.Upgrade;
            HandleState();
        }
    }

    private void StartUpgradeRoutine()
    {
        if (upgradeCoroutine == null)
        {
            upgradeCoroutine = StartCoroutine(UpgradeRoutine());
        }
    }

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
        StopCoroutine(upgradeCoroutine);
        ShowCardUpgradeProp();
        canvas.SetActive(true);
    }

    private void HandleFightState()
    {
        StartUpgradeRoutine();
        canvas.SetActive(false);
    }

    private void HandleCardUpgrades(UpgradeCardData upgradeCardData)
    {

        switch(upgradeCardData.upgradeType)
        {
            case UpgradeType.Doubler:
                DoubleChar(SelectRandomChar());
                break;
            case UpgradeType.Upgrader:
                UpgradeChar(SelectRandomChar());
                break;
            case UpgradeType.Spawner:
                // Handle spawner logic if needed
                Debug.Log("Spawner upgrade selected, but no action defined.");
                break;
            default:
                Debug.LogWarning("Unhandled upgrade type!");
                break;
        }
    }

    public UpgradeType SelectUpgradeType(CharacterData charData)
    {
        int random;

        if (!CheckUpgradeValidity(charData))
        {
            random = UnityEngine.Random.Range(0, 2); // 0, 1
        }
        else
        {
            // Randomly select an upgrade type
            random = UnityEngine.Random.Range(0, 3); // 0, 1, 2
        }

        switch (random)
        {
            case 0:
                return UpgradeType.Doubler;

            case 1:
                return UpgradeType.Spawner;

            case 2:
                return UpgradeType.Upgrader;

            default:
                Debug.LogWarning("Unhandled random value!");
                return UpgradeType.Spawner;
        }
    }

    private bool CheckUpgradeValidity(CharacterData charData)
    {
        if (charData.charLevel >= 3)
        {
            Debug.LogWarning("Character is already at max level!");
            return false;
        }
        return true;
    }

    private void ShowCardUpgradeProp()
    {
        foreach (var card in upgradeCards)
        {
            CharacterData charData = SelectRandomChar();
            UpgradeCardData upgradeCardData = new UpgradeCardData
            {
                charImage = charData.charImage,
                charName = charData.charName,
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

