using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public int amount;
    public DamageType type;
    //public GameObject source; // Hasarý veren obje

    public DamageInfo(int amount, DamageType type)
    {
        this.amount = amount;
        this.type = type;
        //this.source = null; // Varsayýlan olarak null
    }
}