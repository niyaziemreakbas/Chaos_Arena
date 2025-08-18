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

    private void OnDisable()
    {
        Character.OnCharDie -= CheckFightState;
    }

    private void CheckFightState(Owner owner)
    {
        if (checkingOwner != null && checkingOwner == owner)
        {
            print($"Already checking end state for owner: {owner.OwnerName}");
            return;
        }

        if (fightEnded) return;

        checkingOwner = owner; // Þu an kontrol edilen owner'ý set et

        bool anyAlive = false;
        foreach (GameObject go in owner.UnitRegistry.SpawnedCharacters)
        {
            if (go.activeInHierarchy)
            {
                anyAlive = true;
                break; // return yerine break
            }
        }

        if (!anyAlive)
        {
            print($"Owner {owner.OwnerName} has no characters left. Ending fight state.");
            fightEnded = true;
            owner.OnLoseFightState();
            OnFightStateEnd?.Invoke();
        }

        checkingOwner = null; // Kontrol bitti
    }

    public void ResetFightState()
    {
        fightEnded = false;
        checkingOwner = null;
    }
}
