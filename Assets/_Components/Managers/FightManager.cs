using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : SingletonMonoBehaviour<FightManager>
{
    public static event Action OnFightStateEnd;

    private bool fightEnded = false;

    private void OnEnable()
    {
        Character.OnCharDie += CheckFightState;
    }

    private void CheckFightState(Owner owner)
    {
        if (fightEnded) return; // Eðer zaten bitmiþse tekrar kontrol etme

        bool anyAlive = false;
        foreach (GameObject go in owner.UnitRegistry.SpawnedCharacters)
        {
            if (go.activeInHierarchy)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            fightEnded = true; // Fight sadece bir kez bitirilecek
            owner.OnLoseFightState();
            OnFightStateEnd?.Invoke();
        }
    }

    public void ResetFightState()
    {
        fightEnded = false; // Yeni bir fight baþladýðýnda resetle
    }
}
