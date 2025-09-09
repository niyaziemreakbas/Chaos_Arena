using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private CardData cardData;

    [SerializeField] TextMeshProUGUI cardName;

    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI level;

    [SerializeField] Image cardImage;

    [SerializeField] GameObject melee;
    [SerializeField] GameObject ranged;

    [SerializeField] Image backGroundColor;

    [SerializeField] GameObject UpgradeEquipPopUpFrame;
    [SerializeField] GameObject EquipButton;
    [SerializeField] GameObject RemoveButton;

    [SerializeField] GameObject UpgradeFrame;

    CardController cardController;

    public static event Action<CardView> OnCardClicked;

    public static event Action<CardData> OnCardUpgradeClicked;

    public static event Action<CardView> OnCardEquipped;

    private void Start()
    {
        cardController = GetComponent<CardController>();

        UpdateStatsUI();
    }

    public void SetCardData(CardData data)
    {
        cardData = data;

        cardData.OnStatsChanged += UpdateStatsUI; // ?? C# event bağlantısı
        UpdateStatsUI();
    }

    private void OnDestroy()
    {
        if (cardData != null)
            cardData.OnStatsChanged -= UpdateStatsUI;
    }

    public void UpdateStatsUI()
    {
        cardName.text = cardData.cardName;
        damage.text = cardData.damage.ToString();
        health.text = cardData.health.ToString();
        level.text = cardData.Level.ToString();

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

        if(gameObject.CompareTag("Deck"))
        {
            EquipButton.SetActive(false);
            RemoveButton.SetActive(true);
        }
        else if (gameObject.CompareTag("Collection"))
        {
            EquipButton.SetActive(true);
            RemoveButton.SetActive(false);
        }

        UpgradeEquipPopUpFrame.SetActive(false);

        //GetComponent<Image>().color = cardData.cardColor;
    }

    //private void HandleCardClicked(CardView clickedCard)
    //{
    //    if (clickedCard == this)
    //    {
    //        TogglePopUpFrame();
    //    }
    //    else if (UpgradeEquipPopUpFrame.activeSelf)
    //    {
    //        UpgradeEquipPopUpFrame.SetActive(false);
    //        SetSlotToParent();
    //    }
    //}

    // Reference from card button
    public void OnCardClickedFunc()
    {
        OnCardClicked?.Invoke(this);
    }

    public void TogglePopUpFrame()
    {
        if (UpgradeEquipPopUpFrame.activeSelf)
        {
            UpgradeEquipPopUpFrame.SetActive(false);
            gameObject.transform.SetParent(cardController.CurrentSlot.transform);
        }
        else
        {
            CardPanelManager.Instance.ShowElementOnFirst(transform);
            UpgradeEquipPopUpFrame.SetActive(true);
        }
    }

    //private void HandleSwapCompleted(CardController swappedCardFromDeck)
    //{
    //    if (swappedCardFromDeck == cardController)
    //    {
    //        print(swappedCardFromDeck.name + " has been swapped, closing pop-up frame if open.");
    //        UpgradeEquipPopUpFrame.SetActive(false);
    //    }
    //}

    // Will be calling from every card view

    public void OnUpgradeClicked()
    {
        UpgradeEquipPopUpFrame.SetActive(false);
        OnCardUpgradeClicked?.Invoke(cardData);
    }

    public void OnEquipClicked()
    {
        UpgradeEquipPopUpFrame.SetActive(false);
        OnCardEquipped?.Invoke(this);
    }
    public void OnRemoveClicked()
    {
        print("Not implemented for now");
        //UpgradeEquipPopUpFrame.SetActive(false);
        //CardSwapManager.Instance.RemoveCardFromDeck(cardController);
    }

    //private IEnumerator EquipRoutine()
    //{
    //    // Bring card to front
    //    ShowCardOnFirst();
    //    popUpFrame.SetActive(false);

    //    // Move to center of the screen
    //    yield return transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 0.5f).WaitForCompletion();

    //    // Animation handling
    //    Tween rotateTween = transform.DORotate(new Vector3(0, 0, 10), 0.5f)
    //        .SetLoops(-1, LoopType.Yoyo)
    //        .SetEase(Ease.InOutSine);

    //    print("Card is being swapped, waiting for swap to complete...");

    //    yield return new WaitUntil(() => swappedSuccessfully || swapInterrupted);

    //    if (swapInterrupted)
    //    {
    //        CardSwapManager.Instance.ClearCard();
    //    }

    //    print("Force stop equip false.");
    //    swappedSuccessfully = false;
    //    swapInterrupted = false;
    //    transform.rotation = Quaternion.identity;

    //    // Stop Anim and go to currentSlot
    //    rotateTween.Kill();
    //    print("Card swap completed, rotation stopped.");

    //    StartCoroutine(MoveToSlot());
    //}


    public void MoveToSlot()
    {
        if (cardController.CurrentSlot != null)
        {
            StartCoroutine(MoveToSlotCoroutine());
        }
        else
        {
            print("Current slot is null, cannot move card to slot.");
        }
    }

    private IEnumerator MoveToSlotCoroutine()
    {
        print("Moving card to its slot: " + cardController.CardData.cardName);

        yield return GetComponent<RectTransform>()
            .DOAnchorPos(Vector2.zero, 0.5f)
            .WaitForCompletion();

    }

    public void SetEquiped()
    {
        UpgradeEquipPopUpFrame.SetActive(false);
        gameObject.SetActive(true); 
        CardPanelManager.Instance.ShowElementOnFirst(transform);
        transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 0.5f);
    }

}

