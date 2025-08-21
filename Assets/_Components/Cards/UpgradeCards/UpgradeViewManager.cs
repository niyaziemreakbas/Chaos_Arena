using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeViewManager : MonoBehaviour
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
        List<UpgradeCardData> selectedCards = new List<UpgradeCardData>();

        if (!upgradeCardsPanel.activeSelf)
        {
            upgradeCardsPanel.SetActive(true);
        }

        // DIAGNOSTIC
        if (UpgradeCardManager.Instance == null) Debug.LogError("CardUpgradeManager.Instance == null");
        if (OwnerManager.Instance == null) Debug.LogError("OwnerManager.Instance == null");
        if (OwnerManager.Instance != null && OwnerManager.Instance.PlayerOwner == null) Debug.LogError("PlayerOwner == null");
        if (upgradeCards == null) Debug.LogError("upgradeCards == null");
        else if (upgradeCards.Count == 0) Debug.LogWarning("upgradeCards.Count == 0");
        selectedCards = UpgradeCardManager.Instance.ReturnRandomUpgradeList(OwnerManager.Instance.PlayerOwner, upgradeCards.Count);

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            upgradeCards[i].GetComponent<UpgradeCard>().SetUpgradeCard(selectedCards[i]);
        }
    }

    public void HideUpgradeCards()
    {
        if (upgradeCardsPanel.activeInHierarchy)
        {
            upgradeCardsPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Upgrade cards panel is already closed!");
        }
    }
}
