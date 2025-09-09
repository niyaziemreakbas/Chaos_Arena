using System.Collections.Generic;
using UnityEngine;

public class SymmetricGridPositioner
{
    private Vector2 basePos;
    private float xSpacing;
    private float ySpacing;
    private int maxPerRow;

    private List<UnitGroupTemp> groups = new List<UnitGroupTemp>();

    public SymmetricGridPositioner(Vector2 basePos, float xSpacing, float ySpacing, int maxPerRow)
    {
        this.basePos = basePos;
        this.xSpacing = xSpacing;
        this.ySpacing = ySpacing;
        this.maxPerRow = maxPerRow;
    }

    public List<Vector2> AddUnits(UnitType type, int count)
    {
        UnitGroupTemp group = groups.Find(g => g.type == type);
        if (group == null)
        {
            group = new UnitGroupTemp(type);
            groups.Add(group);
        }

        int oldRows = group.rows;
        group.AddCount(count);

        // Eðer satýr atladýysa, tüm önündeki gruplarý ve kendisini öne kaydýr
        int newRows = Mathf.CeilToInt((float)group.totalCount / maxPerRow);
        int rowDiff = newRows - oldRows;
        if (rowDiff > 0)
        {
            ShiftGroupsForward(group, rowDiff);
        }

        RecalculateGroupPositions(group);

        return group.positions.GetRange(group.totalCount - count, count);
    }

    private void ShiftGroupsForward(UnitGroupTemp changedGroup, int rowDiff)
    {
        // Öncelik küçük = önde
        groups.Sort((g1, g2) => g1.type.priority.CompareTo(g2.type.priority));
        foreach (var g in groups)
        {
            if (g.type.priority <= changedGroup.type.priority)
            {
                // Tüm pozisyonlarý öne kaydýr
                for (int i = 0; i < g.positions.Count; i++)
                    g.positions[i] += new Vector2(0, rowDiff * ySpacing);
            }
        }
    }

    private void RecalculateGroupPositions(UnitGroupTemp group)
    {
        group.ClearPositions();
        group.rows = Mathf.CeilToInt((float)group.totalCount / maxPerRow);

        int placed = 0;
        for (int row = 0; row < group.rows; row++)
        {
            int unitsThisRow = Mathf.Min(maxPerRow, group.totalCount - placed);
            group.positions.AddRange(CalculateRow(unitsThisRow, row, group));
            placed += unitsThisRow;
        }
    }

    private List<Vector2> CalculateRow(int count, int rowIndex, UnitGroupTemp group)
    {
        List<Vector2> rowPositions = new List<Vector2>();
        Vector2 rowBase = new Vector2(basePos.x, basePos.y - rowIndex * ySpacing);

        rowPositions.Add(rowBase);
        int placed = 1, step = 1;

        while (placed < count)
        {
            if (placed < count)
            {
                rowPositions.Add(rowBase + new Vector2(step * xSpacing, 0));
                placed++;
            }
            if (placed < count)
            {
                rowPositions.Add(rowBase + new Vector2(-step * xSpacing, 0));
                placed++;
            }
            step++;
        }

        return rowPositions;
    }
}
