using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : SingletonMonoBehaviour<CharacterSpawner>
{
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private GameObject charPrefab;

    [Header("Reposition Settings")]
    [SerializeField] private int maxUnitsPerRow = 6;
    [SerializeField] private float spacingX = 0.2f;
    [SerializeField] private float spacingY = 3f;

    private void OnEnable()
    {
        UpgradeManager.OnSpawnCharacter += SpawnCharacter;
    }

    private void OnDisable()
    {
        UpgradeManager.OnSpawnCharacter -= SpawnCharacter;
    }

    public void SpawnCharacter(CharacterData data, int count, Owner owner)
    {
        var mgr = owner.UnitRegistry;
        string key = data.charName;

        // Sýra kaydý
        if (!mgr.CharacterOrder.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data, owner);
            mgr.CharacterOrder.Insert(insertIndex, data);
        }

        // Grup listesi
        if (!mgr.UnitGroups.ContainsKey(key))
            mgr.UnitGroups[key] = new List<GameObject>();

        // Parent kontrolü ve oluþturma
        if (!mgr.UnitParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");
            parentObj.transform.parent = owner.charsRoot; // istersen sahne kökü yapabilirsin
            mgr.UnitParents[key] = parentObj.transform;
        }

        // Karakterleri oluþtur
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(charPrefab);
            obj.GetComponent<Character>().SetCharData(data);
            Transform child = obj.transform.Find("CharModel");
            child.GetComponent<SpriteRenderer>().sprite = data.charImage;
            obj.name = key;
            //obj.GetComponent<SpriteRenderer>().color = mgr.UnitColors[key];

            // Parent’a ata
            obj.transform.parent = mgr.UnitParents[key];

            mgr.UnitGroups[key].Add(obj);
        }

        RepositionCharacters(owner);

        //print($"CharacterSpawner: Spawned {count} units of {key} at {mgr.UnitParents[key].position} with color {mgr.UnitColors[key]}");
    }

    public void RepositionCharacters(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        foreach (var kvp in mgr.UnitGroups)
        {
            string key = kvp.Key;
            List<GameObject> group = kvp.Value;

            int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
            float baseY = owner.spawnOrigin.position.y + groupYIndex * spacingY;

            for (int i = 0; i < group.Count; i++)
            {
                int total = group.Count;
                int leftCount = total / 2;
                int rightCount = total / 2;

                // If odd, assign the extra unit randomly
                if (total % 2 != 0)
                {
                    if (Random.value < 0.5f)
                        leftCount++;
                    else
                        rightCount++;
                }

                int indexInGroup = i;
                bool placeLeft = indexInGroup < leftCount;

                int indexInSide = placeLeft ? indexInGroup : indexInGroup - leftCount;

                float xOffset = spacingX * (indexInSide); // +1 to avoid spawnOrigin overlap
                xOffset = placeLeft ? -xOffset : xOffset;

                Vector3 newPos = new Vector3(owner.spawnOrigin.position.x + xOffset, baseY, 0);
                group[i].transform.position = newPos;
            }
        }
    }

    public int GetOrdinalIndex(CharacterData charData, Owner owner)
    {
        var mgr = owner.UnitRegistry;

        for (int i = 0; i < mgr.CharacterOrder.Count; i++)
        {
            if (charData.priorityLevel < mgr.CharacterOrder[i].priorityLevel)
            {
                return i;
            }
        }

        return mgr.CharacterOrder.Count;
    }
}
