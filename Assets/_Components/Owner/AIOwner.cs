using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOwner : Owner
{
    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        unitRegistry.SelectedCharacters = DataManager.Instance.RandomlySelectDeck();
        isUpward = false; // Assuming AI is always downward for now
        //foreach (var character in unitRegistry.SelectedCharacters)
        //{
        //    print("AIOWner : " + character.charName);
        //}
    }

    protected override void HandleUpgradeState()
    {
        UpgradeManager.Instance.HandleCardUpgrades(UpgradeManager.Instance.SelectRandomUpgradeCard(this), this);
    }

    protected override void HandleFightState()
    {
        upgradeCount = 0; // Reset upgrade count after fight
    }
}
