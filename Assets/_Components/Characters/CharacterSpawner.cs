using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private GameObject charPrefab;

    [Header("Reposition Settings")]
    [SerializeField] private int maxUnitsPerRow = 8;
    [SerializeField] private Transform spawnOrigin;
    [SerializeField] private float spacingX = 2f;
    [SerializeField] private float spacingY = 3f;

    private void OnEnable()
    {
        UpgradeManager.OnSpawnCharacter += SpawnCharacter;
    }

    private void OnDisable()
    {
        UpgradeManager.OnSpawnCharacter -= SpawnCharacter;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

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

        if (!mgr.UnitColors.ContainsKey(key))
            mgr.UnitColors[key] = new Color(Random.value, Random.value, Random.value);

        if (!mgr.CharacterOrder.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data);
            mgr.CharacterOrder.Insert(insertIndex, data);
        }

        if (!mgr.UnitGroups.ContainsKey(key))
            mgr.UnitGroups[key] = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(charPrefab);
            obj.GetComponent<Character>().SetCharData(data);
            obj.name = key;
            obj.GetComponent<SpriteRenderer>().color = mgr.UnitColors[key];
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

            for (int i = 0; i < group.Count; i++)
            {
                int row = i / maxUnitsPerRow;
                int col = i % maxUnitsPerRow;

                Vector3 newPos = spawnOrigin.position
                    + new Vector3(col * spacingX, groupYIndex * spacingY - row * spacingY, 0);

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
