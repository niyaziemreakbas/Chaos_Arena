using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class DOTMovement : MonoBehaviour
{
    [Header("Character Model")]
    [SerializeField] Transform CharModel; 
    Vector2 initialLocalPosition;

    [Header("Flip")]
    public float flipChance = 0.3f;
    public float directionChangeInterval = 2.5f;
    private bool isLookingRight = true;

    [Header("Jump")]
    public float jumpHeight = 0.3f; // Z�plama y�ksekli�i
    public float jumpDuration = 0.002f; // Z�plama s�resi (yukar� + a�a�� toplam)

    private Vector3 initialPosition;

    private Sequence movementSequence;

    public enum CharacterState { Idle, Move, Attack }

    private CharacterState currentState;
    private Tween currentTween;

    void Start()
    {
        initialLocalPosition = CharModel.localPosition;
        PlayRunAnimation();
        StartCoroutine(FlipRoutine());
        //SetState(CharacterState.Idle);

        //print(gameObject.name + " : Character initialized in state: " + currentState);
    }

    public void SetState(CharacterState newState)
    {
        //if (newState == currentState)
        //    return;

        // �nce �nceki animasyonu iptal et
        //currentTween?.Kill();

        currentState = newState;

        switch (currentState)
        {
            case CharacterState.Idle:
                //currentTween = PlayIdleAnimation();
                break;
            case CharacterState.Move:
                //currentTween = PlayRunAnimation();
                break;
            case CharacterState.Attack:
                currentTween = PlayAttackAnimation();
                break;
        }
    }

    void PlayIdleAnimation()
    {
        Debug.Log("Idle animation started");

        Sequence idleSequence = DOTween.Sequence();

        //idleSequence.Join(transform.DORotate(new Vector3(-30f, 0f, 0f), 1f)
        //                 .SetLoops(-1, LoopType.Yoyo)
        //                 .SetEase(Ease.InOutSine)
        //                 .SetRelative(false)); // Absolute hedef

        //// Z rotasyonu: 10 ile -10 aras�nda gidip gelir
        //idleSequence.Join(transform.DORotate(new Vector3(0f, 0f, -4f), 1f)
        //         .From(new Vector3(0f, 0f, 4f))
        //         .SetLoops(-1, LoopType.Yoyo)
        //         .SetEase(Ease.InOutSine)
        //         .SetRelative(false)); // Absolute hedef



        //return scaleTween; // sadece biri kontrol i�in yeter
    }


    void PlayRunAnimation()
    {

        // Yukar� z�plama ve geri d�nme hareketi, loop ile devam eder
        CharModel.DOLocalMoveY(initialLocalPosition.y + jumpHeight, jumpDuration / 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);

        //if(UnityEngine.Random.value < flipChance)
        //{
        //    transform.DORotate(new Vector3(0f, -180f, 0f), 0.7f)
        //        .SetLoops(-1, LoopType.Yoyo)
        //        .SetEase(Ease.InOutSine);
        //}
    }

    private IEnumerator FlipRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);

            if (UnityEngine.Random.value < flipChance)
            {
                // Y�n� tersine �evir
                isLookingRight = !isLookingRight;

                float targetYRotation = isLookingRight ? 0f : 180f;

                // D�n�� animasyonu
                CharModel.DORotate(new Vector3(0f, targetYRotation, 0f), 0.5f)
                    .SetEase(Ease.InOutSine);
            }
        }
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
