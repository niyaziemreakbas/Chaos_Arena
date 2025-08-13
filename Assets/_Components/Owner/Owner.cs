using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    public event Action<Owner> OnUpgradePerformed;

    private bool isLosedLastFight = false;
    public bool IsLosedLastFight => isLosedLastFight;

    private int fightHealth = 3;
    public int FightHealth => fightHealth;

    public string OwnerName;
    public Color teamColor;

    private Owner enemyOwner;
    public Owner EnemyOwner => enemyOwner;

    protected bool isUpward;
    public bool IsUpward => isUpward;

    protected int upgradeCount = 0;
    public int UpgradeCount => upgradeCount;

    // The spawn origin for owner's characters, where new characters will be instantiated
    public Transform spawnOrigin;

    // The root transform for characters, used for organizing character gameObjects in the hierarchy
    public Transform charsRoot;

    protected UnitRegistry unitRegistry;
    public UnitRegistry UnitRegistry => unitRegistry;

    //private void OnEnable()
    //{
    //    GameStateManager.Instance.OnStateHandle += HandleState;
    //}
    //private void OnDisable()
    //{
    //    GameStateManager.Instance.OnStateHandle -= HandleState;
    //}


    public void HandleState(GameState currentState)
    {
        switch (currentState)
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
       // print(unitRegistry.SpawnedCharacters.Count + " current spawned char Count for owner: " + OwnerName);

    }

    protected virtual void HandleUpgradeState() { }
    protected virtual void HandleFightState() { }

    private void Awake()
    {
        unitRegistry = new UnitRegistry();
    }

    public void Reset()
    {
        print($"Resetting owner {OwnerName}.");
        upgradeCount = 0;
        CharacterSpawner.Instance.RepositionCharacters(this);
        CharacterSpawner.Instance.ActivateAllIfInactive(this.unitRegistry.SpawnedCharacters);
    }

    public void OnUpgradePerformedFunc() { 
      //  print($"Upgrade performed by {ownerName}. Current upgrade count: {upgradeCount}");
        upgradeCount++;
        OnUpgradePerformed?.Invoke(this);
    }

    public void OnLoseFightState()
    {
        isLosedLastFight = true;
        fightHealth--;

        print($"{OwnerName} lost the fight!");
        if (fightHealth <= 0)
        {
            GameSceneManager.OnGameEnded?.Invoke();
        }

        //Handle animations on owners which losing game
    }

    public void SetEnemyOwner(Owner owner)
    {
        enemyOwner = owner;
    }
}
