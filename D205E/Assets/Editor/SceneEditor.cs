using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneEditor : EditorWindow
{
    private static bool bRegisteredSceneDelegate = false;

    [MenuItem("D20/Scene Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneEditor));

        if (!bRegisteredSceneDelegate)
        {
            SceneView.onSceneGUIDelegate += OnScene;
        }
    }

    public void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();
        
        // Do your drawing here using GUI.
       // Debug.Log("SceneEditor:OnScene");

        Handles.EndGUI();
        EventType eventType = Event.current.type;

        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                Debug.Log("DragPerform");
             //   isAccepted = true;
            }
            Event.current.Use();
        }
    }
}
