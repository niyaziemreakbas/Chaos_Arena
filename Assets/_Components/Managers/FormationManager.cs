using System.Collections.Generic;
using UnityEngine;

public static class FormationManager
{
    [Header("Reposition Settings")]
    [SerializeField] static float spacingX = 0.5f;

    [SerializeField] static float inGroupSpacingY = 0.7f;

    [SerializeField] static float bwGroupSpacingY = 1f;

    public static void Reposition(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        Vector3 baseTransform = owner.spawnOrigin.position;

        float direction = owner.IsUpward ? 1f : -1f;

        // priority sýralama
        mgr.UnitGroups.Sort((a, b) => b.priorityLevel.CompareTo(a.priorityLevel));

        foreach (var group in mgr.UnitGroups)
        {
            int rowCount = PositionGroup(group, baseTransform, owner, spacingX, inGroupSpacingY);

            baseTransform.y += direction * (((rowCount - 1) * inGroupSpacingY) + bwGroupSpacingY);
        }
    }

    static int PositionGroup(UnitGroup group, Vector3 baseTransform, Owner owner, float spacingX, float spacingY)
    {
        List<GameObject> units = group.unitList;

        int maxPerRow = group.characterData.maxUnitsPerRow;

        int rowCount = Mathf.CeilToInt((float)units.Count / maxPerRow);

        float direction = owner.IsUpward ? 1f : -1f;

        Vector3 startPos = baseTransform;
        startPos.y += direction * (rowCount - 1) * spacingY;

        for (int i = 0; i < units.Count; i++)
        {
            int row = i / maxPerRow;
            int col = i % maxPerRow;

            int unitsInRow = Mathf.Min(maxPerRow, units.Count - row * maxPerRow);

            float rowWidth = (unitsInRow - 1) * spacingX;

            Vector3 pos = new Vector3(
                startPos.x - rowWidth / 2 + col * spacingX,
                startPos.y - direction * row * spacingY,
                startPos.z
            );

            units[i].transform.position = pos;
        }

        return rowCount;
    }
}