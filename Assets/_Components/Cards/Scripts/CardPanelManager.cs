using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPanelManager : SingletonMonoBehaviour<CardPanelManager>
{
    CardController equipedCard;
    public CardController EquipedCard => equipedCard;

    CardState currentState = CardState.Idle;

    public static Action<CardController> OnSwapCompleted;

    public static Action OnEmptySpaceClicked;

    public static Action<CardState> OnStateChanged;

    [SerializeField] private Button EmptySpaceButton;

    [SerializeField] private GameObject Collection;

    [SerializeField] private GameObject CardSlot;

    [SerializeField] private GameObject UpgradePopUpFrame;

    private CardUpgradeFrameView upgradeFrameView;
    
    private ColorBlock originalColors;

    [SerializeField] GameObject FadePanel;

    [SerializeField] GameObject EmptySpaceClick;

    List<CardSlot> slots = new List<CardSlot>();
    public List<CardSlot> Slots => slots;

    List<CardController> cards = new List<CardController>();
    public List<CardController> Cards => cards;


    protected override void ChildAwake()
    {
        InitializeSlotsAndCards();
    }

    private void Start()
    {
        EmptySpaceButton.onClick.AddListener(() =>
        {
            EmptySpaceButtonHandle();
        });

        upgradeFrameView = UpgradePopUpFrame.GetComponent<CardUpgradeFrameView>();

        originalColors = EmptySpaceButton.colors;

    }

    public void EmptySpaceButtonHandle()
    {
        if (currentState == CardState.Upgrading)
        {
            FadePanel.SetActive(false);
            EmptySpaceClick.transform.SetAsFirstSibling();
            print("Upgrade frame clicked, closing it now.");
            UpgradePopUpFrame.SetActive(false);
            ChangeState(CardState.Idle);
            return;
        }

        print("Empty space clicked");
        UpgradePopUpFrame.SetActive(false);
        ChangeState(CardState.Idle);
        if (equipedCard != null)
        {
            UnEquipCard(equipedCard);
        }
    }

    public void InitializeSlotsAndCards()
    {
        slots.AddRange(FindObjectsOfType<CardSlot>());

        foreach(var slot in slots)
        {
            cards.Add(slot.GetComponentInChildren<CardController>());
        }

        print("Initialized card slots with number of : " + slots.Count);
    }

    public void SwapCards(CardController UpComingCard, CardController OnDeckCard)
    {
        UpComingCard.SetChooseState(false);
        ChangeState(CardState.Idle);

        CardSlot slot1 = UpComingCard.CurrentSlot;
        CardSlot slot2 = OnDeckCard.CurrentSlot;

        print($"Swapping cards: {UpComingCard.CardData.cardName} + {UpComingCard.CurrentSlot.name} with {OnDeckCard.CardData.cardName} + {OnDeckCard.CurrentSlot.name}");

        slot1.SetCurrentCard(OnDeckCard);

        slot2.SetCurrentCard(UpComingCard);

        //OnSwapCompleted?.Invoke(OnDeckCard);


        print($"Swapped cards: {UpComingCard.CardData.cardName} {UpComingCard.CurrentSlot.name} with {OnDeckCard.CardData.cardName} {OnDeckCard.CurrentSlot.name}");
    }

    public void HandleCardEquipClicked(CardView clickedCard)
    {
        CardController clickedController = clickedCard.GetComponent<CardController>();

        EquipCard(clickedController);
    }

    public void EquipCard(CardController card)
    {
        print($"Equipping card: {card.CardData.cardName}");
        equipedCard = card;
        card.SetChooseState(true);
        ChangeState(CardState.Equipped);
    }

    public void UnEquipCard(CardController card)
    {
        card.CardView.TogglePopUpFrame();
        equipedCard.SetChooseState(false);
        equipedCard = null;
        ChangeState(CardState.Idle);
    }

    public void HandleCardClicked(CardView clickedCard)
    {
        CardController clickedController = clickedCard.GetComponent<CardController>();

        if (equipedCard == null)
        {
            ChangeState(CardState.Idle);
            clickedController.CardView.TogglePopUpFrame();
        }

        // If the clicked card same with the equiped card, equip it
        else if (equipedCard.gameObject == clickedCard.gameObject)
        {
            UnEquipCard(clickedController);
        }

        // If another card is selected, swap it with the clicked card
        else if (equipedCard != null && equipedCard.gameObject != clickedCard.gameObject)
        {
            SwapCards(clickedController, equipedCard);
            UnEquipCard(equipedCard);
        }
    }

    void HandleStatesOnCards()
    {
        if(currentState == CardState.Idle)
        {
            foreach(var slot in slots)
            {
                slot.gameObject.SetActive(true);
                slot.SetCurrentCard(slot.CurrentCard);
                slot.CurrentCard.CardView.UpdateStatsUI();
            }
        }

        else if (currentState == CardState.Equipped)
        {
            foreach (var card in cards)
            {
                if (card.IsEquiped)
                {
                    card.GetComponent<CardView>().SetEquiped();
                    card.CurrentSlot.gameObject.SetActive(false);
                }
                else if (card.CompareTag("Collection"))
                {
                    card.CurrentSlot.gameObject.SetActive(false);
                }
            }
        }

        else if (currentState == CardState.Upgrading)
        {
            var cb = EmptySpaceButton.colors;
            cb.normalColor = new Color(0f, 0f, 0f, 0.5f);  // siyah yarý saydam
            cb.highlightedColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            cb.pressedColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            cb.selectedColor = cb.highlightedColor;
            cb.disabledColor = cb.normalColor;
            EmptySpaceButton.colors = cb;

            UpgradePopUpFrame.SetActive(true);
        }
    }

    public void HandleSlotClicked(CardSlot cardSlot)
    {
        print($"Slot clicked: {cardSlot.name}");
        if (equipedCard != null)
        {
            cardSlot.SetCurrentCard(equipedCard);
            equipedCard.SetCurrentSlot(cardSlot);
            equipedCard = null;
            ChangeState(CardState.Idle);
        }
    }

    public void ChangeState(CardState state)
    {
        //if(currentState == state)
        //{
        //    print($"State is already {state}, no change needed.");
        //    return;
        //}

        currentState = state;

        HandleStatesOnCards();
    }

    public void ShowElementOnFirst(Transform transform)
    {
        transform.SetParent(MenuController.Instance.cardsPanel.transform);
        transform.SetAsLastSibling();
    }

    public void RemoveCardFromDeck(CardController card)
    {

        //GameObject newSlot = Instantiate(CardSlot, Collection.transform);
        //newSlot.GetComponentInChildren<CardController>().Initialize(card.CardData);
        //card.CurrentSlot.ClearCurrentCard();
        //Destroy(card.gameObject);
    }

    public void UpgradeFrameClicked(CardData card)
    {
        ShowElementOnFirst(FadePanel.transform);
        FadePanel.SetActive(true);
        EmptySpaceClick.transform.SetAsLastSibling();

        ChangeState(CardState.Upgrading);
        ShowElementOnFirst(UpgradePopUpFrame.transform);
        upgradeFrameView.SetCardData(card);
        UpgradePopUpFrame.SetActive(true);
    }

    private void OnEnable()
    {
        CardView.OnCardEquipped += HandleCardEquipClicked;
        CardView.OnCardClicked += HandleCardClicked;
        CardView.OnCardUpgradeClicked += UpgradeFrameClicked;
    }

    private void OnDisable()
    {
        CardView.OnCardEquipped -= HandleCardEquipClicked;
        CardView.OnCardClicked -= HandleCardClicked;
        CardView.OnCardUpgradeClicked -= UpgradeFrameClicked;
    }
}
public enum CardState
{
    Idle,
    Equipped,
    Upgrading
}