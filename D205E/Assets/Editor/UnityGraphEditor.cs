using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Burton.Lib.Unity
{
    [CustomEditor(typeof(UnityGraph))]
    public class UnityGraphEditor : Editor
    {
        // Might not need this -- serializedObject may be more appropriate.
        UnityGraph Graph;

        public void OnEnable()
        {
            Graph = serializedObject.targetObject as UnityGraph;

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            if (GUILayout.Button("Rebuild"))
            {
                Graph.Rebuild();

               // Graph.WeightEdges();
                Graph.RemoveUnWalkableNodesAndEdges();
                EditorUtility.SetDirty(Graph);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
          
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WallLayerMask"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawWalkable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WalkableRadius"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("TileColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WalkableColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BlockedColor"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("RayHitColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultRayColor"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawEdges"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EdgeColor"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawNodeIndex"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("NumTilesX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("NumTilesY"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TileWidth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TileHeight"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("TilePadding"));
  
            serializedObject.ApplyModifiedProperties();

        }
    }
}
