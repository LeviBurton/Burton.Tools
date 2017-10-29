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

    public void OnEnable()
    {
        PathManager = serializedObject.targetObject as UnityPathManager;
        Graph = FindObjectsOfType<UnityGraph>()[0];

     //   Debug.LogFormat("Graph: {0}, PathManager: {1}", Graph.Name, PathManager.name);

        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        //Debug.Log("OnSceneGUI");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Target"));

        if (GUILayout.Button("Add Search"))
        {
            PathManager.Searches.Add(new Search(0, 0));
        }

        if (GUILayout.Button("Clear Searches"))
        {
            PathManager.Searches.Clear();
            PathManager.TargetFoundSearchRequests.Clear();
            EditorUtility.SetDirty(PathManager);
        }

        EditorGUILayout.LabelField("Searches", EditorStyles.boldLabel);

        if (GUILayout.Button("Path to Target"))
        {
            // Get node closest to transform.position
            var TargetPosition = PathManager.Target.position;

            // map it to graph coordinate space
            var LocalPosition = Graph.transform.InverseTransformPoint(TargetPosition);
            
            // Snap to closest grid point
            var NodePosition = new Vector3(Mathf.Round((LocalPosition.x * Graph.TileWidth) / Graph.TileWidth), Mathf.Round((LocalPosition.y * Graph.TileWidth) / Graph.TileWidth), Mathf.Round((LocalPosition.z * Graph.TileHeight) / Graph.TileHeight));

            // Get the node at that grid point
            var Node = Graph.GetNodeAtPosition(NodePosition);

            Debug.LogFormat("World: {0}, Local: {1}, Test: {2}, Node: {3}", TargetPosition, LocalPosition, NodePosition, Node != null ? Node.NodeIndex : 0);
        }

        if (GUILayout.Button("Set Searches"))
        {
            PathManager.TargetFoundSearchRequests.Clear();
            PathManager.SearchRequests.Clear();

            IHeuristic<SparseGraph<UnityNode, UnityEdge>> Heuristic = new UnityDistanceHeuristic();

            foreach (var Search in PathManager.Searches)
            {
                var Astar = new Search_AStar<UnityNode, UnityEdge>(Graph.Graph, Heuristic, Search.StartNode, Search.EndNode);
                var PathPlan = new UnityPathPlanner(Graph, Astar);
          
                PathManager.Register(PathPlan);
            }
        }

        if (GUILayout.Button("Step Search"))
        {
            PathManager.UpdateSearches();
            EditorUtility.SetDirty(PathManager);
        }

        EditorGUILayout.BeginVertical();
        foreach (var Search in PathManager.Searches)
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