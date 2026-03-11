using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class UpgradeCardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI charNameText;
    [SerializeField] private Image charImage;
    [SerializeField] private Image backGroundColor;
    [SerializeField] private Image doubleImage;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TextMeshProUGUI upgradeTypeText;

    [Header("Animation Settings")]
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] private float animDelay = 0f;
    [SerializeField] private float hoverScaleMultiplier = 1.05f;
    [SerializeField] private float animDuration = 0.5f;

    private Vector2 originalPos;
    private Vector3 originalScale;

    public Action OnViewClicked;

    UpgradeCardData upgradeCardData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        originalPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        AnimateIn();
    }

    private void OnDisable()
    {
        // Prevent DOTween memory leaks
        rectTransform.DOKill();
        canvasGroup.DOKill();
    }

    public void AnimateIn()
    {
        rectTransform.DOKill();
        canvasGroup.DOKill();

        rectTransform.anchoredPosition = new Vector2(originalPos.x, originalPos.y - 500f);
        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(originalPos, animDuration).SetEase(Ease.OutBack));
        seq.Join(canvasGroup.DOFade(1f, 0.3f));
        seq.SetDelay(animDelay);
    }

    public void AnimateOut()
    {
        rectTransform.DOKill();
        canvasGroup.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(new Vector2(originalPos.x, originalPos.y - 500f), animDuration).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0f, 0.3f));
        seq.OnComplete(() => gameObject.SetActive(false));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(originalScale * hoverScaleMultiplier, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Notify subscribers (the Controller)
        OnViewClicked?.Invoke();
    }

    public void SetData(UpgradeCardData data)
    {
        this.upgradeCardData = data;
        SetCharacterInfo(data.charData.charName, data.charData.charImage);
    }

    public void SetCharacterInfo(string name, Sprite image)
    {
        charNameText.text = name;
        charImage.sprite = image;
    }

    public void SetBackgroundColor(Color color)
    {
        backGroundColor.color = color;
    }

    public void SetUpgradeIcons(bool showDouble, bool showUpgrade)
    {
        doubleImage.gameObject.SetActive(showDouble);
        upgradeImage.gameObject.SetActive(showUpgrade);
    }

    public void SetUpgradeText(string text)
    {
        upgradeTypeText.text = text;
    }
}