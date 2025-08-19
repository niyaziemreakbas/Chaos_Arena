using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : SingletonMonoBehaviour<PlayerInfo>
{
    public int gold = 500;

    public event Action OnDataChanged;

    public void SetGold(int value)
    {
        print("SetGold called with value: " + value);
        gold += value;
        OnDataChanged?.Invoke();
    }
}
