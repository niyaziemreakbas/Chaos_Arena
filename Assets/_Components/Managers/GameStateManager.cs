using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public static event Action OnCharUIUpdateHandle;

    [SerializeField] TextMeshProUGUI upgradeCount;

    private GameState currentState = GameState.Upgrade;
    public GameState CurrentState => currentState;

    List<Owner> owners = new List<Owner>();

    private int maxUpgradeCount = 3;

    private int currentUpgradeCount = 0;

    private bool bonusPlayedFlag = false;

    private void OnEnable()
    {
        FightManager.OnFightStateEnd += HandleFightStateEnd;
    }

    private void OnDisable()
    {
        FightManager.OnFightStateEnd -= HandleFightStateEnd;
        UnsubscribeOwners();
    }

    private void Start()
    {
        owners = OwnerManager.Instance.Owners;

        SubscribeOwners();

        StartCoroutine(StartGameAfterDelay());
    }

    private void Update()
    {
        upgradeCount.text = $"Turn : {currentUpgradeCount}/{maxUpgradeCount}";
       // print(currentState + " is the current state");
    }

    private void HandleFightStateEnd()
    {
        print("Fight state ended, resetting game...");

        StartCoroutine(ResetGameAfterDelay());
    }

    private IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);

        currentUpgradeCount = 0;

        ResetOwners();

        print("The game resetted now deciding to state bonus or upgrade");
        if (owners.Exists(o => o.IsLosedLastFight))
        {
            ChangeState(GameState.Idle);
            StartCoroutine(HandleBonusState());
        }
        else
        {
            print("Decided to upgrade");
            ChangeState(GameState.Upgrade);
        }
    }

    private IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);

        HandleStatesOnOwners();
    }

    private void ChangeState(GameState newState)
    {
        if (currentState == newState)
        {
            Debug.LogWarning($"Game State is already {newState}, no change made.");
            return;
        }

        if(newState == GameState.Idle)
        {
            currentState = GameState.Idle;
            print("Game waiting on idle now");
            return;
        }

        print($"Changing game state from {currentState} to {newState}");
        currentState = newState;
        HandleStatesOnOwners();
    }

    private void SubscribeOwners()
    {
        foreach (var owner in owners)
        {
            owner.OnUpgradePerformed += HandleOwnerUpgraded;
        }
    }

    private void UnsubscribeOwners()
    {
        foreach (var owner in owners)
        {
            owner.OnUpgradePerformed -= HandleOwnerUpgraded;
        }
    }

    public bool DecideCanUpgrade()
    {
        if (owners.Count == 0) return false;

        // Ýlk owner'ýn upgrade count'unu referans alýyoruz
        int targetCount = owners[0].UpgradeCount;

        foreach (var owner in owners)
        {
            if (owner.UpgradeCount != targetCount)
            {
                return false;
            }
        }
        return true;
    }

    public bool DecideCanUpgradeForOwner(Owner owner)
    {
        if (currentUpgradeCount == owner.UpgradeCount)
        {
            return true;
        }
        return false;
    }

    private void HandleOwnerUpgraded(Owner owner)
    {
        if (DecideCanUpgrade())
        {
            currentUpgradeCount++;
        }
        else
        {
            print($"Owner {owner.OwnerName} performed an upgrade but not all owners have reached the same upgrade count yet.");
            return;
        }

        // All owners have performed their upgrades and reached the same count now we going fight state.
        if (currentUpgradeCount >= maxUpgradeCount)
        {
            ChangeState(GameState.Fight);
            FightManager.Instance.ResetFightState();
            return;
        }
        else
        {
            // Upgrade again
            OnCharUIUpdateHandle?.Invoke();
            HandleStatesOnOwners();
        }

    }

    private IEnumerator HandleBonusState()
    {
        print("Handling Bonus State");
        foreach (var owner in owners)
        {
            if (owner.IsLosedLastFight)
            {
                bonusPlayedFlag = false;

                owner.OnBonusPlayed += HandleBonusPlayed; // evente abone ol

                owner.HandleState(GameState.Upgrade);

                yield return new WaitUntil(() => bonusPlayedFlag);

                owner.OnBonusPlayed -= HandleBonusPlayed;

                ChangeState(GameState.Upgrade);
            }
        }
    }

    private void HandleBonusPlayed()
    {
        bonusPlayedFlag = true;
    }

    public void HandleStatesOnOwners()
    {
        //bool isBonusRoundPlayed = false;
        foreach (var owner in owners)
        {
            owner.HandleState(currentState);
        }
    }

    private void ResetOwners()
    {
        foreach (var owner in owners)
        {
            owner.Reset();
        }
    }
}

public enum GameState
{
    Upgrade,
    Fight,
    Idle
}

