using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : SingletonMonoBehaviour<FightManager>
{
    public static event Action OnFightStateEnd;

    private bool fightEnded = false;
    private Owner checkingOwner;

    private void OnEnable()
    {
        Character.OnCharDie += CheckFightState;
    }

    private void CheckFightState(Owner owner)
    {
        if(checkingOwner != null && checkingOwner == owner)
        {
            print($"Already checking end state for owner: {owner.OwnerName}");
            return;
        }

        if (fightEnded) return; // Eðer zaten bitmiþse tekrar kontrol etme

        bool anyAlive = false;
        foreach (GameObject go in owner.UnitRegistry.SpawnedCharacters)
        {
            if (go.activeInHierarchy)
            {
                anyAlive = true;
                return;
            }
        }

        if (!anyAlive)
        {
            print($"Owner {owner.OwnerName} has no characters left. Ending fight state.");
            fightEnded = true; // Fight sadece bir kez bitirilecek
            owner.OnLoseFightState();
            OnFightStateEnd?.Invoke();
        }
    }

    public void ResetFightState()
    {
        fightEnded = false;
        checkingOwner = null;
    }
}
