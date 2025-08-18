using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private float pulseScale = 1.2f; // Ne kadar büyüyecek
    [SerializeField] private float duration = 0.5f;   // Animasyon süresi

    private Tween pulseTween;
    private Vector3 initialScale;

    private void OnEnable()
    {
        initialScale = transform.localScale;

        // Sonsuz döngü ile ölçek deðiþimi
        pulseTween = transform.DOScale(initialScale * pulseScale, duration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine); // Daha yumuþak geçiþ
    }

    private void OnDestroy()
    {
        pulseTween?.Kill();
    }
}
