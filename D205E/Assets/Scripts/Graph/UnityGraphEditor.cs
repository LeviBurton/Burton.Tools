using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityGraph))]
public class UnityGraphEditor : Editor
{
    public UnityGraph SelectedGraph;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
       
        SelectedGraph.DefaultTileColor = EditorGUILayout.ColorField(SelectedGraph.DefaultTileColor);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Load Graph", "Currently only loads the Default Graph"));
        if (GUILayout.Button("Load Graph"))
        {
            SelectedGraph.LoadGraph();
        }

        EditorGUILayout.EndHorizontal();
        SceneView.RepaintAll();
    }

    protected virtual void OnSceneGUI()
    {
        if (SelectedGraph == null)
        {
            SelectedGraph = target as UnityGraph;
        }

        if (SelectedGraph == null)
            return;

        if (SelectedGraph.Graph == null)
        {
            SelectedGraph.LoadGraph();
        }

        if (Event.current.type == EventType.Repaint)
        {
           // DrawGraph();
        }
    }

    private void DrawGraph()
    {
        if (SelectedGraph != null)
        {
            foreach (var Node in SelectedGraph.Nodes)
            {
                Handles.color = SelectedGraph.DefaultTileColor;

                Handles.CubeHandleCap(0,
                    Node.Position,
                    Quaternion.identity,
                     SelectedGraph.TileWidth * (1 - SelectedGraph.TilePadding),
                     EventType.Repaint);

            }
        }
    }
}
