using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
    public static Rect ToScreenSpace(this Bounds bounds, Camera camera)
    {
        var WorldToCameraMin = camera.WorldToScreenPoint(bounds.min);
        var WorldToCameraMax = camera.WorldToScreenPoint(bounds.max);

        var x = WorldToCameraMin.x;
        var y = Screen.height - WorldToCameraMax.y;
        var width = WorldToCameraMax.x - WorldToCameraMin.x;
        var height = WorldToCameraMax.y - WorldToCameraMin.y;

        Rect ScreenSpaceObjectRect = new Rect(x, y, width, height);
                                             
        return ScreenSpaceObjectRect;
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
    public List<GameObject> SelectableObjects = new List<GameObject>();

    void Awake()
    {
    }

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

        foreach (var go in SelectableObjects)
        {
            var WorldBounds = go.GetComponent<BoxCollider>().bounds;
            var ScreenSpaceObjectRect = WorldBounds.ToScreenSpace(PlayerCamera);

            GUI.Box(ScreenSpaceObjectRect, go.name);

            #region DEBUG
            //Debug.LogFormat("{0}", go.name);
            //Debug.LogFormat("    World Position: {0}", go.transform.position);
            //Debug.LogFormat("    World Bounds: {0}", WorldBounds.ToString());
            //Debug.LogFormat("    World Bounds Min: {0}", WorldBounds.min.ToString());
            //Debug.LogFormat("    World Bounds Max: {0}", WorldBounds.max.ToString());
            //Debug.LogFormat("    Camera World to Screen Min: {0}", WorldToCameraMin);
            //Debug.LogFormat("    Camera World to Screen Max: {0}", WorldToCameraMax);
            #endregion

        }
    }

    public List<GameObject> FindGameObjectWithType<T>()
    {
        GameObject[] AllGameObjects = FindObjectsOfType<GameObject>();
        List<GameObject> ObjectsWithType = new List<GameObject>();

        for (int i = 0; i < AllGameObjects.Length; i++)
        {
            if (AllGameObjects[i].GetComponent<T>() != null)
            {
                ObjectsWithType.Add(AllGameObjects[i]);        
            }
        }

        return ObjectsWithType;
    }

    public bool HandleSelectionDrag()
    {
        bool bDragging = false;

        if (Input.GetMouseButtonDown(0))
        {
            DragStartPosition = Input.mousePosition;
            DragEndPositon = DragStartPosition;
            bDrawSelectionRectangle = true;
            FindGameObjectWithType<ISelectable>();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            bDrawSelectionRectangle = false;
            SelectionRectangle.Set(0, 0, 0, 0);
        }
        else if (Input.GetMouseButton(0))
        {
            SelectableObjects = FindGameObjectWithType<ISelectable>();

            DragEndPositon = Input.mousePosition;
            SelectionRectangle.Set(DragStartPosition.x,
                                   Screen.height - DragStartPosition.y,
                                   DragEndPositon.x - DragStartPosition.x,
                                   -1 * ((Screen.height - DragStartPosition.y) - (Screen.height - DragEndPositon.y)));

            Ray RayStartDrag = PlayerCamera.ScreenPointToRay(DragStartPosition);
            Debug.DrawRay(RayStartDrag.origin, RayStartDrag.direction * 100, Color.red, 1f, false);

            Ray RayEndDrag = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(RayEndDrag.origin, RayEndDrag.direction * 100, Color.green, .15f, false);

            bDragging = true;
        }

        return bDragging;
    }

    public void ZoomCamera()
    {
        var ScrollWheenl = Input.GetAxis("Mouse ScrollWheel");

        if (ScrollWheenl != 0)
        {
            PlayerCamera.transform.position += PlayerCamera.transform.forward * ScrollWheenl * ZoomSpeed * Time.deltaTime;
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
