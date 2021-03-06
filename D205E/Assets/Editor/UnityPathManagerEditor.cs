﻿using Burton.Lib.Graph;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Burton.Lib.Unity
{
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

        public void OnSceneGUI()
        {
            PathManager.TargetPosition = Handles.PositionHandle(PathManager.TargetPosition, Quaternion.identity);

            foreach (var SearchRequest in PathManager.SearchRequests)
            {
                if (SearchRequest.PathToTarget.Count == 0)
                    continue;

                List<Vector3> Points = new List<Vector3>();
                Points.Add(SearchRequest.Graph.GetNode(SearchRequest.Search.TargetNodeIndex).Position);
                SearchRequest.PathToTarget.ForEach(x => Points.Add(SearchRequest.Graph.GetNode(x.FromIndex).Position));

                using (new Handles.DrawingScope(PathManager.SearchPathColor))
                {
                    Points.ForEach(x => Handles.SphereHandleCap(0, x, Quaternion.identity, .075f, EventType.Repaint));
                    Handles.DrawAAPolyLine(4.0f, Points.ToArray());
                }
            }

            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(20, 20, 150, 60));
            var Rect = EditorGUILayout.BeginVertical();
            GUI.Box(Rect, GUIContent.none);
            if (GUILayout.Button("Find Path"))
            {
                PathManager.ClearSearches();

                var TargetWorldPosition = PathManager.TargetPosition;

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

            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
            Handles.EndGUI();

            // Swallow up mouse activity so we don't lose our selection when clicking in the scene.
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetPosition"));
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Searches", EditorStyles.boldLabel);

            if (GUILayout.Button("Path to Target"))
            {
                PathManager.ClearSearches();

                var TargetWorldPosition = PathManager.TargetPosition;

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
}



//Handles.BeginGUI();
//            GUILayout.BeginArea(new Rect(20, 20, 150, 60));
//            var rect = EditorGUILayout.BeginVertical();
//GUI.Box(rect, GUIContent.none);
//            GUILayout.BeginHorizontal();
//            GUILayout.FlexibleSpace();
//            GUILayout.Label("Rotate");
//            GUILayout.FlexibleSpace();
//            GUILayout.EndHorizontal();
//            GUILayout.BeginHorizontal();
//            GUI.backgroundColor = Color.red;
//            if (GUILayout.Button("Left"))
//            {

//            }

//            if (GUILayout.Button("Right"))
//            {

//            }

//            GUILayout.EndHorizontal();
//            EditorGUILayout.EndVertical();
//            GUILayout.EndArea();
//            Handles.EndGUI();