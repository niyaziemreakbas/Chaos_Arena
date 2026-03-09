using FurtleGame.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : SingletonMonoBehaviour<CharacterSpawner>
{
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

        RegisterCharacterData(mgr, data);

        UnitGroup group = GetOrCreateGroup(mgr, key, data);

        Transform parent = GetOrCreateParent(mgr, owner, key);

        SpawnUnits(count, data, owner, group, parent, mgr);

        FormationManager.Reposition(owner);

        ResetChars(owner);
    }

    void RegisterCharacterData(UnitRegistry mgr, CharacterData data)
    {
        if (!mgr.SpawnedCharData.Exists(c => c.charName == data.charName))
            mgr.SpawnedCharData.Add(data);
    }

    UnitGroup GetOrCreateGroup(UnitRegistry mgr, string key, CharacterData data)
    {
        if (!mgr.HasUnitGroup(key))
            mgr.AddUnitGroup(new UnitGroup(key, data.priorityLevel, data));

        return mgr.ReturnUnitGroup(key);
    }

    Transform GetOrCreateParent(UnitRegistry mgr, Owner owner, string key)
    {
        if (!mgr.UnitGroupParents.ContainsKey(key))
        {
            GameObject parentObj = new GameObject(key + "Group");

            parentObj.transform.SetParent(owner.charsRoot);

            mgr.UnitGroupParents[key] = parentObj.transform;
        }

        return mgr.UnitGroupParents[key];
    }

    void SpawnUnits(int count, CharacterData data, Owner owner, UnitGroup group, Transform parent, UnitRegistry mgr)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(data.charPrefab);

            obj.name = $"{data.charPrefab.name}_{Random.Range(100, 1000)}";

            obj.GetComponent<Character>()
               .Initialize(OwnerManager.Instance.GetEnemyOwner(owner), owner, data);

            obj.tag = owner.OwnerName;

            obj.transform.SetParent(parent);

            group.AddUnit(obj);

            mgr.SpawnedCharacters.Add(obj);
        }
    }

    void ResetChars(Owner owner)
    {
        var mgr = owner.UnitRegistry;

        foreach (var character in mgr.SpawnedCharacters)
        {
            character.GetComponent<Character>().ResetChar();
        }
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