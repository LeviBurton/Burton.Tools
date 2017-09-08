using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGraphManager : MonoBehaviour {
    public List<UnityGraph> Graphs;

    public float TilePadding = .25f;
    public Color DefaultTileColor = Color.white;
    public bool DrawEdges = true;
    public bool DrawTiles = true;
    public string GraphAssetsPath = @"Assets/Data/Graphs";
    public Color DefaultEdgeLineColor = Color.gray;

    #region Singleton
    private static UnityGraphManager _instance;

    public static UnityGraphManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var type = typeof(UnityGraphManager);
                var objects = FindObjectsOfType<UnityGraphManager>();

                if (objects.Length > 0)
                {
                    _instance = objects[0];
                    if (objects.Length > 1)
                    {
                        Debug.LogWarning("There is more than one instance of Singleton of type \"" + type + "\". Keeping the first. Destroying the others.");
                        for (var i = 1; i < objects.Length; i++)
                            DestroyImmediate(objects[i].gameObject);
                    }
                    return _instance;
                }

                var gameObject = new GameObject();
                gameObject.name = type.ToString();

                _instance = gameObject.AddComponent<UnityGraphManager>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }
    #endregion

    public UnityGraphManager()
    {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void RemoveNode(int NodeIndex)
    {
        Graphs[0].Graph.RemoveNode(NodeIndex);
    }

    private void OnDrawGizmos()
    {
        var UnityGraph = Graphs[0];

        if (UnityGraph != null || UnityGraph.Graph != null)
        {
            foreach (var Node in UnityGraph.Graph.Nodes)
            {
                UnityNode GraphNode = null;

                GraphNode = (UnityNode)UnityGraph.Graph.GetNode(Node.NodeIndex);
             
                if (GraphNode == null)
                    continue;

                if (DrawTiles)
                {
                     
                    Gizmos.color = DefaultTileColor;
                    Vector3 CubePosition = new Vector3(transform.position.x + Node.Position.x, transform.position.y + Node.Position.y - 1f, transform.position.z + Node.Position.z);
                    Vector3 CubeSize = new Vector3(UnityGraph.TileWidth * (1 - TilePadding), .01f, UnityGraph.TileHeight * (1 - TilePadding));
                    Gizmos.DrawCube(CubePosition, CubeSize);
                }

                if (DrawEdges)
                {
                    foreach (var Edge in UnityGraph.Graph.Edges[Node.NodeIndex])
                    {
                        var FromNode = UnityGraph.Graph.GetNode(Edge.FromNodeIndex) as UnityNode;
                        var ToNode = UnityGraph.Graph.GetNode(Edge.ToNodeIndex) as UnityNode;
                        var FromPosition = new Vector3(FromNode.Position.x, 0, FromNode.Position.z);
                        var ToPosition = new Vector3(ToNode.Position.x, 0, ToNode.Position.z);

                        Gizmos.color = DefaultEdgeLineColor;
                        Gizmos.DrawLine(FromPosition, ToPosition);
                    }
                }
            }
        }
    }

}
