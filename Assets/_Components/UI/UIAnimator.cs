using DG.Tweening;
using UnityEngine;

public static class UIAnimator
{

    // Will be implented for UI elements that need to animate in and out.
    public static void PlayIntro(RectTransform target, float duration = 0.6f)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>() ?? target.gameObject.AddComponent<CanvasGroup>();

        Vector3 originalScale = target.localScale;
        Vector3 originalPos = target.localPosition;

        target.localScale = Vector3.zero;
        target.localPosition = originalPos + new Vector3(0, -50f, 0);
        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(target.DOLocalMove(originalPos, duration).SetEase(Ease.OutCubic));
        seq.Join(target.DOScale(originalScale, duration).SetEase(Ease.OutBack));
        seq.Join(canvasGroup.DOFade(1f, duration * 0.8f));
    }

    public static void PlayOutro(RectTransform target, float duration = 0.6f, System.Action onComplete = null)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>() ?? target.gameObject.AddComponent<CanvasGroup>();

        Vector3 originalPos = target.localPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(target.DOLocalMove(originalPos + new Vector3(0, -50f, 0), duration).SetEase(Ease.InCubic));
        seq.Join(target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0f, duration * 0.8f));

        seq.OnComplete(() => onComplete?.Invoke());
    }
}
