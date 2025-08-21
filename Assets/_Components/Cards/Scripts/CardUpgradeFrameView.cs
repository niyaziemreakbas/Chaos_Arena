using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeFrameView : MonoBehaviour
{
    CardData cardData;

    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI cooldown;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI goldAmount;

    [SerializeField] GameObject melee;
    [SerializeField] GameObject ranged;

    [SerializeField] Image cardImage;


    public void SetCardData(CardData data)
    {
        cardData = data;
        UpdateView();
    }

    public void UpdateView()
    {
        goldAmount.text = cardData.GetUpgradeCost().ToString();
        cardName.text = cardData.cardName;
        damage.text = cardData.damage.ToString();
        health.text = cardData.health.ToString();
        level.text = cardData.Level.ToString();
        cooldown.text = cardData.attackCooldown.ToString();
        speed.text = cardData.movementSpeed.ToString();

        if (cardData.range < 2)
        {
            melee.SetActive(true);
            ranged.SetActive(false);
        }
        else
        {
            melee.SetActive(false);
            ranged.SetActive(true);
        }

        cardImage.sprite = cardData.cardImage;
    }

    public void UpgradeCard()
    {
        // Animations can handle here
        cardData.UpgradeStats();
        UpdateView();
    }
}
