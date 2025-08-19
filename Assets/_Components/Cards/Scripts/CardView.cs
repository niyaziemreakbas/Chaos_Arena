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

    [SerializeField] GameObject popUpFrame;

    CardController cardController;

    public static event Action<CardView> OnCardClicked;

    private bool forceStopEquip = false;

    private void Start()
    {
        cardController = GetComponent<CardController>();
        //parentSlot = transform.parent.gameObject;
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

        //GetComponent<Image>().color = cardData.cardColor;
    }

    public void OnCardClickedFunction()
    {
        if(CardSwapManager.Instance.CanSwap() && gameObject.CompareTag("OnDeck"))
        {
            CardSwapManager.Instance.SwapCards(GetComponent<CardController>(), CardSwapManager.Instance.currentCard);
        }
        else
        {
            TogglePopUpFrame();
        }

       // TogglePopUpFrame();
    }

    private void TogglePopUpFrame()
    {
        if (popUpFrame.activeSelf)
        {
            popUpFrame.SetActive(false);
            SetSlotToParent();
        }
        else
        {
            ShowCardOnFirst();
            popUpFrame.SetActive(true);
            OnCardClicked?.Invoke(this);
        }
    }

    private void HandleOtherCardClicked(CardView sender)
    {
        print($"Card clicked: {sender.cardData.cardName}");
        print($"Current card: {this.cardData.cardName}");
        if (sender != this)
        {
            print("Another card clicked, closing pop-up frame if open.");
            popUpFrame.SetActive(false);
            forceStopEquip = true; // force stop equip animation
        }
    }

    public void OnEquipClicked()
    {
        CardSwapManager.Instance.SelectCurrentCard(GetComponent<CardController>());
        StartCoroutine(EquipRoutine());
    }

    private IEnumerator EquipRoutine()
    {
        // Bring card to front
        ShowCardOnFirst();
        popUpFrame.SetActive(false);

        // Move to center of the screen
        yield return transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 0.5f).WaitForCompletion();

        // Animation handling
        Tween rotateTween = transform.DORotate(new Vector3(0, 0, 10), 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        print("Card is being swapped, waiting for swap to complete...");

        yield return new WaitUntil(() => CardSwapManager.Instance.SwapCompleted || forceStopEquip);

        forceStopEquip = false;
        CardSwapManager.Instance.SwapCompleted = false;

        // Stop Anim and go to currentSlot
        rotateTween.Kill();
        transform.rotation = Quaternion.identity;
        print("Card swap completed, rotation stopped.");

        StartCoroutine(MoveToSlot());
    }

    private void SetSlotToParent()
    {
        transform.SetParent(cardController.CurrentSlot.gameObject.transform);
    }

    public IEnumerator MoveToSlot()
    {
        SetSlotToParent();

        yield return GetComponent<RectTransform>()
            .DOAnchorPos(Vector2.zero, 0.5f)
            .WaitForCompletion();
    }

    private void ShowCardOnFirst()
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    private void OnEnable()
    {
        OnCardClicked += HandleOtherCardClicked;
    }

    private void OnDisable()
    {
        OnCardClicked -= HandleOtherCardClicked;
    }
}

