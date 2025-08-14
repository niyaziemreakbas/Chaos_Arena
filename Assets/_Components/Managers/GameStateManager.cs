using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public event Action OnStateHandle;

    [SerializeField] TextMeshProUGUI upgradeCount;

    private GameState currentState = GameState.Upgrade;
    public GameState CurrentState => currentState;

    List<Owner> owners = new List<Owner>();

    private int maxUpgradeCount = 3;

    private int currentUpgradeCount = 0;

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
        upgradeCount.text = $"Upgrade Count: {currentUpgradeCount}/{maxUpgradeCount}";
    }

    private void HandleFightStateEnd()
    {
        print("Fight state ended, resetting game...");
        StartCoroutine(ResetGameAfterDelay());
    }

    private IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        print("Game state to Upgrade...");
        ResetOwners();
        currentUpgradeCount = 0;
        ChangeState(GameState.Upgrade);
    }

    private IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(0.0f);
        
        HandleStatesOnOwners();
    }

    void ChangeState(GameState newState)
    {
        if (currentState == newState)
            return;

        if(currentState == GameState.Fight && newState == GameState.Upgrade)
        {
            ResetOwners();
        }

        currentState = newState;
        Debug.Log($"Game State changed to {newState}");

        HandleStatesOnOwners();
        //OnStateHandle?.Invoke(newState);
    }

    private void SubscribeOwners()
    {
        foreach (var owner in owners)
        {
           // print($"Subscribing to owner: {owner.ownerName}");
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

    private bool DecideCanUpgrade()
    {
        foreach (var owner in owners)
        {
            //print($"Checking owner: {owner.ownerName} with upgrade count: {owner.UpgradeCount} against current upgrade count: {currentUpgradeCount}");
            if (owner.UpgradeCount != currentUpgradeCount)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleOwnerUpgraded(Owner owner)
    {
        // Owner upgrade performed but not all handled yet.
        if (!DecideCanUpgrade())
        {
            return;
        }

        // All owners have performed their upgrades and reached the same count now we increment the upgrade count.
        if (currentUpgradeCount >= maxUpgradeCount)
        {
            ChangeState(GameState.Fight);
            FightManager.Instance.ResetFightState(); // Reset fight state for the new fight
        }

        //print($"All owners have performed their upgrades. Upgrade count increasing...Current upgrade count: {currentUpgradeCount}");
        HandleStatesOnOwners();
    }

    public void HandleStatesOnOwners()
    {
        foreach (var owner in owners)
        {
            if (owner.IsLosedLastFight)
            {
                //owner.OnUpgradePerformedFunc(); // Kaybedene ekstra upgrade
                owner.HandleState(currentState);
                print($"{owner.OwnerName} gets extra upgrade for losing last fight!");
            }

            owner.HandleState(currentState);
        }

        if (currentState == GameState.Upgrade)
        {
            currentUpgradeCount++;
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
}

