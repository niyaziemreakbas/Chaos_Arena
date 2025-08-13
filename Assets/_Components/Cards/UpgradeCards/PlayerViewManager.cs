using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerViewManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeCardsPanel;

    public List<GameObject> upgradeCards = new List<GameObject>();

    private Owner playerOwner;
    private Owner enemyOwner;

    [Header("EnemyUI Components")]
    [SerializeField] TextMeshProUGUI enemyHealth;
    [SerializeField] TextMeshProUGUI enemyName;

    [Header("PlayerUI Components")]
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI playerName;

    private void OnEnable()
    {
        PlayerOwner.OnUpgradeViewHandle += ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle += HideUpgradeCards;
        FightManager.OnFightStateEnd += UpdateOwnerData;
    }

    private void OnDisable()
    {
        FightManager.OnFightStateEnd -= UpdateOwnerData; 
        PlayerOwner.OnUpgradeViewHandle -= ShowCardUpgradeProp;
        PlayerOwner.OnFightViewHandle -= HideUpgradeCards;
    }

    private void Start()
    {
        playerOwner = OwnerManager.Instance.PlayerOwner.GetComponent<Owner>();
        enemyOwner = OwnerManager.Instance.EnemyOwner.GetComponent<Owner>();
        if (playerOwner == null || enemyOwner == null)
        {
            Debug.LogError("Player or Enemy owner is not assigned!");
            return;
        }
        UpdateOwnerData();
    }

    // Displays upgrade cards with random character data and upgrade types
    private void ShowCardUpgradeProp()
    {
        if (!upgradeCardsPanel.activeSelf)
        {
            upgradeCardsPanel.SetActive(true);
        }

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

    private void UpdateOwnerData()
    {
        if (playerOwner != null || enemyOwner != null)
        {
            playerHealth.text = playerOwner.FightHealth.ToString();
            playerName.text = playerOwner.OwnerName;
            playerName.color = playerOwner.teamColor;

            enemyHealth.text = enemyOwner.FightHealth.ToString();
            enemyName.text = enemyOwner.OwnerName;
            enemyName.color = enemyOwner.teamColor;
        }
        else
        {
            Debug.LogWarning("owners is not assigned!");
        }
    }
}
