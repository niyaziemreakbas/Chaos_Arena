using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOwner : Owner
{
    public static event Action OnUpgradeViewHandle;
    public static event Action OnFightViewHandle;

    private void OnEnable()
    {
        UpgradeCard.OnUpgradeCardClicked += UpgradeCardClicked;
    }

    private void OnDisable()
    {
        UpgradeCard.OnUpgradeCardClicked -= UpgradeCardClicked;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        unitRegistry.SelectedCharacters = DataManager.Instance.PlayerSelectedCharacters;
        isUpward = true; // Assuming player is always upward for now
    }

    protected override void HandleUpgradeState()
    {
        print("Player Owner handling upgrade state.");
        OnUpgradeViewHandle?.Invoke();
    }

    protected override void HandleFightState()
    {
        base.HandleFightState();
        OnFightViewHandle?.Invoke();
    }

    private void UpgradeCardClicked(UpgradeCardData upgradeCardData, Owner owner)
    {
        if (GameStateManager.Instance.DecideCanUpgradeForOwner(this) || IsLosedLastFight)
        {
            if(UpgradeCardManager.Instance.HandleCardUpgrades(upgradeCardData, this))
            {
                OnUpgradePerformedFunction();
            }
        }
    }
}
