using UnityEngine;

[System.Serializable]
public class UnitType
{
    public string typeName;
    public GameObject prefab;
    public int priority; // K���k = �nde, b�y�k = arkada
    public Color debugColor = Color.white; // Debug i�in renk
}
