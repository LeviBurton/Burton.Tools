using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
    public static Rect ToScreenSpace(this Bounds Bounds, Camera Camera)
    {
        var WorldToCameraMin = Camera.WorldToScreenPoint(Bounds.min);
        var WorldToCameraMax = Camera.WorldToScreenPoint(Bounds.max);

        // For clarity.
        var x = WorldToCameraMin.x;
        var y = Screen.height - WorldToCameraMax.y;
        var width = WorldToCameraMax.x - WorldToCameraMin.x;
        var height = WorldToCameraMax.y - WorldToCameraMin.y;
                               
        return new Rect(x, y, width, height);
    }
}

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera = null;
    public Vector3 CurrentMousePosition;
    public Vector3 PreviousMousePosition;
    public float ZoomLevel;

    public float PanSpeed = 5.0f;
    public float ZoomSpeed = 5.0f;

    public bool bDrawSelectionRectangle;
    public Rect SelectionRectangle;
    public Vector3 DragStartPosition;
    public Vector3 DragEndPositon;

    public List<Rect> RectsToDraw = new List<Rect>();
    public SelectableGameObject[] SelectableObjects = null;

    void Update()
    {
        HandleSelectionDrag();
        PanCamera();
        ZoomCamera();
    }

    public void OnGUI()
    {
        if (bDrawSelectionRectangle)
        {
            GUI.Box(SelectionRectangle, "");
        }

        #region DEBUG
        //foreach (var go in SelectableObjects)
        //{
        //    var WorldBounds = go.GetComponent<Renderer>().bounds;
        //    var ScreenSpaceObjectRect = WorldBounds.ToScreenSpace(PlayerCamera);

        //    Debug.LogFormat("{0}", go.name);
        //    Debug.LogFormat("    World Position: {0}", go.transform.position);
        //    Debug.LogFormat("    World Bounds: {0}", WorldBounds.ToString());
        //    Debug.LogFormat("    World Bounds Min: {0}", WorldBounds.min.ToString());
        //    Debug.LogFormat("    World Bounds Max: {0}", WorldBounds.max.ToString());
        //    Debug.LogFormat("    Camera World to Screen Min: {0}", WorldToCameraMin);
        //    Debug.LogFormat("    Camera World to Screen Max: {0}", WorldToCameraMax);
        //    GUI.Box(ScreenSpaceObjectRect, go.name);
        //}
        #endregion
    }

    public bool HandleSelectionDrag()
    {
        bool bDragging = false;

        if (Input.GetMouseButtonDown(0))
        {
            SelectableObjects = FindObjectsOfType<SelectableGameObject>();
        
            foreach (var Object in SelectableObjects)
            {
                Object.OnDeselect();
            }

            DragStartPosition = Input.mousePosition;
            DragEndPositon = DragStartPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bDrawSelectionRectangle = false;
            SelectionRectangle.Set(0, 0, 0, 0);
        }
        else if (Input.GetMouseButton(0))
        {
            bDrawSelectionRectangle = true;
            DragEndPositon = Input.mousePosition;

            SelectionRectangle.Set(DragStartPosition.x,
                                   Screen.height - DragStartPosition.y,
                                   DragEndPositon.x - DragStartPosition.x,
                                   -1 * ((Screen.height - DragStartPosition.y) - (Screen.height - DragEndPositon.y)));

            foreach (var Obj in SelectableObjects)
            {
                // Transform the world-space bounds of our SelectableObject to a screen-space rect so we can later
                // check if our selection rectangle (which is also in screen-space) overlaps it.
                var WorldBounds = Obj.GetComponent<Renderer>().bounds;
                var ScreenSpaceObjectRect = WorldBounds.ToScreenSpace(PlayerCamera);

                // Change this to a SelectionHovered type of thing, and actually select on mouse up.
                if (SelectionRectangle.Overlaps(ScreenSpaceObjectRect, true))
                {
                    Obj.OnSelect();
                }
                else
                {
                    Obj.OnDeselect();
                }
            }

            bDragging = true;
        }

        return bDragging;
    }

    public void ZoomCamera()
    {
        var ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (ScrollWheel != 0)
        {
            PlayerCamera.transform.position += PlayerCamera.transform.forward * ScrollWheel * ZoomSpeed * Time.deltaTime;
        }
    }

    public void PanCamera()
    {
        PreviousMousePosition = CurrentMousePosition;
        CurrentMousePosition = Input.mousePosition;

        Vector3 TargetPosition = PlayerCamera.transform.position;

        if (Input.GetMouseButtonDown(2))
        {
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(2))
        {

            Cursor.visible = true;
        }
        else if (Input.GetMouseButton(2))
        {
            TargetPosition += PlayerCamera.transform.right * (CurrentMousePosition.x - PreviousMousePosition.x) * PanSpeed * -1f * Time.deltaTime;
            TargetPosition += Vector3.Cross(PlayerCamera.transform.right, Vector3.up) * (CurrentMousePosition.y - PreviousMousePosition.y) * PanSpeed * -1f * Time.deltaTime;
        }

        PlayerCamera.transform.position = TargetPosition;
    }
}
