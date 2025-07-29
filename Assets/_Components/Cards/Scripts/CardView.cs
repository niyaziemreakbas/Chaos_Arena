using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private CardData cardData;

    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI range;
    [SerializeField] Image cardImage;

    public void SetCardData(CardData data)
    {
        if (cardData != null)
            cardData.onStatsChanged -= UpdateStatsUI;

        cardData = data;

        cardData.onStatsChanged += UpdateStatsUI; // ?? C# event baðlantýsý
        UpdateStatsUI();
    }

    private void OnDestroy()
    {
        if (cardData != null)
            cardData.onStatsChanged -= UpdateStatsUI;
    }

    void UpdateStatsUI()
    {
        cardName.text = cardData.cardName;
        damage.text = cardData.damage.ToString();

        range.text = cardData.range < 2 ? "Melee" : "Ranged";
        cardImage.sprite = cardData.cardImage;
    }
}

