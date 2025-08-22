using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Owner : MonoBehaviour
{
    public event Action<Owner> OnUpgradePerformed;
    public event Action OnBonusPlayed;
    public event Action OnDataChanged;

    protected bool isLosedLastFight = false;
    public bool IsLosedLastFight
    {
        get => isLosedLastFight;
        protected set
        {
            if (isLosedLastFight != value)
            {
                isLosedLastFight = value;
                OnDataChanged?.Invoke();
            }
        }
    }

    private int gameHealth = 3;
    public int GameHealth
    {
        get => gameHealth;
        private set
        {
            if (gameHealth != value)
            {
                gameHealth = value;
                OnDataChanged?.Invoke();
            }
        }
    }

    protected int upgradeCount = 0;
    public int UpgradeCount
    {
        get => upgradeCount;
        protected set
        {
            if (upgradeCount != value)
            {
                upgradeCount = value;
                OnDataChanged?.Invoke();
            }
        }
    }

    public string OwnerName;
    public Color teamColor;

    private Owner enemyOwner;
    public Owner EnemyOwner => enemyOwner;

    protected bool isUpward;
    public bool IsUpward => isUpward;

    // The spawn origin for owner's characters, where new characters will be instantiated
    public Transform spawnOrigin;

    // The root transform for characters, used for organizing character gameObjects in the hierarchy
    public Transform charsRoot;

    protected UnitRegistry unitRegistry;
    public UnitRegistry UnitRegistry => unitRegistry;

    protected void OnUpgradePerformedFunction()
    {
        if (IsLosedLastFight)
        {
            IsLosedLastFight = false;
            OnBonusPlayed?.Invoke();
        }
        else
        {
            UpgradeCount++;
            OnUpgradePerformed?.Invoke(this);
        }
    }

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
    }

    protected abstract void HandleUpgradeState();

    protected virtual void HandleFightState()
    {
        UpgradeCount = 0;

        foreach (var character in unitRegistry.SpawnedCharacters)
        {
            character.GetComponent<Character>().OnFightStateStarted();
        }
    }

    private void Awake()
    {
        unitRegistry = new UnitRegistry();
        //selector = GetComponent<IUpgradeSelector>();
    }

    public void Reset()
    {
        UpgradeCount = 0;

        CharacterSpawner.Instance.RepositionCharacters(this);
        CharacterSpawner.Instance.ActivateAllIfInactive(unitRegistry.SpawnedCharacters);
    }

    public void OnLoseFightState()
    {
        IsLosedLastFight = true;
        GameHealth--;

        print($"{OwnerName} lost the fight!");
        if (gameHealth <= 0)
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
