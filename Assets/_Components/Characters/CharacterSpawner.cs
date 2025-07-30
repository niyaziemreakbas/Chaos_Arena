using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{

    [SerializeField] private float spawnInterval = 3f;

    [SerializeField] private GameObject charPrefab;


    [SerializeField] private List<CharacterData> selectedCharacters;

    private List<CharacterData> characterQueue = new List<CharacterData>();
    private Dictionary<string, List<GameObject>> unitGroups = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, Color> unitColors = new Dictionary<string, Color>();

    [Header("Reposition Settings")]
    [SerializeField] private int maxUnitsPerRow = 8;
    [SerializeField] private Transform spawnOrigin;
    [SerializeField] private float spacingX = 2f;
    [SerializeField] private float spacingY = 3f;


    private void Awake()
    {
        if (selectedCharacters == null || selectedCharacters.Count == 0)
        {
            selectedCharacters = new List<CharacterData>();
            foreach (var card in CardSelectManager.Instance.SelectedCards)
            {
                selectedCharacters.Add(new CharacterData(card));
            }
        }
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

            if (selectedCharacters.Count == 0)
                continue;

            CharacterData randomChar = selectedCharacters[Random.Range(0, selectedCharacters.Count)];
            int count = Random.Range(1, 4); // 1-3 arasý rastgele sayýda karakter spawn et

            SpawnCharacter(randomChar,count);
            RepositionCharacters();
        }
    }

    public void SpawnCharacter(CharacterData data, int count)
    {
        string key = data.charName;

        // Renk atamasý yoksa oluþtur
        if (!unitColors.ContainsKey(key))
            unitColors[key] = new Color(Random.value, Random.value, Random.value);

        // Önceden characterQueue'da yoksa ekle
        if (!characterQueue.Exists(c => c.charName == key))
        {
            int insertIndex = GetOrdinalIndex(data);
            characterQueue.Insert(insertIndex, data);
        }

        // Grup yoksa oluþtur
        if (!unitGroups.ContainsKey(key))
            unitGroups[key] = new List<GameObject>();

        // Karakterleri instantiate et, ama pozisyon vermeden
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(charPrefab);
            obj.name = key;
            obj.GetComponent<SpriteRenderer>().color = unitColors[key];
            unitGroups[key].Add(obj);
        }
    }

    public void RepositionCharacters()
    {
        foreach (var kvp in unitGroups)
        {
            string key = kvp.Key;
            List<GameObject> group = kvp.Value;

            int groupYIndex = characterQueue.FindIndex(c => c.charName == key);

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
        for (int i = 0; i < characterQueue.Count; i++)
        {
            if (charData.priorityLevel < characterQueue[i].priorityLevel)
            {
                return i;
            }
        }

        return characterQueue.Count;
    }
}
