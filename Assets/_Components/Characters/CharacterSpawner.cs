using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private GameObject charPrefab;

    [Header("Reposition Settings")]
    [SerializeField] private int maxUnitsPerRow = 6;
    [SerializeField] private float spacingX = 2f;
    [SerializeField] private float spacingY = 3f;

    [Header("Player Spawn Origins")]
    [SerializeField] private Transform playerSpawnOrigin;
    [SerializeField] private Transform playerChars;

    [Header("Enemy Spawn Origins")]
    [SerializeField] private Transform enemySpawnOrigin;
    [SerializeField] private Transform enemyChars;


    private void OnEnable()
    {
        CardUpgradeManager.OnSpawnCharacter += SpawnCharacter;
    }

    private void OnDisable()
    {
        CardUpgradeManager.OnSpawnCharacter -= SpawnCharacter;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        print("CharacterSpawner: Starting spawn routine...");
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            print("sayýsýý :: " + CharacterManager.Instance.SelectedCharacters.Count);

            if (CharacterManager.Instance.SelectedCharacters.Count == 0)
                continue;

            CharacterData randomChar = CharacterManager.Instance.SelectedCharacters[Random.Range(0, CharacterManager.Instance.SelectedCharacters.Count)];
            int count = Random.Range(1, 4);

            SpawnCharacter(randomChar, count);
            RepositionCharacters();
        }
    }

    public void SpawnCharacter(CharacterData data, int count)
    {
        var mgr = CharacterManager.Instance;
        string key = data.charName;

        // Renk kaydý
        if (!mgr.UnitColors.ContainsKey(key))
            mgr.UnitColors[key] = new Color(Random.value, Random.value, Random.value);

        // Sýra kaydý
        if (!mgr.CharacterOrder.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data);
            mgr.CharacterOrder.Insert(insertIndex, data);
        }

        // Grup listesi
        if (!mgr.UnitGroups.ContainsKey(key))
            mgr.UnitGroups[key] = new List<GameObject>();

        // Parent kontrolü ve oluþturma
        if (!mgr.UnitParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");
            parentObj.transform.parent = playerChars; // istersen sahne kökü yapabilirsin
            mgr.UnitParents[key] = parentObj.transform;
        }

        // Karakterleri oluþtur
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(charPrefab);
            obj.GetComponent<Character>().SetCharData(data);
            obj.name = key;
            obj.GetComponent<SpriteRenderer>().color = mgr.UnitColors[key];

            // Parent’a ata
            obj.transform.parent = mgr.UnitParents[key];

            mgr.UnitGroups[key].Add(obj);
        }
    }

    public void RepositionCharacters()
    {
        var mgr = CharacterManager.Instance;

        foreach (var kvp in mgr.UnitGroups)
        {
            string key = kvp.Key;
            List<GameObject> group = kvp.Value;

            int groupYIndex = mgr.CharacterOrder.FindIndex(c => c.charName == key);
            float baseY = playerSpawnOrigin.position.y + groupYIndex * spacingY;

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

                Vector3 newPos = new Vector3(playerSpawnOrigin.position.x + xOffset, baseY, 0);
                group[i].transform.position = newPos;
            }
        }
    }

    public int GetOrdinalIndex(CharacterData charData)
    {
        var mgr = CharacterManager.Instance;

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
