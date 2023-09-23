using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler
{
    RectTransform rectTransform;
    Vector2 minPosition;
    Vector2 maxPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        rectTransform.anchoredPosition = Vector3.zero;
        CalculateDragLimits();
    }

    void CalculateDragLimits()
    {
        RectTransform canvasRect = rectTransform.root.GetComponent<RectTransform>();
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        Vector2 minCanvasPos = canvasRect.InverseTransformPoint(canvasCorners[0]);
        Vector2 maxCanvasPos = canvasRect.InverseTransformPoint(canvasCorners[2]);

        Vector2 halfSize = rectTransform.sizeDelta * 0.5f;
        minPosition = minCanvasPos + halfSize;
        maxPosition = maxCanvasPos - halfSize;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 anchoredPosition = rectTransform.anchoredPosition + eventData.delta;
        anchoredPosition = new Vector2(
            Mathf.Clamp(anchoredPosition.x, minPosition.x, maxPosition.x),
            Mathf.Clamp(anchoredPosition.y, minPosition.y, maxPosition.y)
        );

        rectTransform.anchoredPosition = anchoredPosition;
    }


}
