using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeFrameView : MonoBehaviour
{
    CardData cardData;

    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI damage;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI cooldown;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI goldAmount;

    [SerializeField] GameObject melee;
    [SerializeField] GameObject ranged;

    [SerializeField] Image cardImage;

    [SerializeField] float duration = 0.6f;
    [SerializeField] float delay = 0f;
    [SerializeField] Ease easeType = Ease.OutBack;

    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    private Vector3 originalPos;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPos = transform.localPosition;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetCardData(CardData data)
    {
        cardData = data;
        UpdateView();
    }

    public void UpdateView()
    {
        PlayIntro();

        goldAmount.text = cardData.GetUpgradeCost().ToString();
        cardName.text = cardData.cardName;
        damage.text = cardData.damage.ToString();
        health.text = cardData.health.ToString();
        level.text = cardData.Level.ToString();
        cooldown.text = cardData.attackCooldown.ToString();
        speed.text = cardData.movementSpeed.ToString();

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

        cardImage.sprite = cardData.cardImage;
    }

    public void PlayIntro()
    {
        // Baþlangýç deðerlerini ayarla
        transform.localScale = Vector3.zero;
        transform.localPosition = originalPos + new Vector3(0, -50f, 0); // aþaðýdan gelsin
        canvasGroup.alpha = 0f;

        // Animasyon sequence
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMove(originalPos, duration).SetEase(Ease.OutCubic));
        seq.Join(transform.DOScale(originalScale, duration).SetEase(easeType));
        seq.Join(canvasGroup.DOFade(1f, duration * 0.8f));

        seq.SetDelay(delay);
        seq.Play();
    }

    public void PlayOutro(System.Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(originalPos + new Vector3(0, -50f, 0), duration).SetEase(Ease.InCubic));
        seq.Join(transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0f, duration * 0.8f));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false); // Ýstersen Destroy(gameObject) yap
        });

        seq.Play();
    }

    public void UpgradeCard()
    {
        // Animations can handle here
        cardData.UpgradeStats();
        UpdateView();
    }
}
