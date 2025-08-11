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

    [SerializeField] Image doubleImage;

    [SerializeField] Image upgradeImage;

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

        switch (upgradeCard.upgradeType.ToString())
        {
            case "Doubler":
                doubleImage.gameObject.SetActive(true);
                upgradeImage.gameObject.SetActive(false);
                break;
            case "Upgrader":
                doubleImage.gameObject.SetActive(false);
                upgradeImage.gameObject.SetActive(true);
                break;
            default:
                //Debug.Log("Unknown upgrade type: " + upgradeCard.upgradeType);
                doubleImage.gameObject.SetActive(false);
                upgradeImage.gameObject.SetActive(false);
                break;
        }

        GetComponent<Image>().color = upgradeCard.charData.charColor;
    }
}
