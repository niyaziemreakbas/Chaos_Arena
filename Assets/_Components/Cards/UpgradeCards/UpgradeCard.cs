using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler
{
    public static event Action<UpgradeCardData, Owner> OnUpgradeCardClicked;
    public static event Action OnUpgradeCardClickedForUI;

    UpgradeCardView upgradeCardView;

    UpgradeCardData upgradeCardData;
    public UpgradeCardData UpgradeCardData => upgradeCardData;

    Owner playerOwner;

    private void Awake()
    {
        upgradeCardView = GetComponent<UpgradeCardView>();
        //upgradeCardView.SetUpgradeCard(upgradeCardData);
    }

    private void Start()
    {
        playerOwner = OwnerManager.Instance.PlayerOwner;
        //if (playerOwner == null)
        //{
        //    Debug.LogError("Player owner is not set in UpgradeCard!");
        //}
    }

    public void SetUpgradeCard(UpgradeCardData upgradeCardData)
    {
        this.upgradeCardData = upgradeCardData;
        upgradeCardView.SetUpgradeCard(upgradeCardData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgradeCardData != null)
        {
            OnUpgradeCardClicked?.Invoke(upgradeCardData, playerOwner);

            if (!GameStateManager.Instance.DecideCanUpgradeForOwner(playerOwner))
            {
                OnUpgradeCardClickedForUI?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("UpgradeCardData is null on click!");
        }
    }
}

public class UpgradeCardData
{
    public Sprite charImage;
    public string charName;
    public CharacterData charData;
    public UpgradeType upgradeType;
}
