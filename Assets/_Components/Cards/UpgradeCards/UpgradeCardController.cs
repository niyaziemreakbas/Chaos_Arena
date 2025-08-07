using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCardController : MonoBehaviour, IPointerClickHandler
{
    public static event Action<UpgradeCardData> OnUpgradeCardClicked;

    UpgradeCardView upgradeCardView;

    UpgradeCardData upgradeCardData;
    public UpgradeCardData UpgradeCardData => upgradeCardData;

    private void Awake()
    {
        upgradeCardView = GetComponent<UpgradeCardView>();
        upgradeCardView.SetUpgradeCard(upgradeCardData);
    }

    public void SetUpgradeCard(UpgradeCardData upgradeCardData)
    {
        this.upgradeCardData = upgradeCardData;
        upgradeCardView.SetUpgradeCard(upgradeCardData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("UpgradeCardController: OnPointerClick called for " + upgradeCardData?.charName);
        if (upgradeCardData != null)
        {
            OnUpgradeCardClicked?.Invoke(upgradeCardData);
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
