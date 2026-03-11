using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOwner : Owner
{
    public static event Action OnUpgradeViewHandle;
    public static event Action OnFightViewHandle;
    public static event Action OnCardSelectionHandle;

    private bool selectionLocked = false;

    private void OnEnable()
    {
        UpgradeCardController.OnCardSelected += UpgradeCardClicked;
    }

    private void OnDisable()
    {
        UpgradeCardController.OnCardSelected -= UpgradeCardClicked;
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
        selectionLocked = false;
        OnUpgradeViewHandle?.Invoke();
    }

    protected override void HandleFightState()
    {
        base.HandleFightState();
        OnFightViewHandle?.Invoke();
    }

    private void UpgradeCardClicked(UpgradeCardData upgradeCardData, Owner owner)
    {
        if (selectionLocked)
            return;

        selectionLocked = true;

        print("PLAYER CLICK");

        if (UpgradeCardManager.Instance.HandleCardUpgrades(upgradeCardData, this))
        {
            OnUpgradePerformedFunction();

            OnCardSelectionHandle?.Invoke();
        }
        else
        {
            Debug.LogError("UpgradeCardManager failed to handle the card upgrade. Check the implementation of HandleCardUpgrades.");
            selectionLocked = false;
        }
    }
}
