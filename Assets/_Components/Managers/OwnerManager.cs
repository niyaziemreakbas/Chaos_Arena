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

        ////Instantiate(playerPrefab, gameObject.transform);
        //Instantiate(AIPrefab, gameObject.transform);
    }
}
