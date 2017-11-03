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
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Target"));
        EditorGUILayout.LabelField("Searches", EditorStyles.boldLabel);

        if (GUILayout.Button("Path to Target"))
        {
            PathManager.ClearSearches();

            var TargetWorldPosition = PathManager.Target.position;

            var TargetNode = Graph.GetNodeAtPosition(Graph.WorldToLocalTile(TargetWorldPosition));
            var SourceNode = Graph.GetNodeAtPosition(Graph.WorldToLocalTile(PathManager.transform.position));

            // map it to graph coordinate space
            var LocalNodePosition = Graph.transform.InverseTransformPoint(TargetWorldPosition);

            IHeuristic<SparseGraph<UnityNode, UnityEdge>> Heuristic = new UnityDistanceHeuristic();
            var Astar = new Search_AStar<UnityNode, UnityEdge>(Graph.Graph, Heuristic, SourceNode.NodeIndex, TargetNode.NodeIndex);
            var PathPlan = new UnityPathPlanner(Graph, Astar);

            PathManager.Register(PathPlan);

            PathManager.UpdateSearches();

            EditorUtility.SetDirty(PathManager);

            Debug.LogFormat("World: {0}, Local: {1}, Node: {2}, Node: {3}", TargetWorldPosition, LocalNodePosition, TargetNode.Position, TargetNode != null ? TargetNode.NodeIndex : 0);
        }

        if (GUILayout.Button("Step Search"))
        {
            PathManager.UpdateSearches();
            EditorUtility.SetDirty(PathManager);
        }

        if (GUILayout.Button("Clear Searches"))
        {
            PathManager.ClearSearches();
            EditorUtility.SetDirty(PathManager);
        }

     

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumSearchCyclesPerUpdate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SearchPathColor"));
        serializedObject.ApplyModifiedProperties();
    }
}