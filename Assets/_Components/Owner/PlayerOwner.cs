using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOwner : Owner
{
    public static event Action OnUpgradeViewHandle;
    public static event Action OnFightViewHandle;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        unitRegistry.SelectedCharacters = DataManager.Instance.PlayerSelectedCharacters;

        //foreach (var character in unitRegistry.SelectedCharacters)
        //{
        //    print("Player : " + character.charName);
        //}

    }

    protected override void HandleUpgradeState()
    {
        OnUpgradeViewHandle?.Invoke();

    }

    protected override void HandleFightState()
    {
        OnFightViewHandle?.Invoke();
    }
}
