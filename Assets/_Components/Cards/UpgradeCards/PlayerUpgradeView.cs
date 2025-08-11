using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeView : MonoBehaviour
{
    [SerializeField] GameObject upgradeCardsPanel;

    public List<GameObject> upgradeCards = new List<GameObject>();

    private Owner playerOwner;

    private void OnEnable()
    {
        // Should be called 
        PlayerOwner.OnUpgradeViewHandle += ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle += HideUpgradeCards;
    }

    private void OnDisable()
    {
        PlayerOwner.OnUpgradeViewHandle -= ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle -= HideUpgradeCards;
    }

    private void Start()
    {
        playerOwner = OwnerManager.Instance.PlayerOwner.GetComponent<Owner>();
    }

    // Displays upgrade cards with random character data and upgrade types
    private void ShowCardUpgradeProp()
    {
        if (!upgradeCardsPanel.activeSelf)
        {
            upgradeCardsPanel.SetActive(true);
        }

        Debug.Log("ShowCardUpgradeProp called");
        //SetActive(true);
        foreach (var card in upgradeCards)
        {
            card.GetComponent<UpgradeCard>().SetUpgradeCard(UpgradeManager.Instance.SelectRandomUpgradeCard(playerOwner));
        }
    }

    public void HideUpgradeCards()
    {
        if (upgradeCardsPanel)
        {
            upgradeCardsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Upgrade cards panel is not assigned!");
        }
    }
}
