using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathTester))]
public class PathTesterEditor : Editor
{
    public void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    public void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("EnableMouseTracking"));

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        var PathTester = serializedObject.targetObject as PathTester;

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
public class PathTester : MonoBehaviour {
    public bool EnableMouseTracking;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
