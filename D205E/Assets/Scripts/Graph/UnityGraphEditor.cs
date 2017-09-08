using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityGraph))]
[Serializable]
public class GraphEditor : Editor
{
    public UnityGraph Graph;

    [Range(0, 100)]
    public int Test;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Debug.Log("GraphEditor.OnInspectorGUI()");

        Graph = target as UnityGraph;

        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Name", "Name of this graph"));
        Graph.Name = EditorGUILayout.TextField(Graph.Name);

        EditorGUILayout.LabelField(new GUIContent("Build Graph", "Builds a new graph based on the contents of the scene"));
        if (GUILayout.Button("Build Graph"))
        {
    
        }

        EditorGUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(UnityGraphManager))]
public class UnityGraphEditor : Editor
{
    public UnityGraph SelectedGraph;
    public UnityGraphManager GraphManager;

    [MenuItem("D20/Graphs/Create")]
    public static void CreateAsset()
    {
        var AssetPath = UnityGraphManager.Instance.GraphAssetsPath + string.Format(@"/{0}.asset", "Test");
        UnityGraph NewAsset = ScriptableObject.CreateInstance<UnityGraph>();

        AssetDatabase.CreateAsset(NewAsset, AssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = NewAsset;
    }

    public override void OnInspectorGUI()
    {
        GraphManager = target as UnityGraphManager;

        DrawDefaultInspector();

        UnityGraphManager.Instance.DefaultTileColor = EditorGUILayout.ColorField(UnityGraphManager.Instance.DefaultTileColor);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Build Graph", "Builds a new graph based on the contents of the scene"));
        if (GUILayout.Button("Build Graph"))
        {
            var Graph = UnityGraphManager.Instance.Graphs[0];
            if (Graph != null)
            {
                Graph.BuildDefaultGraph();
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Remove  Test", "currently only loads the default graph"));
        if (GUILayout.Button("Remove Test"))
        {
            var Graph = UnityGraphManager.Instance.Graphs[0];
            for (int i = 0; i <= 10; i++)
            {
                Graph.RemoveNode(i);
            }
            for (int i = 30; i <= 40; i++)
            {
                Graph.RemoveNode(i);
            }

            for (int i = 60; i <= 75; i++)
            {
                Graph.RemoveNode(i);
            }

        }

        EditorGUILayout.EndHorizontal();
        SceneView.RepaintAll();
    }

    protected virtual void OnSceneGUI()
    {
        GraphManager = target as UnityGraphManager;
        if (Event.current.type == EventType.Repaint)
        {
        }
    }

    private void DrawGraph()
    {
        
    }
}
