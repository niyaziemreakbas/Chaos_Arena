using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
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

    private void Awake()
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
        //SetButtonActive(battleButton);
    }

    private void SetPanelActive(GameObject activePanel, Button button)
    {
        cardsPanel.SetActive(activePanel == cardsPanel);
        battlePanel.SetActive(activePanel == battlePanel);
        shopPanel.SetActive(activePanel == shopPanel);

       // SetButtonActive(button);
    }

    private void SetButtonActive(Button button)
    {
        // Tüm butonlarý varsayýlan renge ayarla
        cardImage.color = darkColor;
        battleImage.color = darkColor;
        shopImage.color = darkColor;
        // Aktif butonu açýk renge ayarla
        button.GetComponent<Image>().color = lightColor;

        cardsButtonRect.sizeDelta = new Vector2(180f, cardsButtonRect.sizeDelta.y);
        battleButtonRect.sizeDelta = new Vector2(180f, battleButtonRect.sizeDelta.y);
        shopButtonRect.sizeDelta = new Vector2(180f, shopButtonRect.sizeDelta.y);

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(190f, button.GetComponent<RectTransform>().sizeDelta.y);
    }
}
