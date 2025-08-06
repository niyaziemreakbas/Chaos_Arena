using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragZoneController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform swipePanel;
    [SerializeField] private float upperBound = 500f;
    [SerializeField] private float lowerBound = -500f;
    [SerializeField] private float returnSpeed = 10f;

    private Coroutine returnCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Kullanýcý input'a baþladýysa, varsa animasyonu iptal et
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Parmaðýný býraktýysa, paneli geri getir
        returnCoroutine = StartCoroutine(ReturnToBounds());
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = eventData.delta.y;
        float newY = swipePanel.localPosition.y + deltaY;

        if (newY > upperBound)
            deltaY *= 0.3f;
        else if (newY < lowerBound)
            deltaY *= 0.3f;

        swipePanel.localPosition += new Vector3(0, deltaY, 0);
    }

    private IEnumerator ReturnToBounds()
    {
        Vector3 startPos = swipePanel.localPosition;
        float clampedY = Mathf.Clamp(startPos.y, lowerBound, upperBound);
        Vector3 targetPos = new Vector3(startPos.x, clampedY, startPos.z);

        while (Vector3.Distance(swipePanel.localPosition, targetPos) > 0.1f)
        {
            swipePanel.localPosition = Vector3.Lerp(swipePanel.localPosition, targetPos, Time.deltaTime * returnSpeed);
            yield return null;
        }

        swipePanel.localPosition = targetPos;
        returnCoroutine = null;
    }
}
