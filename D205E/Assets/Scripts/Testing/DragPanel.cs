using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 PointerOffset;
    private RectTransform CanvasRectTransform;
    private RectTransform PanelRectTransform;

    private void Awake()
    {
        Canvas Canvas = GetComponentInParent<Canvas>();
        if (Canvas != null)
        {
            CanvasRectTransform = Canvas.transform as RectTransform;

            // This is a hit zone, so the parent is who want to move.
            PanelRectTransform = transform.parent as RectTransform;
        }
    }

    public void OnPointerDown(PointerEventData Data)
    {
        // Bring us to the top.
        PanelRectTransform.SetAsLastSibling();

        // Map our pointer position to a local position within the panel, and store in PointerOffset.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(PanelRectTransform, Data.position, Data.pressEventCamera, out PointerOffset);
    }

    public void OnDrag(PointerEventData Data)
    {
        if (PanelRectTransform == null)
            return;

        Vector2 LocalPointerPosition;
        Vector2 PointerPosition = ClampToCanvas(Data);

        bool bHit = RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRectTransform, 
                                                                            PointerPosition, 
                                                                            Data.pressEventCamera, 
                                                                            out LocalPointerPosition);
        if (bHit)
        {
            PanelRectTransform.localPosition = LocalPointerPosition - PointerOffset;
        }
    }
    
    // Clamps mouse position to extents of Canvas.
    Vector2 ClampToCanvas(PointerEventData Data)
    {
        Vector2 RawPointerPosition = Data.position;
        Vector3[] CanvasCorners = new Vector3[4];

        CanvasRectTransform.GetWorldCorners(CanvasCorners);
        float ClampedX = Mathf.Clamp(RawPointerPosition.x, CanvasCorners[0].x, CanvasCorners[2].x);
        float ClampedY = Mathf.Clamp(RawPointerPosition.y, CanvasCorners[0].y, CanvasCorners[2].y);

        return new Vector2(ClampedX, ClampedY);
    }
}
