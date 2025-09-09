using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitGroupTemp
{
    public UnitType type;           // Bu grubun tipi
    public int totalCount = 0;      // Toplam birim say�s�
    public List<Vector2> positions; // Pozisyonlar
    public int rows = 0;            // Sat�r say�s�
    public Color debugColor;        // Debug i�in renk

    public UnitGroupTemp(UnitType type)
    {
        this.type = type;
        positions = new List<Vector2>();
        debugColor = type.debugColor;
    }

    public void AddCount(int count)
    {
        totalCount += count;
    }

    public void ClearPositions()
    {
        positions.Clear();
        rows = 0;
    }
}
