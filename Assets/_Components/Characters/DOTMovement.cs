using UnityEngine;
using DG.Tweening;
using System;

public class DOTMovement : MonoBehaviour
{
    [Header("Rotation")]
    public float flipChance = 0.3f; // %30 ihtimalle döner
    public float tiltAngle = 10f;
    public float tiltSpeed = 1f;

    private bool facingRight = true;
    private Sequence movementSequence;

    public enum CharacterState { Idle, Move, Attack }

    private CharacterState currentState;
    private Tween currentTween;

    void Start()
    {
        SetState(CharacterState.Attack);

        print(gameObject.name + " : Character initialized in state: " + currentState);
    }

    public void SetState(CharacterState newState)
    {
        //if (newState == currentState)
        //    return;

        // Önce önceki animasyonu iptal et
        //currentTween?.Kill();

        currentState = newState;

        switch (currentState)
        {
            case CharacterState.Idle:
                currentTween = PlayIdleAnimation();
                break;
            case CharacterState.Move:
                currentTween = PlayRunAnimation();
                break;
            case CharacterState.Attack:
                currentTween = PlayAttackAnimation();
                break;
        }
    }

    Tween PlayIdleAnimation()
    {
        Debug.Log("Idle animation started");

        // Scale idle efekti
        Tween scaleTween = transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        // Tilt idle efekti
        transform.DORotate(new Vector3(0f, 0f, 10f), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        return scaleTween; // sadece biri kontrol için yeter
    }


    Tween PlayRunAnimation()
    {
        return DOTween.Sequence()
        .Append(transform.DOMoveX(transform.position.x + 1f, 0.2f)
            .SetEase(Ease.Linear))
        .Join(transform.DOMoveY(transform.position.y + 0.2f, 0.1f)
            .SetEase(Ease.OutQuad));

    }

    Tween PlayAttackAnimation()
    {
        // Ýleri hafif hamle, sonra geri dön
        float attackDistance = 0.5f;
        float attackDuration = 0.2f;

        Sequence attackSequence = DOTween.Sequence();

        attackSequence.Append(transform.DOMoveX(transform.position.x + attackDistance, attackDuration).SetEase(Ease.OutQuad));
        attackSequence.Append(transform.DOMoveX(transform.position.x, attackDuration).SetEase(Ease.InQuad));

        // Tek seferlik, tamamlandýðýnda otomatik durur
        return attackSequence;
    }
}
