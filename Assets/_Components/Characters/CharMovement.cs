using UnityEngine;
using DG.Tweening;
using System;

public class CharMovement : MonoBehaviour
{
    [Header("Movement")]
    public float jumpDistance = 2f;
    public float jumpHeight = 1f;
    public float jumpDuration = 0.5f;
    public float jumpInterval = 1f;

    [Header("Rotation")]
    public float flipChance = 0.3f; // %30 ihtimalle d�ner
    public float tiltAngle = 10f;
    public float tiltSpeed = 1f;

    private bool facingRight = true;
    private Sequence movementSequence;

    public enum CharacterState { Idle, Move, Attack }

    private CharacterState currentState = CharacterState.Idle;
    private Tween currentTween;

    void Start()
    {
        SetState(CharacterState.Idle);
        if(CompareTag("Player"))
        {
            // Oyuncu karakteri i�in ba�lang��ta hareket etmeye ba�la
            SetState(CharacterState.Move);
        }
    }

    public void SetState(CharacterState newState)
    {
        if (newState == currentState)
            return;

        // �nce �nceki animasyonu iptal et
        currentTween?.Kill();

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
        // Hafif nefes alma efekti
        return transform.DOScale(new Vector3(1.05f, 1.05f, 1), 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    Tween PlayRunAnimation()
    {
        // �leriye do�ru s�rekli hareket (1 birim sa�a)
        return transform.DOMoveY(transform.position.y + 1, 0.5f)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    Tween PlayAttackAnimation()
    {
        // �leri hafif hamle, sonra geri d�n
        float attackDistance = 0.5f;
        float attackDuration = 0.2f;

        Sequence attackSequence = DOTween.Sequence();

        attackSequence.Append(transform.DOMoveX(transform.position.x + attackDistance, attackDuration).SetEase(Ease.OutQuad));
        attackSequence.Append(transform.DOMoveX(transform.position.x, attackDuration).SetEase(Ease.InQuad));

        // Tek seferlik, tamamland���nda otomatik durur
        return attackSequence;
    }
}
