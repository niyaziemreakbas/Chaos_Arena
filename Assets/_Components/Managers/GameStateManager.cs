using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    //public event Action<GameState> OnStateHandle;

    [SerializeField] TextMeshProUGUI upgradeCount;

    private GameState currentState = GameState.Upgrade;
    public GameState CurrentState => currentState;

    List<Owner> owners = new List<Owner>();

    GameObject OwnerContainer;

    private int maxUpgradeCount = 3;

    private int currentUpgradeCount = 0;

    private bool canUpgrade;

    private void Start()
    {
        owners = OwnerManager.Instance.Owners;

        SubscribeOwners();

        //HandleStatesOnOwners();
    }

    private void Update()
    {
        upgradeCount.text = $"Upgrade Count: {currentUpgradeCount}/{maxUpgradeCount}";
    }

    void ChangeState(GameState newState)
    {
        if (CurrentState == newState)
            return;

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
                canUpgrade = false;
                return false;
            }
        }
        canUpgrade = true;
        return true;
    }

    private void HandleOwnerUpgraded(Owner owner)
    {
        //print($"Owner {owner.ownerName} performed an upgrade. Current upgrade count: {owner.UpgradeCount}");

        // Owner upgrade performed but not all handled yet.
        if (!DecideCanUpgrade())
        {
            Debug.Log("Cannot upgrade yet, waiting for all owners to reach the same upgrade count.");
            return;
        }

        // All owners have performed their upgrades and reached the same count now we increment the upgrade count.
        if (currentUpgradeCount >= maxUpgradeCount)
        {
            print("Max upgrade count reached, switching to fight state.");
            ChangeState(GameState.Fight);
            StartCoroutine(HandleTempFightState());
        }

        print($"All owners have performed their upgrades. Upgrade count increasing...Current upgrade count: {currentUpgradeCount}");
        HandleStatesOnOwners();
    }

    public void HandleStatesOnOwners()
    {
        foreach (var owner in owners)
        {
            owner.HandleState(currentState);
        }
        if (currentState == GameState.Upgrade)
        {
            currentUpgradeCount++;
        }
    }

    private IEnumerator HandleTempFightState()
    {
        yield return new WaitForSeconds(5f);
        currentUpgradeCount = 0; // Reset upgrade count after fight state
        ChangeState(GameState.Upgrade);
    }
}

public enum GameState
{
    Upgrade,
    Fight,
}

