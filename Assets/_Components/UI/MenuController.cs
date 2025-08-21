using FurtleGame.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : SingletonMonoBehaviour<MenuController>
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [SerializeField] Color lightColor;
    [SerializeField] Color darkColor;

    [Header("Buttons")]
    public Button cardsButton;
    public Button battleButton;
    public Button shopButton;

    [Header("Panels")]
    public GameObject cardsPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;

    private Image cardImage;
    private Image battleImage;
    private Image shopImage;

    private RectTransform cardsButtonRect;
    private RectTransform battleButtonRect;
    private RectTransform shopButtonRect;

    protected override void ChildAwake()
    {
        cardImage = cardsButton.GetComponent<Image>();
        battleImage = battleButton.GetComponent<Image>();
        shopImage = shopButton.GetComponent<Image>();

        cardsButtonRect = cardsButton.GetComponent<RectTransform>();
        battleButtonRect = battleButton.GetComponent<RectTransform>();
        shopButtonRect = shopButton.GetComponent<RectTransform>();
    }

    private void Start()
    {
        cardsButton.onClick.AddListener(() => SetPanelActive(cardsPanel, cardsButton));
        battleButton.onClick.AddListener(() => SetPanelActive(battlePanel, battleButton));
        shopButton.onClick.AddListener(() => SetPanelActive(shopPanel, shopButton));

        // Baþlangýçta sadece battle paneli açýk olsun
        SetPanelActive(battlePanel, battleButton);
    }

    private void SetPanelActive(GameObject activePanel, Button button)
    {
        cardsPanel.SetActive(activePanel == cardsPanel);
        battlePanel.SetActive(activePanel == battlePanel);
        shopPanel.SetActive(activePanel == shopPanel);

        SetButtonActive(button);
    }

    private void SetButtonActive(Button activeButton)
    {
        // Boyut deðerleri
        Vector2 inactiveSize = new Vector2(350f, 180f);
        Vector2 activeSize = new Vector2(400f, 200f);

        // Hepsini inaktif yap
        cardImage.sprite = inactiveSprite;
        battleImage.sprite = inactiveSprite;
        shopImage.sprite = inactiveSprite;

        cardImage.color = darkColor;
        battleImage.color = darkColor;
        shopImage.color = darkColor;

        cardsButtonRect.sizeDelta = inactiveSize;
        battleButtonRect.sizeDelta = inactiveSize;
        shopButtonRect.sizeDelta = inactiveSize;

        // Aktif olaný özel ayarla
        Image activeImage = activeButton.GetComponent<Image>();
        activeImage.sprite = activeSprite;
        activeImage.color = lightColor;

        RectTransform activeRect = activeButton.GetComponent<RectTransform>();
        activeRect.sizeDelta = activeSize;
    }
}
