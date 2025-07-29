using UnityEngine;
using DG.Tweening;

public class TargetMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float arrivalThreshold = 0.1f;

    [SerializeField] private Transform target;

    private Tween moveTween;

    public bool HasTarget => target != null;

    void Update()
    {
        if (target == null)
            return;

        // Eðer zaten hedefe yakýnsak gitmeye gerek yok
        if (Vector3.Distance(transform.position, target.position) <= arrivalThreshold)
        {
            PlayIdle(target,
                baseScale: Vector3.one,
                scaleAmount: new Vector3(0.1f, 0.1f, 0.1f),
                scaleDuration: 0.5f,
                tiltAngle: 10f,
                tiltDuration: 0.5f);
            return;
        }
        // Aktif hareket varsa boz
        if (moveTween == null || !moveTween.IsActive())
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        moveTween?.Kill();

        float distance = Vector3.Distance(transform.position, target.position);
        float duration = distance / moveSpeed;

        // Kavisli bir zýplama animasyonu gibi
        moveTween = transform.DOJump(target.position, 0.5f, 6, duration)
            .SetEase(Ease.OutSine);
    }

    private Tween scaleTween;
    private Tween tiltTween;

    // Tek fonksiyon, deðiþkenlerle
    public void PlayIdle(Transform target, Vector3 baseScale, Vector3 scaleAmount, float scaleDuration, float tiltAngle, float tiltDuration)
    {
        scaleTween?.Kill();
        tiltTween?.Kill();

        Vector3 targetScale = baseScale + scaleAmount;

        scaleTween = target.DOScale(targetScale, scaleDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        tiltTween = target.DORotate(new Vector3(0, 0, tiltAngle), tiltDuration)
            .SetRelative()
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopIdle()
    {
        scaleTween?.Kill();
        tiltTween?.Kill();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        MoveToTarget();
    }

    public void ClearTarget()
    {
        target = null;
        moveTween?.Kill();
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
    }
}
