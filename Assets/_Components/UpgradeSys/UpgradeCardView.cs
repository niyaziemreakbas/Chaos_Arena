using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardView : MonoBehaviour
{
    private UpgradeCardData upgradeCard;

    [SerializeField] TextMeshProUGUI charName;

    [SerializeField] Image charImage;

    [SerializeField] TextMeshProUGUI upgradeType;

    public void SetUpgradeCard(UpgradeCardData upgradeCard)
    {
        this.upgradeCard = upgradeCard;
        UpdateUpgradeUI();
    }

    private void UpdateUpgradeUI()
    {
        if (upgradeCard == null)
        {
            Debug.LogWarning("UpgradeCard is not set.");
            return;
        }

        charName.text = upgradeCard.charName;
        charImage.sprite = upgradeCard.charImage;
        upgradeType.text = upgradeCard.upgradeType.ToString();
    }
}
