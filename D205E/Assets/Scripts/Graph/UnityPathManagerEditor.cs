using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Search
{
    public int StartNode;
    public int EndNode;

    public Search(int startNode, int endNode)
    {
        StartNode = startNode;
        EndNode = endNode;
    }
}

[CustomEditor(typeof(UnityPathManager))]
public class UnityPathManagerEditor : Editor
{
    // Might not need this -- serializedObject may be more appropriate.
    UnityGraph Graph = null;
    UnityPathManager PathManager = null;
    List<Search> Searches = new List<Search>();

    public void OnEnable()
    {
        PathManager = serializedObject.targetObject as UnityPathManager;
        Graph = FindObjectsOfType<UnityGraph>()[0];

        Debug.LogFormat("Graph: {0}, PathManager: {1}", Graph.Name, PathManager.name);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Add Search"))
        {
            Searches.Add(new Search(0, 0));
        }

        if (GUILayout.Button("Clear Searches"))
        {
            Searches.Clear();
        }

        EditorGUILayout.LabelField("Searches", EditorStyles.boldLabel);

        if (GUILayout.Button("Submit Searches"))
        {
            IHeuristic<SparseGraph<UnityNode, UnityEdge>> Heuristic = new UnityDistanceHeuristic();

            foreach (var Search in Searches)
            {
                var Astar = new Search_AStar<UnityNode, UnityEdge>(Graph.Graph, Heuristic, Search.StartNode, Search.EndNode);
                var PathPlan = new UnityPathPlanner(Graph.Graph, Astar);

                PathManager.Register(PathPlan);
            }
        }

        if (GUILayout.Button("Step Search"))
        {
            PathManager.UpdateSearches();
        }

        EditorGUILayout.BeginVertical();
        foreach (var Search in Searches)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("StartNode: ");
            Search.StartNode = EditorGUILayout.IntField(Search.StartNode);
            EditorGUILayout.PrefixLabel("EndNode: ");
            Search.EndNode = EditorGUILayout.IntField(Search.EndNode);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumSearchCyclesPerUpdate"));

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
