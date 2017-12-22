using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera = null;
    public Vector3 CurrentMousePosition;
    public Vector3 PreviousMousePosition;
    public float ZoomLevel;

    public float PanSpeed = 5.0f;
    public float ZoomSpeed = 5.0f;

    void Awake()
    {
    }

    void Update()
    {
        HandleMouse();
    }

    public void HandleMouse()
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

    void PanCamera()
    {
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
    }
}
