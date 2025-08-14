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
    // Maybe we can make maxUnits for every chars in the future
    [SerializeField] private int maxUnitsPerRow = 6;
    private float spacingX = 0.5f;
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

        // S�ra kayd�
        if (!mgr.CharacterOrder.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data, owner);
            mgr.CharacterOrder.Insert(insertIndex, data);
        }

        // Grup listesi
        if (!mgr.UnitGroups.ContainsKey(key))
            mgr.UnitGroups[key] = new List<GameObject>();

        //// Parent kontrol� ve olu�turma
        if (!mgr.UnitParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");
            parentObj.transform.parent = owner.charsRoot; // istersen sahne k�k� yapabilirsin
            mgr.UnitParents[key] = parentObj.transform;
        }

        // Karakterleri olu�tur
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(data.charPrefab);

            int randomNumber = Random.Range(100, 1000);

            // Objenin ismine ekle
            obj.name = $"{data.charPrefab.name}_{randomNumber}";

            obj.GetComponent<Character>().Initialize(OwnerManager.Instance.GetEnemyOwner(owner), owner, data);

            obj.tag = owner.OwnerName;

            // Parent�a ata
            obj.transform.parent = mgr.UnitParents[key];

            mgr.UnitGroups[key].Add(obj);

            mgr.SpawnedCharacters.Add(obj);

        }

        RepositionCharacters(owner);
    }

    public void RepositionCharacters(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        foreach (var kvp in mgr.UnitGroups)
        {
            string key = kvp.Key;
            List<GameObject> group = kvp.Value;

            int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
            int totalGroups = mgr.CharacterOrder.Count;

            // baseY hesaplama: 
            // Birbirlerine bakmalar� i�in
            // isUpward true ise priority en d���k en �stte
            // isUpward false ise priority en d���k en altta
            float baseY;
            if (owner.IsUpward)
            {
                int invertedIndex = totalGroups - 1 - groupYIndex;
                baseY = owner.spawnOrigin.position.y + invertedIndex * spacingY;
            }
            else
            {
                baseY = owner.spawnOrigin.position.y + groupYIndex * spacingY;
            }

            int totalUnits = group.Count;
            int rows = Mathf.CeilToInt((float)totalUnits / maxUnitsPerRow);

            for (int row = 0; row < rows; row++)
            {
                int unitsInRow = Mathf.Min(maxUnitsPerRow, totalUnits - row * maxUnitsPerRow);

                // Sat�rdaki birimleri yatayda ortalamak i�in ba�lang�� X
                float rowWidth = (unitsInRow - 1) * spacingX;
                float startX = owner.spawnOrigin.position.x - rowWidth / 2f;

                for (int i = 0; i < unitsInRow; i++)
                {
                    int index = row * maxUnitsPerRow + i;
                    if (index >= totalUnits)
                        break;

                    float xPos = startX + i * spacingX;

                    float yPos;
                    // isUpward true ise yukar�dan a�a�� sat�rlar artacak (yani negatif y�nde)
                    if (owner.IsUpward)
                    {
                        yPos = baseY - row * spacingY;
                    }
                    else
                    {
                        // isUpward false ise a�a��dan yukar� sat�rlar artacak (pozitif y�nde)
                        yPos = baseY + row * spacingY;
                    }

                    group[index].transform.position = new Vector3(xPos, yPos, 0);
                }
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
