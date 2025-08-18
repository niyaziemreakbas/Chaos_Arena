using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerViewManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeCardsPanel;

    public List<GameObject> upgradeCards = new List<GameObject>();

    private void OnEnable()
    {
        PlayerOwner.OnUpgradeViewHandle += ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle += HideUpgradeCards;
        UpgradeCard.OnUpgradeCardClickedForUI += HideUpgradeCards;

    }

    private void OnDisable()
    {
        PlayerOwner.OnUpgradeViewHandle -= ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle -= HideUpgradeCards;
        UpgradeCard.OnUpgradeCardClickedForUI -= HideUpgradeCards;
    }

    // Displays upgrade cards with random character data and upgrade types
    private void ShowCardUpgradeProp()
    {
        print("Showing upgrade cards...");

        List<UpgradeCardData> selectedCards = new List<UpgradeCardData>();

        if (!upgradeCardsPanel.activeSelf)
        {
            upgradeCardsPanel.SetActive(true);
        }

        selectedCards = UpgradeManager.Instance.ReturnRandomUpgradeList(OwnerManager.Instance.PlayerOwner, upgradeCards.Count);

        print($"Selected count : {selectedCards.Count} upgrade card count: {upgradeCards.Count}");

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            upgradeCards[i].GetComponent<UpgradeCard>().SetUpgradeCard(selectedCards[i]);
        }
    }

    public void HideUpgradeCards()
    {
        print("Hiding upgrade cards...");
        if (upgradeCardsPanel.activeInHierarchy)
        {
            upgradeCardsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Upgrade cards panel is not assigned!");
        }
    }
}
