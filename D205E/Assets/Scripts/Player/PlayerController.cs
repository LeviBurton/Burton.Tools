﻿using Burton.Lib.StateMachine;
using Burton.Lib.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera = null;
    private Vector3 CurrentMousePosition;
    private Vector3 PreviousMousePosition;
    public float ZoomLevel;

    public float PanSpeed = 5.0f;
    public float ZoomSpeed = 5.0f;

    private bool bDrawSelectionRectangle;
    private Rect SelectionRectangle;
    private Vector3 DragStartPosition;
    private Vector3 DragEndPositon;

    private List<Rect> RectsToDraw = new List<Rect>();
    public SelectableGameObject[] SelectableObjects = null;
    public LayerMask GraphLayerMask;
    public UnityGraph UnityGraph;

    public List<SelectableGameObject> SelectedObjects = new List<SelectableGameObject>();

    public StateMachine<PlayerController> StateMachine;
    protected State_Normal State_Normal = new State_Normal();
    protected State_Encounter State_Encounter = new State_Encounter();

    public Transform HoveredNodePrefab;
    public Transform HoveredNode;

    void Start()
    {
        StateMachine = new StateMachine<PlayerController>(this);
        StateMachine.ChangeState(State_Normal);
        HoveredNode = Instantiate(HoveredNodePrefab, Vector3.zero, Quaternion.identity);
        HoveredNode.gameObject.SetActive(false);
        HoveredNode.gameObject.GetComponent<Renderer>().enabled = false;
        UnityGraph = FindObjectOfType<UnityGraph>();
    }

    void Update()
    {
        if (StateMachine != null)
        {
            StateMachine.Update();
        }
    }

    public void OnGUI()
    {
        if (bDrawSelectionRectangle)
        {
            GUI.Box(SelectionRectangle, "");
        }

        #region DEBUG
        foreach (var go in SelectableObjects)
        {
            //var WorldBounds = go.GetComponent<Renderer>().bounds;
            //var ScreenSpaceObjectRect = WorldBounds.ToScreenSpace(PlayerCamera);
            //Vector3 CameraPosition = PlayerCamera.transform.position;

            //// Testing -- extend bound by distance from center of screen.
            //var Distance_X = WorldBounds.center.x - CameraPosition.x;
            //Debug.Log(Distance_X);
            //WorldBounds.Expand(new Vector3(Distance_X, 1, 1));

            //Debug.LogFormat("{0}", go.name);
            //Debug.LogFormat("    World Position: {0}", go.transform.position);
            //Debug.LogFormat("    World Bounds: {0}", WorldBounds.ToString());
            //Debug.LogFormat("    World Bounds Min: {0}", WorldBounds.min.ToString());
            //Debug.LogFormat("    World Bounds Max: {0}", WorldBounds.max.ToString());
            //Debug.LogFormat("    Camera World to Screen Min: {0}", WorldToCameraMin);
            //Debug.LogFormat("    Camera World to Screen Max: {0}", WorldToCameraMax);
            //GUI.Box(ScreenSpaceObjectRect, go.name);
        }
        #endregion
    }

    public void CastMousePointerIntoWorld()
    {
        RaycastHit hit;
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 1000, GraphLayerMask))
        {
            Transform objectHit = hit.transform;
            var TargetNode = UnityGraph.GetNodeAtPosition(UnityGraph.WorldToLocalTile(objectHit.position));
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, .25f);
        }
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

            SelectedObjects.Clear();

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
                Bounds WorldBounds = Obj.GetComponent<Renderer>().bounds;
                Vector3 CameraPosition = PlayerCamera.transform.position;
                Rect ScreenSpaceObjectRect = WorldBounds.ToScreenSpace(PlayerCamera);

                // Change this to a SelectionHovered type of thing, and actually select on mouse up.
                if (SelectionRectangle.Overlaps(ScreenSpaceObjectRect, true))
                {
                    Obj.OnSelect();
            
                    if (!SelectedObjects.Contains(Obj))
                    {
                        SelectedObjects.Add(Obj);
                    }
                }
                else
                {
                    Obj.OnDeselect();
                    SelectedObjects.Remove(Obj);
                }
            }

            bDragging = true;
        }

        return bDragging;
    }

    public void ZoomCamera()
    {
        float ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

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
            HoveredNode.gameObject.SetActive(false);
            HoveredNode.gameObject.GetComponent<Renderer>().enabled = false;
            TargetPosition += PlayerCamera.transform.right * (CurrentMousePosition.x - PreviousMousePosition.x) * PanSpeed * -1f * Time.deltaTime;
            TargetPosition += Vector3.Cross(PlayerCamera.transform.right, Vector3.up) * (CurrentMousePosition.y - PreviousMousePosition.y) * PanSpeed * -1f * Time.deltaTime;
        }

        PlayerCamera.transform.position = TargetPosition;
    }
}

public static class BoundsExtensions
{
    public static Rect ToScreenSpace(this Bounds Bounds, Camera Camera)
    {
        Vector3 WorldToCameraMin = Camera.WorldToScreenPoint(Bounds.min);
        Vector3 WorldToCameraMax = Camera.WorldToScreenPoint(Bounds.max);

        float x = WorldToCameraMin.x;
        float y = Screen.height - WorldToCameraMax.y;
        float width = WorldToCameraMax.x - WorldToCameraMin.x;
        float height = WorldToCameraMax.y - WorldToCameraMin.y;

        return new Rect(x, y, width, height);
    }
}
