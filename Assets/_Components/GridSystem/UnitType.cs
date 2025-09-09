using UnityEngine;

[System.Serializable]
public class UnitType
{
    public string typeName;
    public GameObject prefab;
    public int priority; // Küçük = önde, büyük = arkada
    public Color debugColor = Color.white; // Debug için renk
}
