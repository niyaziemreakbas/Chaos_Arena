using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private CardData cardData;

    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI damage;
    //[SerializeField] TextMeshProUGUI range;
    [SerializeField] Image cardImage;

    [SerializeField] GameObject melee;
    [SerializeField] GameObject ranged;

    [SerializeField] Image backGroundColor;

    [SerializeField] GameObject popUpFrame;

    GameObject parentSlot;

    public static event Action<CardView> OnCardViewClicked;

    private void Start()
    {
        parentSlot = transform.parent.gameObject;
    }

    public void SetCardData(CardData data)
    {
        cardData = data;

        cardData.OnStatsChanged += UpdateStatsUI; // ?? C# event baðlantýsý
        UpdateStatsUI();
    }

    private void OnDestroy()
    {
        if (cardData != null)
            cardData.OnStatsChanged -= UpdateStatsUI;
    }

    void UpdateStatsUI()
    {
        cardName.text = cardData.cardName;
        damage.text = $"Damage {cardData.damage.ToString()}";

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


        switch (cardData.cardName)
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

        //range.text = cardData.range < 2 ? "Melee" : "Ranged";
        cardImage.sprite = cardData.cardImage;

        //GetComponent<Image>().color = cardData.cardColor;
    }

    public void TogglePopUpFrame()
    {
        if (popUpFrame.activeSelf)
        {
            popUpFrame.SetActive(false);
            transform.SetParent(parentSlot.transform);
        }
        else
        {
            transform.SetParent(popUpFrame.transform.root);
            transform.SetAsLastSibling();
            popUpFrame.SetActive(true);
            OnCardViewClicked?.Invoke(this);
        }
    }

    private void HandleOtherPopUpOpened(CardView sender)
    {
        if (sender != this) // baþkasý açtýysa
        {
            popUpFrame.SetActive(false);
            transform.SetParent(parentSlot.transform);
        }
    }

    private void OnEnable()
    {
        OnCardViewClicked += HandleOtherPopUpOpened;
    }

    private void OnDisable()
    {
        OnCardViewClicked -= HandleOtherPopUpOpened;
    }
}

