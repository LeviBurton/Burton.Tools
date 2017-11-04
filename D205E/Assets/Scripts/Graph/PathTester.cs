using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Unity;

[CustomEditor(typeof(PathTester))]
public class PathTesterEditor : Editor
{
    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        return;

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("EnableMouseTracking"));

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        var PathTester = serializedObject.targetObject as PathTester;

        PathTester.PointA = Handles.PositionHandle(PathTester.PointA, Quaternion.identity);
        PathTester.PointB = Handles.PositionHandle(PathTester.PointB, Quaternion.identity);
        PathTester.TangentA = Handles.PositionHandle(PathTester.TangentA, Quaternion.identity);
        PathTester.TangentB = Handles.PositionHandle(PathTester.TangentB, Quaternion.identity);

        Handles.DrawBezier(PathTester.PointA, PathTester.PointB, PathTester.TangentA, PathTester.TangentB, Color.red, null, 5);

        if (PathTester.EnableMouseTracking)
        {
            var PathManager = PathTester.GetComponentInParent<UnityPathManager>();

            if (PathManager != null)
            {
            
            }
        }
    }
}

[RequireComponent(typeof(UnityPathManager))]
public class PathTester : MonoBehaviour
{
    public bool EnableMouseTracking;
    public Vector3 PointA;
    public Vector3 PointB;
    public Vector3 TangentA;
    public Vector3 TangentB;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
