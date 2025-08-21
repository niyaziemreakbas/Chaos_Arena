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

    [SerializeField] Image backGroundColor;

    [SerializeField] Image doubleImage;

    [SerializeField] Image upgradeImage;

    [SerializeField] TextMeshProUGUI upgradeType;

   // [SerializeField] GameObject starContainer;

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

        switch (upgradeCard.upgradeType)
        {
            case UpgradeType.Doubler:
                doubleImage.gameObject.SetActive(true);
                upgradeImage.gameObject.SetActive(false);
                upgradeType.text = $"x2 {upgradeCard.charData.charName}";
               // ActivateStars();
                break;
            case UpgradeType.Upgrader:
                doubleImage.gameObject.SetActive(false);
                upgradeImage.gameObject.SetActive(true);
                upgradeType.text = $"Upgrade";
              //  ActivateStars();
                break;
            case UpgradeType.Spawner:
                doubleImage.gameObject.SetActive(false);
                upgradeImage.gameObject.SetActive(false);
                upgradeType.text = $"+{upgradeCard.charData.spawnCount.ToString()} {upgradeCard.charData.charName}";
                //DeactivateStars();
                break;

            default:
                Debug.Log("Unknown upgrade type: ");
                break;
        }

        switch (upgradeCard.charData.charName)
        {
            //Green
            case "Blup":
                backGroundColor.color = new Color(0.5647f, 0.6392f, 0.7294f);
                break;
            //Orange
            case "Dino":
                backGroundColor.color = new Color(1.0f, 0.9608f, 0.1333f);
                break;
            //Red
            case "Demon":
                backGroundColor.color = new Color(0.0235f, 0.6745f, 0.9961f);
                break;
        }

        //GetComponent<Image>().color = upgradeCard.charData.charColor;
    }

    //private void ActivateStars()
    //{
    //    foreach (Transform star in starContainer.transform)
    //    {
    //        star.gameObject.SetActive(true);
    //    }
    //}
    //private void DeactivateStars()
    //{
    //    foreach (Transform star in starContainer.transform)
    //    {
    //        star.gameObject.SetActive(false);
    //    }
    //}

}
