using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(UnityGraph))]
public class UnityGraphEditor : Editor
{
    // Might not need this -- serializedObject may be more appropriate.
    UnityGraph Graph;

    public void OnEnable()
    {
        Graph = serializedObject.targetObject as UnityGraph;
    }

    //public override void OnInspectorGUI()
    //{
    //    Graph = (UnityGraph) this.target;

    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.PrefixLabel("Name");
    //    Graph.Name = EditorGUILayout.TextField(Graph.Name);
    //    EditorGUILayout.EndHorizontal();

    //    //EditorGUILayout.PrefixLabel("Name");
    //    if (GUILayout.Button("Rebuild"))
    //    {
    //        Graph.BuildDefaultGraph();
    //        EditorUtility.SetDirty(Graph);
    //        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    //    }
    //}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
        if (GUILayout.Button("Rebuild"))
        {
            Graph.BuildDefaultGraph();
            EditorUtility.SetDirty(Graph);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        #region Test
        EditorGUILayout.LabelField("Test Stuff", EditorStyles.boldLabel);
        if (GUILayout.Button("Remove Test"))
        {
            for (int i = 0; i <= 5; i++)
            {
                Graph.RemoveNode(i);
            }

            EditorUtility.SetDirty(Graph);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        EditorGUILayout.Separator(); EditorGUILayout.Separator();
        #endregion

        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawNodes"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawEdges"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumTilesX"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumTilesY"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TileWidth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TileHeight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultTileColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultEdgeColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TilePadding"));
        //EditorGUILayout.LabelField(string.Format("Nodes: {0}", Graph.Graph == null ? 0 : Graph.Graph.ActiveNodeCount()));

        serializedObject.ApplyModifiedProperties();

    }
}


//[CustomEditor(typeof(UnityGraphManager))]
//public class UnityGraphEditor : Editor
//{
//    public UnityGraph SelectedGraph;
//    public List<UnityGraph> AvailableGraphs = new List<UnityGraph>();

//    [MenuItem("D20/Graphs/Create")]
//    public static void CreateAsset()
//    {

//    }

//    private void OnEnable()
//    {
//        Debug.Log("UnityGraphEditor.OnEnable");
//        AvailableGraphs.Clear();

//        // Populate AvailableGraphs using the UnityGraphManager
//        AvailableGraphs = UnityGraphManager.Instance.Graphs;
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // DrawDefaultInspector();

//        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
//        if (GUILayout.Button("Create Graph"))
//        {
//            UnityGraphManager.Instance.ResetAll();
//            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
//        }

//        if (GUILayout.Button("Load All"))
//        {
//            UnityGraphManager.Instance.LoadAll();
//            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
//        }

//        if (GUILayout.Button("Save All"))
//        {

//        }
//        EditorGUILayout.Separator(); EditorGUILayout.Separator();

//        EditorGUILayout.LabelField("Graphs", EditorStyles.boldLabel);
//        foreach (var Graph in AvailableGraphs)
//        {
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.Space();
//            EditorGUILayout.LabelField("Name");
//            Graph.Name = EditorGUILayout.TextField(Graph.Name);
//            EditorGUILayout.EndHorizontal();


//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.Space();
//            // UnityGraphManager.Instance.DefaultTileColor = EditorGUILayout.ColorField(UnityGraphManager.Instance.DefaultTileColor);
//            EditorGUILayout.LabelField("Default Tile Color");
//            Graph.DefaultTileColor = EditorGUILayout.ColorField(Graph.DefaultTileColor);
//            EditorGUILayout.EndHorizontal();

//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.Space();
//            EditorGUILayout.LabelField("Default Edge Color");
//            Graph.DefaultEdgeColor = EditorGUILayout.ColorField(Graph.DefaultEdgeColor);
//            EditorGUILayout.EndHorizontal();

//            EditorGUILayout.Separator(); EditorGUILayout.Separator();

//        }

//    }

//    protected virtual void OnSceneGUI()
//    {
//        if (Event.current.type == EventType.Repaint)
//        {
//        }
//    }
//}
