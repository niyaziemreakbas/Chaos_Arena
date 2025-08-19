using FurtleGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerManager : SingletonMonoBehaviour<OwnerManager>
{
    //[SerializeField] GameObject playerPrefab;
    //[SerializeField] GameObject AIPrefab;

    List<Owner> owners = new List<Owner>();
    public List<Owner> Owners => owners;

    [SerializeField] public GameObject OwnerContainer;

    Owner playerOwner;
    public Owner PlayerOwner => playerOwner;

    Owner enemyOwner;
    public Owner EnemyOwner => enemyOwner;

    protected override void ChildAwake()
    {
        for (int i = 0; i < OwnerContainer.transform.childCount; i++)
        {
            Transform child = OwnerContainer.transform.GetChild(i);
            Owner ownerComponent = child.GetComponent<Owner>();
            if (ownerComponent != null)
            {
                owners.Add(ownerComponent);

                if (child.name == "PlayerOwner")
                {
                    playerOwner = ownerComponent;
                }
                else if (child.name == "EnemyOwner")
                {
                    enemyOwner = ownerComponent;
                }
            }
            else
            {
                Debug.LogWarning($"Child {child.name} does not have an Owner component.");
            }
        }
    }

    private void Start()
    {
        GenerateOwners();
    }

    private void GenerateOwners()
    {
        OwnerContainer.SetActive(true);

        foreach (var owner in owners)
        {
            owner.SetEnemyOwner(GetEnemyOwner(owner)); 
        }
    }

    public Owner GetEnemyOwner(Owner owner)
    {
        if (owner == null) return null;
        foreach (var o in owners)
        {
            if (o.OwnerName != owner.OwnerName)
            {
                //Return the owner who is not the same as the given owner
                return o;
            }
        }
        Debug.LogWarning("No enemy owner found for the given owner.");
        return null;
    }

    public IUpgradeDecisionStrategy ReturnRandomStrategy()
    {
        List<IUpgradeDecisionStrategy> strategies = new List<IUpgradeDecisionStrategy>
        {
            new MaxUnitsStrategy(),
            new RandomStrategy(),
            new RangedFocusStrategy()
        };

        int randomIndex = Random.Range(0, strategies.Count);

        print($"Selected Strategy: {strategies[randomIndex].GetType().Name}");
        return strategies[randomIndex];
    }
}
