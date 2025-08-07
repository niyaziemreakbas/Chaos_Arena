using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button cardsButton;
    public Button battleButton;
    public Button shopButton;

    [Header("Panels")]
    public GameObject cardsPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;

    private void Start()
    {
        cardsButton.onClick.AddListener(() => ShowPanel(cardsPanel));
        battleButton.onClick.AddListener(() => ShowPanel(battlePanel));
        shopButton.onClick.AddListener(() => ShowPanel(shopPanel));

        // Baþlangýçta sadece battle paneli açýk olsun
        ShowPanel(battlePanel);
    }

    private void ShowPanel(GameObject activePanel)
    {
        cardsPanel.SetActive(activePanel == cardsPanel);
        battlePanel.SetActive(activePanel == battlePanel);
        shopPanel.SetActive(activePanel == shopPanel);
    }
}
