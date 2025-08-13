using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class AnimationController : MonoBehaviour
{
    [Header("Sword Animation")]
    [SerializeField] Transform swordModel;
    [SerializeField] float swingDuration = 0.3f;
    [SerializeField] float swingAngle = 90f;
    private Quaternion initialRotation;

    [Header("Character Model")]
    Transform charModel;
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

        charModel = GetComponent<Character>().CharModel;

        initialLocalPosition = charModel.localPosition;
        initialRotation = swordModel.localRotation;



        //PlayIdleAnimation();
        //PlayRunAnimation();
        //StartCoroutine(SwordSwing());
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
        charModel.DOLocalMoveY(initialLocalPosition.y + jumpHeight, jumpDuration / 2)
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
                charModel.DORotate(new Vector3(0f, targetYRotation, 0f), 0.5f)
                    .SetEase(Ease.InOutSine);
            }
        }
    }

    public IEnumerator SwordSwing()
    {
        yield return new WaitForSeconds(0.1f); // Küçük bir gecikme ekleyelim
                                               // Ýlk açýdan -swingAngle/2'ye, sonra +swingAngle/2'ye ve tekrar baþlangýca dön
        Sequence swingSequence = DOTween.Sequence();

        swingSequence.Append(swordModel.transform.DOLocalRotate(new Vector3(0, 0, -swingAngle / 2), swingDuration / 2)
            .SetEase(Ease.OutQuad));
        swingSequence.Append(swordModel.transform.DOLocalRotate(new Vector3(0, 0, swingAngle / 2), swingDuration)
            .SetEase(Ease.InOutQuad));
        swingSequence.Append(swordModel.transform.DOLocalRotateQuaternion(initialRotation, swingDuration / 2)
            .SetEase(Ease.InQuad));
    }
}
