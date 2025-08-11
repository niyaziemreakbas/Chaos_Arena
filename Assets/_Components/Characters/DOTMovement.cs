using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

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
    public float jumpHeight = 0.3f; // Zýplama yüksekliði
    public float jumpDuration = 0.002f; // Zýplama süresi (yukarý + aþaðý toplam)

    [Header("Idle")]
    private Tween xTween;
    private Tween zTween;
    [SerializeField] private float xTarget = -30f;
    [SerializeField] private float zFrom = 4f;
    [SerializeField] private float zTo = -4f;
    [SerializeField] private float duration = 1f;

    private Vector3 initialPosition;

    private Sequence movementSequence;

    public enum CharacterState { Idle, Move, Attack }

    private CharacterState currentState;
    public CharacterState CurrentState => currentState;

    Tween currentTween;

    void Start()
    {
        initialLocalPosition = CharModel.localPosition;
        //PlayIdleAnimation();
        //PlayRunAnimation();
        StartCoroutine(FlipRoutine());
        //SetState(CharacterState.Idle);

        //print(gameObject.name + " : Character initialized in state: " + currentState);
    }

    //public void SetState(CharacterState newState)
    //{
    //    if (newState == currentState)
    //        return;

    //    currentTween?.Kill();

    //    currentState = newState;

    //    switch (currentState)
    //    {
    //        case CharacterState.Idle:
    //            currentTween = PlayIdleAnimation();
    //            break;
    //        case CharacterState.Move:
    //            currentTween = PlayRunAnimation();
    //            break;
    //        case CharacterState.Attack:
    //            //currentTween = PlayAttackAnimation();
    //            break;
    //    }
    //}

    void PlayIdleAnimation()
    {
        // Ýstediðimiz baþlangýç z deðerini açýkça koy (ör. From gibi davranmasý için)
        var e = transform.localEulerAngles;
        e.z = zFrom;
        transform.localEulerAngles = e;

        xTween = DOTween.To(
            () => NormalizeAngle(transform.localEulerAngles.x),
            x => { var ev = transform.localEulerAngles; ev.x = x; transform.localEulerAngles = ev; },
            xTarget,
            duration
        ).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        zTween = DOTween.To(
            () => NormalizeAngle(transform.localEulerAngles.z),
            z => { var ev = transform.localEulerAngles; ev.z = z; transform.localEulerAngles = ev; },
            zTo,
            duration
        ).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    float NormalizeAngle(float a) => Mathf.Repeat(a + 180f, 360f) - 180f;

    void PlayRunAnimation()
    {

        // Yukarý zýplama ve geri dönme hareketi, loop ile devam eder
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
                // Yönü tersine çevir
                isLookingRight = !isLookingRight;

                float targetYRotation = isLookingRight ? 0f : 180f;

                // Dönüþ animasyonu
                CharModel.DORotate(new Vector3(0f, targetYRotation, 0f), 0.5f)
                    .SetEase(Ease.InOutSine);
            }
        }
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
