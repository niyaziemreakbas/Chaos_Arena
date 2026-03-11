using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeViewManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeCardsPanel;

    public List<GameObject> upgradeCardsGOs = new List<GameObject>();
    private List<UpgradeCardController> cardControllers = new List<UpgradeCardController>();

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
        //// DIAGNOSTIC
        //if (UpgradeCardManager.Instance == null) Debug.LogError("CardUpgradeManager.Instance == null");
        //if (OwnerManager.Instance == null) Debug.LogError("OwnerManager.Instance == null");
        //if (OwnerManager.Instance != null && OwnerManager.Instance.PlayerOwner == null) Debug.LogError("PlayerOwner == null");
        //if (upgradeCards == null) Debug.LogError("upgradeCards == null");
        //else if (upgradeCards.Count == 0) Debug.LogWarning("upgradeCards.Count == 0");

        Debug.Log("Initializing upgrade cards...");

        if (cardControllers.Count == 0) // sadece ilk sefer
        {
            List<UpgradeCardData> selectedCards = UpgradeCardManager.Instance.ReturnRandomUpgradeList(OwnerManager.Instance.PlayerOwner, upgradeCardsGOs.Count);

            for (int i = 0; i < upgradeCardsGOs.Count; i++)
            {
                UpgradeCardView cardView = upgradeCardsGOs[i].GetComponent<UpgradeCardView>();
                UpgradeCardData cardData = selectedCards[i];

                UpgradeCardController cardController = new UpgradeCardController(cardData, cardView, OwnerManager.Instance.PlayerOwner);
                cardControllers.Add(cardController);

                cardController.InitializeCard();

                Debug.Log($"Initialized card {i} with character {cardData.charName} and upgrade type {cardData.upgradeType}");
            }
        }
        else
        {
            // kartlarý yeniden seç, controller update et
            List<UpgradeCardData> selectedCards = UpgradeCardManager.Instance.ReturnRandomUpgradeList(OwnerManager.Instance.PlayerOwner, upgradeCardsGOs.Count);

            for (int i = 0; i < cardControllers.Count; i++)
            {
                cardControllers[i].UpdateCard(selectedCards[i]); // yeni method ekle

                Debug.Log($"Updated card {i} with character {selectedCards[i].charName} and upgrade type {selectedCards[i].upgradeType}");
            }
        }

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
            Debug.Log("Hiding upgrade cards...");
            foreach (var card in upgradeCardsGOs)
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
        upgradeCardsPanel.SetActive(true);

        Debug.Log("Showing upgrade cards...");

        foreach (var card in upgradeCardsGOs)
        {
            card.SetActive(true);
            card.GetComponent<UpgradeCardView>().AnimateIn();

        }
    }
}
