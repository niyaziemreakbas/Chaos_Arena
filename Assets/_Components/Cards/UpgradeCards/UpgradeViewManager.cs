using DG.Tweening;
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
        PlayerOwner.OnUpgradeViewHandle += InitializeUpgradeCards;
        PlayerOwner.OnFightViewHandle += CloseUpgradePanel;
        PlayerOwner.OnCardSelectionHandle += HideUpgradeCards;
    }

    private void OnDisable()
    {
        PlayerOwner.OnUpgradeViewHandle -= InitializeUpgradeCards;
        PlayerOwner.OnFightViewHandle -= CloseUpgradePanel;
        PlayerOwner.OnCardSelectionHandle -= HideUpgradeCards;
    }

    // Displays upgrade cards with random character data and upgrade types
    private void InitializeUpgradeCards()
    {
        List<UpgradeCardData> selectedCards = new List<UpgradeCardData>();

        //// DIAGNOSTIC
        //if (UpgradeCardManager.Instance == null) Debug.LogError("CardUpgradeManager.Instance == null");
        //if (OwnerManager.Instance == null) Debug.LogError("OwnerManager.Instance == null");
        //if (OwnerManager.Instance != null && OwnerManager.Instance.PlayerOwner == null) Debug.LogError("PlayerOwner == null");
        //if (upgradeCards == null) Debug.LogError("upgradeCards == null");
        //else if (upgradeCards.Count == 0) Debug.LogWarning("upgradeCards.Count == 0");
        selectedCards = UpgradeCardManager.Instance.ReturnRandomUpgradeList(OwnerManager.Instance.PlayerOwner, upgradeCards.Count);

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            // Get the view component attached to the UI object
            UpgradeCardView cardView = upgradeCards[i].GetComponent<UpgradeCardView>();

            // Get the corresponding data model
            UpgradeCardData cardData = selectedCards[i];

            // Create the controller and inject dependencies
            UpgradeCardController cardController = new UpgradeCardController(cardData, cardView, OwnerManager.Instance.PlayerOwner);

            // Trigger the calculation and UI update
            cardController.InitializeCard();
        }

        //if (!upgradeCardsPanel.activeSelf)
        //{
        //    upgradeCardsPanel.SetActive(true);
        //}

        ShowUpgradeCards();
    }

    public void CloseUpgradePanel()
    {
        upgradeCardsPanel.SetActive(false);
    }

    public void HideUpgradeCards()
    {
        if (upgradeCardsPanel.activeInHierarchy)
        {
            foreach (var card in upgradeCards)
            {
                if (card.activeSelf)
                {
                    card.GetComponent<UpgradeCardView>().AnimateOut();
                }
            }
        }
        else
        {
            Debug.Log("Upgrade cards panel is already closed!");
        }
    }

    public void ShowUpgradeCards()
    {
        if (upgradeCardsPanel.activeInHierarchy)
        {
            foreach (var card in upgradeCards)
            {
                card.SetActive(true);
                card.GetComponent<UpgradeCardView>().AnimateIn();

            }
        }
        else
        {
            Debug.Log("Upgrade cards panel is already closed!");
        }
    }
}
