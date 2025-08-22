using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI infoText;

    UpgradeCardData upgradeCard;

    public void UpdateUI()
    {
        switch (upgradeCard.upgradeType)
        {
            case UpgradeType.Doubler:
                charName.text = upgradeCard.charData.charName;
                infoText.text = $"x2 {upgradeCard.charData.charName}";
                // ActivateStars();
                break;
            case UpgradeType.Upgrader:
                charName.text = upgradeCard.charData.charName;
                infoText.text = $"Upgrade";
                //  ActivateStars();
                break;
            case UpgradeType.Spawner:
                charName.text = upgradeCard.charData.charName;
                infoText.text = $"+{upgradeCard.charData.spawnCount.ToString()} {upgradeCard.charData.charName}";
                //DeactivateStars();
                break;

            default:
                Debug.Log("Unknown upgrade type: ");
                break;
        }
    }
}
