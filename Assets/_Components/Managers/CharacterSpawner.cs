using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : SingletonMonoBehaviour<CharacterSpawner>
{
    [Header("Reposition Settings")]
    private float spacingX = 0.5f;
    private float inGroupSpacingY = 0.7f;

    private float bwGroupSpacingY = 1f;

    private void OnEnable()
    {
        UpgradeCardManager.OnSpawnCharacter += SpawnCharacter;
    }

    private void OnDisable()
    {
        UpgradeCardManager.OnSpawnCharacter -= SpawnCharacter;
    }

    public void SpawnCharacter(CharacterData data, int count, Owner owner)
    {
        var mgr = owner.UnitRegistry;
        string key = data.charName;

        // Add character data to spawned list 
        if (!mgr.SpawnedCharData.Exists(c => c.charName == key))
        {
            mgr.SpawnedCharData.Add(data);
        }

        // Generate unit group if not exists
        if (!mgr.HasUnitGroup(key))
            mgr.AddUnitGroup(new UnitGroup(key, data.priorityLevel, data));

        // Parent kontrolü ve oluşturma
        if (!mgr.UnitGroupParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");
            parentObj.transform.parent = owner.charsRoot; // istersen sahne kökü yapabilirsin
            mgr.UnitGroupParents[key] = parentObj.transform;
        }

        // Karakterleri oluştur
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(data.charPrefab);

            int randomNumber = Random.Range(100, 1000);

            // Objenin ismini randomla
            obj.name = $"{data.charPrefab.name}_{randomNumber}";

            obj.GetComponent<Character>().Initialize(OwnerManager.Instance.GetEnemyOwner(owner), owner, data);

            obj.tag = owner.OwnerName;

            // Parent’a ata
            obj.transform.parent = mgr.UnitGroupParents[key];

            // Oluşturulan birimi gruba ekle
            mgr.ReturnUnitGroup(key).AddUnit(obj);

            mgr.SpawnedCharacters.Add(obj);

        }

        RepositionGroups(owner);
    }

    public void RepositionGroups(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        Vector3 baseTransform = owner.spawnOrigin.transform.position;

        mgr.UnitGroups.Sort((a, b) => b.priorityLevel.CompareTo(a.priorityLevel));

        foreach (var UnitGroup in mgr.UnitGroups)
        {
            int rowCount = RepositionCharacters(UnitGroup, baseTransform, owner);

            float direction = owner.IsUpward ? 1f : -1f;

            baseTransform = new Vector3(
                baseTransform.x,
                baseTransform.y + direction * (((rowCount - 1) * inGroupSpacingY) + bwGroupSpacingY),
                baseTransform.z
            );
        }

        ResetChars(owner);
    }

    public int RepositionCharacters(UnitGroup unitGroup, Vector3 baseTransform, Owner owner)
    {
        List<GameObject> units = unitGroup.unitList;
        int maxPerRow = unitGroup.characterData.maxUnitsPerRow;

        int rowCount = Mathf.CeilToInt((float)units.Count / maxPerRow);

        float direction = owner.IsUpward ? 1f : -1f;

        Vector3 startPos = new Vector3(
            baseTransform.x,
            baseTransform.y + direction * (rowCount - 1) * inGroupSpacingY,
            baseTransform.z
        );

        for (int i = 0; i < units.Count; i++)
        {
            int row = i / maxPerRow;
            int col = i % maxPerRow;

            int unitsInThisRow = Mathf.Min(maxPerRow, units.Count - row * maxPerRow);
            float rowWidth = (unitsInThisRow - 1) * spacingX;

            Vector3 newPos = new Vector3(
                startPos.x - rowWidth / 2 + col * spacingX,
                startPos.y - direction * row * inGroupSpacingY,
                startPos.z
            );

            units[i].transform.position = newPos;
        }

        return rowCount;
    }

    private void ResetChars(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        foreach (var character in mgr.SpawnedCharacters)
        {
            character.GetComponent<Character>().ResetChar();
        }
    }

    public int GetOrdinalIndex(CharacterData charData, Owner owner)
    {
        var mgr = owner.UnitRegistry;

        for (int i = 0; i < mgr.SpawnedCharData.Count; i++)
        {
            if (charData.priorityLevel < mgr.SpawnedCharData[i].priorityLevel)
            {
                return i;
            }
        }

        return mgr.SpawnedCharData.Count;
    }

    public void ActivateAllIfInactive(List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
            }
        }
    }
}
