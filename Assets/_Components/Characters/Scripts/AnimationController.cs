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

    Sequence currentSequence;

    void Start()
    {
        HandleState();
    }

    Sequence PlayIdleAnimation()
    {
        float scaleAmount = 0.02f;
        float timeAmount = 0.38f;
        float frontRotateAmount = 4f;
        float backRotateAmount = 3f;

        var rotateSequence = DOTween.Sequence();

        rotateSequence.Append(transform.DORotate(new Vector3(0, 0, -frontRotateAmount), timeAmount).SetEase(Ease.Linear));
        rotateSequence.Append(transform.DORotate(new Vector3(0, 0, 0), timeAmount).SetEase(Ease.Linear));
        rotateSequence.Append(transform.DORotate(new Vector3(0, 0, backRotateAmount), timeAmount).SetEase(Ease.Linear));
        rotateSequence.Append(transform.DORotate(new Vector3(0, 0, 0), timeAmount).SetEase(Ease.Linear));

        var scaleSequence = DOTween.Sequence();

        float localScale = transform.localScale.y;
        scaleSequence.Append(transform.DOScaleY(localScale - scaleAmount, timeAmount).SetEase(Ease.Linear));
        scaleSequence.Append(transform.DOScaleY(localScale, timeAmount).SetEase(Ease.Linear));
        scaleSequence.Append(transform.DOScaleY(localScale - scaleAmount, timeAmount).SetEase(Ease.Linear));
        scaleSequence.Append(transform.DOScaleY(localScale, timeAmount).SetEase(Ease.Linear));

        var masterSequence = DOTween.Sequence();
        masterSequence.Append(rotateSequence);
        masterSequence.Join(scaleSequence);

        masterSequence.SetLoops(-1);

        return masterSequence;
    }

    Sequence PlayRunAnimation()
    {
        float jumpAmount = 0.01f;
        float timeAmount = 0.3f;
        var rotateSequence = DOTween.Sequence();

        rotateSequence.Append(transform.DOLocalMove(new Vector3(transform.position.x, transform.position.y+jumpAmount , transform.position.z), timeAmount).SetLoops(-1, LoopType.Yoyo));

        //float frontRotateAmount = 4f;
        //float backRotateAmount = 3f;

        //rotateSequence.Append(transform.DORotate(new Vector3(0, 0, -frontRotateAmount), timeAmount).SetEase(Ease.Linear));
        //rotateSequence.Append(transform.DORotate(new Vector3(0, 0, 0), timeAmount).SetEase(Ease.Linear));
        //rotateSequence.Append(transform.DORotate(new Vector3(0, 0, backRotateAmount), timeAmount).SetEase(Ease.Linear));
        //rotateSequence.Append(transform.DORotate(new Vector3(0, 0, 0), timeAmount).SetEase(Ease.Linear));
        //rotateSequence.SetLoops(-1);

        return rotateSequence;
    }

    public void ChangeState(CharacterState newState)
    {
        if(newState == currentState)
            return;

        currentState = newState;
        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                currentSequence = null;
                currentSequence = PlayIdleAnimation();
                break;
            case CharacterState.Move:
                currentSequence = null;
                currentSequence = PlayRunAnimation();
                break;
            default:
                print("Not handled");
                break;
        }

    }

    //private IEnumerator FlipRoutine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(directionChangeInterval);

    //        if (UnityEngine.Random.value < flipChance)
    //        {
    //            // Yönü tersine çevir
    //            isLookingRight = !isLookingRight;

    //            float targetYRotation = isLookingRight ? 0f : 180f;

    //            // Dönüþ animasyonu
    //            charModel.DORotate(new Vector3(0f, targetYRotation, 0f), 0.5f)
    //                .SetEase(Ease.InOutSine);
    //        }
    //    }
    //}

    //public IEnumerator SwordSwing()
    //{
    //    yield return new WaitForSeconds(0.1f); // Küçük bir gecikme ekleyelim
    //                                           // Ýlk açýdan -swingAngle/2'ye, sonra +swingAngle/2'ye ve tekrar baþlangýca dön
    //    Sequence swingSequence = DOTween.Sequence();

    //    swingSequence.Append(swordModel.transform.DOLocalRotate(new Vector3(0, 0, -swingAngle / 2), swingDuration / 2)
    //        .SetEase(Ease.OutQuad));
    //    swingSequence.Append(swordModel.transform.DOLocalRotate(new Vector3(0, 0, swingAngle / 2), swingDuration)
    //        .SetEase(Ease.InOutQuad));
    //    swingSequence.Append(swordModel.transform.DOLocalRotateQuaternion(initialRotation, swingDuration / 2)
    //        .SetEase(Ease.InQuad));
    //}
}
