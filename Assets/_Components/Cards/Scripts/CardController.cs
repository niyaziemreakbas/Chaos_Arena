using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardSO cardSO;

    private CardData cardData; 
    public CardData CardData => cardData;

    private CardSlot currentSlot;
    public CardSlot CurrentSlot => currentSlot;

    private CardView cardView;
    public CardView CardView => cardView;

    private bool isEquiped;
    public bool IsEquiped => isEquiped;

    public void SetChooseState(bool state)
    {
        isEquiped = state;
    }

    private void Awake()
    {
        cardData = new CardData(cardSO);

        Initialize(cardData);
    }

    public void Initialize(CardData cardData)
    {
        cardView = GetComponent<CardView>();
        cardView.SetCardData(cardData);
    }

    public void SetCurrentSlot(CardSlot slot)
    {
        //slot != currentSlot
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform.anchoredPosition != Vector2.zero && currentSlot != null)
        {
            CardView.MoveToSlot();
        }

        currentSlot = slot;
    }

    public void UpgradeStats()
    {
        cardData.UpgradeStats();
    }
}

