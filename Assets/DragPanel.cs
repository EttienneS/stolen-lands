using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;

    private Vector2 pointerOffset;

    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null)
            return;

        var pointerPostion = ClampToWindow(data);

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition
        ))
        {
            panelRectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position,
            data.pressEventCamera, out pointerOffset);
    }

    private void Awake()
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;
            panelRectTransform = transform.parent as RectTransform;
        }
    }

    private Vector2 ClampToWindow(PointerEventData data)
    {
        var rawPointerPosition = data.position;

        var canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        var clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        var clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

        var newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }
}