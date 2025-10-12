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
    private float spacingY = 0.8f;

    //private float groupSpacingX = 1f;
    private float groupSpacingY = 1.5f;

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

            // Objenin ismini randomla
            obj.name = $"{data.charPrefab.name}_{randomNumber}";

            obj.GetComponent<Character>().Initialize(OwnerManager.Instance.GetEnemyOwner(owner), owner, data);

            obj.tag = owner.OwnerName;

            // Parent’a ata
            obj.transform.parent = mgr.UnitParents[key];

            // Oluşturulan birimi gruba ekle
            mgr.ReturnUnitGroup(key).AddUnit(obj);

            mgr.SpawnedCharacters.Add(obj);

        }

        RepositionGroups(owner);
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

    //public void RepositionCharacters(Owner owner)
    //{
    //    var mgr = owner.UnitRegistry;

    //    foreach (var kvp in mgr.UnitGroups)
    //    {
    //        string key = kvp.Key;
    //        List<GameObject> group = kvp.Value;

    //        int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
    //        int totalGroups = mgr.CharacterOrder.Count;

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

    //            float rowWidth = (unitsInRow - 1) * spacingX;
    //            float startX = owner.spawnOrigin.position.x - rowWidth / 2f;

    //            for (int i = 0; i < unitsInRow; i++)
    //            {
    //                int index = row * maxUnitsPerRow + i;
    //                if (index >= totalUnits)
    //                    break;

    //                float xPos = startX + i * spacingX;
    //                float yPos = owner.IsUpward
    //                    ? baseY - row * spacingY
    //                    : baseY + row * spacingY;

    //                var obj = group[index];
    //                obj.transform.position = new Vector3(xPos, yPos, 0);

    //                // GridPos'u Character component'te tut
    //                var character = obj.GetComponent<Character>();
    //                if (character != null)
    //                {
    //                    character.GridPos = new Vector2Int(i, row);
    //                }
    //            }
    //        }
    //    }

    //    ResetChars(owner);
    //}

    public void RepositionGroups(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        Vector3 baseTransform = owner.spawnOrigin.transform.position;

        if (owner.IsUpward)
        {
            // Bigger to smaller
            mgr.UnitGroups.Sort((a, b) => b.priorityLevel.CompareTo(a.priorityLevel));
        }
        else
        {
            // Smaller to bigger
            mgr.UnitGroups.Sort((a, b) => a.priorityLevel.CompareTo(b.priorityLevel));
        }


        foreach (var UnitGroup in mgr.UnitGroups)
        {
            RepositionCharacters(UnitGroup, baseTransform);

            // Calculate LimitY according to space between groups and rows 
            baseTransform = new Vector3(
                baseTransform.x,
                baseTransform.y + (UnitGroup.rowCount - 1) * spacingY + groupSpacingY,
                baseTransform.z
            );
        }

        ResetChars(owner);
    }

    public void RepositionCharacters(UnitGroup unitGroup, Vector3 baseTransform)
    {
        List<GameObject> units = unitGroup.unitList;
        CharacterData data = unitGroup.characterData;
        float maxPerRow = data.maxUnitsPerRow;

        Vector3 startPos = baseTransform;

        for (int i = 0; i < units.Count; i++)
        {
            int row = i / (int)maxPerRow;       // Hangi satır
            int col = i % (int)maxPerRow;       // Hangi sütun

            // Centering lines
            int unitsInThisRow = Mathf.Min((int)maxPerRow, units.Count - row * (int)maxPerRow);
            float rowWidth = (unitsInThisRow - 1) * spacingX;

            Vector3 newPos = new Vector3(
                startPos.x - rowWidth / 2 + col * spacingX,  // merkezleme
                startPos.y - row * spacingY,                // alt satır için Y ekseni
                startPos.z
            );

            units[i].transform.position = newPos;
        }

        //unitGroup.rowCount = totalRows;
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
