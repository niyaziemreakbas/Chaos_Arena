using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : SingletonMonoBehaviour<CharacterSpawner>
{
    [Header("Reposition Settings")]
    // Maybe we can make maxUnits for every chars in the future
    [SerializeField] private int maxUnitsPerRow = 6;
    private float spacingX = 0.5f;
    [SerializeField] private float spacingY = 1f;

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

        // Sıra kaydı
        if (!mgr.CharacterOrder.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data, owner);
            mgr.CharacterOrder.Insert(insertIndex, data);
        }

        // Grup listesi
        if (!mgr.UnitGroups.ContainsKey(key))
            mgr.UnitGroups[key] = new List<GameObject>();

        //// Parent kontrolü ve oluşturma
        if (!mgr.UnitParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");
            parentObj.transform.parent = owner.charsRoot; // istersen sahne kökü yapabilirsin
            mgr.UnitParents[key] = parentObj.transform;
        }

        // Karakterleri oluştur
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(data.charPrefab);

            int randomNumber = Random.Range(100, 1000);

            // Objenin ismine ekle
            obj.name = $"{data.charPrefab.name}_{randomNumber}";

            obj.GetComponent<Character>().Initialize(OwnerManager.Instance.GetEnemyOwner(owner), owner, data);

            obj.tag = owner.OwnerName;

            // Parent’a ata
            obj.transform.parent = mgr.UnitParents[key];

            mgr.UnitGroups[key].Add(obj);

            mgr.SpawnedCharacters.Add(obj);

        }

        RepositionCharacters(owner);
    }

    //public void RepositionCharacters(Owner owner)
    //{
    //    var mgr = owner.UnitRegistry;

    //    foreach (var kvp in mgr.UnitGroups)
    //    {
    //        string key = kvp.Key;
    //        List<GameObject> group = kvp.Value;

    //        int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
    //        int totalGroups = mgr.CharacterOrder.Count;

    //        // baseY hesaplama: 
    //        // Birbirlerine bakmaları için
    //        // isUpward true ise priority en düşük en üstte
    //        // isUpward false ise priority en düşük en altta
    //        float baseY;
    //        if (owner.IsUpward)
    //        {
    //            int invertedIndex = totalGroups - 1 - groupYIndex;
    //            baseY = owner.spawnOrigin.position.y + invertedIndex * spacingY;
    //        }
    //        else
    //        {
    //            baseY = owner.spawnOrigin.position.y + groupYIndex * spacingY;
    //        }

    //        int totalUnits = group.Count;
    //        int rows = Mathf.CeilToInt((float)totalUnits / maxUnitsPerRow);

    //        for (int row = 0; row < rows; row++)
    //        {
    //            int unitsInRow = Mathf.Min(maxUnitsPerRow, totalUnits - row * maxUnitsPerRow);

    //            // Satırdaki birimleri yatayda ortalamak için başlangıç X
    //            float rowWidth = (unitsInRow - 1) * spacingX;
    //            float startX = owner.spawnOrigin.position.x - rowWidth / 2f;

    //            for (int i = 0; i < unitsInRow; i++)
    //            {
    //                int index = row * maxUnitsPerRow + i;
    //                if (index >= totalUnits)
    //                    break;

    //                float xPos = startX + i * spacingX;

    //                float yPos;
    //                // isUpward true ise yukarıdan aşağı satırlar artacak (yani negatif yönde)
    //                if (owner.IsUpward)
    //                {
    //                    yPos = baseY - row * spacingY;
    //                }
    //                else
    //                {
    //                    // isUpward false ise aşağıdan yukarı satırlar artacak (pozitif yönde)
    //                    yPos = baseY + row * spacingY;
    //                }

    //                group[index].transform.position = new Vector3(xPos, yPos, 0);
    //            }
    //        }
    //    }

    //    ResetChars(owner);
    //}

    public void RepositionCharacters(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        foreach (var kvp in mgr.UnitGroups)
        {
            string key = kvp.Key;
            List<GameObject> group = kvp.Value;

            int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
            int totalGroups = mgr.CharacterOrder.Count;

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

                float rowWidth = (unitsInRow - 1) * spacingX;
                float startX = owner.spawnOrigin.position.x - rowWidth / 2f;

                for (int i = 0; i < unitsInRow; i++)
                {
                    int index = row * maxUnitsPerRow + i;
                    if (index >= totalUnits)
                        break;

                    float xPos = startX + i * spacingX;
                    float yPos = owner.IsUpward
                        ? baseY - row * spacingY
                        : baseY + row * spacingY;

                    var obj = group[index];
                    obj.transform.position = new Vector3(xPos, yPos, 0);

                    // GridPos'u Character component'te tut
                    var character = obj.GetComponent<Character>();
                    if (character != null)
                    {
                        character.GridPos = new Vector2Int(i, row);
                    }
                }
            }
        }

        ResetChars(owner);
    }

    //public void RepositionCharacters(Owner owner)
    //{
    //    var mgr = owner.UnitRegistry;

    //    foreach(var character in mgr.CharacterOrder)
    //    {

    //    }

    //    ResetChars(owner);
    //}





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
