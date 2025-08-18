using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private float pulseScale = 1.2f; // Ne kadar b�y�yecek
    [SerializeField] private float duration = 0.5f;   // Animasyon s�resi

    private Tween pulseTween;
    private Vector3 initialScale;

    private void OnEnable()
    {
        initialScale = transform.localScale;

        // Sonsuz d�ng� ile �l�ek de�i�imi
        pulseTween = transform.DOScale(initialScale * pulseScale, duration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine); // Daha yumu�ak ge�i�
    }

    private void OnDestroy()
    {
        pulseTween?.Kill();
    }
}
