using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    public string characterName;

    public int priorityLevel;

    public float rowCount;

    public int chartCount;

    public CharacterData characterData;

    public List<GameObject> unitList = new List<GameObject>();
    
    public UnitGroup(string charName, int priority, CharacterData data)
    {
        characterName = charName;
        priorityLevel = priority;
        characterData = data;
    }

    public void AddUnit(GameObject unit)
    {
        unitList.Add(unit);
        chartCount++;
        rowCount = characterData.spawnCount / characterData.maxUnitsPerRow;
    }

    public void RepositionGroup()
    {

    }

}
