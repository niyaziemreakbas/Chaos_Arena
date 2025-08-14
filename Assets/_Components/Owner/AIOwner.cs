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
    }

    protected override void HandleUpgradeState()
    {
        UpgradeManager.Instance.HandleCardUpgrades(UpgradeManager.Instance.ReturnRandomUpgradeCard(this), this);
    }

    protected override void HandleFightState()
    {
        foreach (var character in unitRegistry.SpawnedCharacters)
        {
            character.GetComponent<Character>().OnFightStateStarted();
        }
        upgradeCount = 0; // Reset upgrade count after fight
    }
    
}
