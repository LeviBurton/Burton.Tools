﻿using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class UnityDistanceHeuristic : IHeuristic<SparseGraph<UnityNode, UnityEdge>>
{
    public double Calculate(SparseGraph<UnityNode, UnityEdge> Graph, int Node1, int Node2)
    {
        var UnityNode1 = Graph.GetNode(Node1);
        var UnityNode2 = Graph.GetNode(Node2);
        var Distance = Vector3.Distance(UnityNode1.Position, UnityNode2.Position);

        return Distance;
    }
}


[Serializable]
public class UnityGraph : MonoBehaviour, ISerializationCallbackReceiver
{
    [NonSerialized]
    public SparseGraph<UnityNode, UnityEdge> Graph = new SparseGraph<UnityNode, UnityEdge>();
    public byte[] SerializedGraph;

    private IHeuristic<SparseGraph<UnityNode, UnityEdge>> Heuristic = new UnityDistanceHeuristic();

    public string Name = "UnityGraph";
    public int NumTilesX = 25;
    public int NumTilesY = 25;
    public int TileWidth = 10;      // TODO: What units do these represent?
    public int TileHeight = 10;     // TODO: What units do these represent?
    public float TilePadding = .01f;

    public Color DefaultTileColor;
    public Color DefaultEdgeColor;
    public Color DefaultSearchPathColor;
    public float PathSphereSize = 1.0f;

    public bool DrawNodeIndex = true;
    public bool DrawEdges = true;
    public bool DrawNodes = true;
    public bool DrawSearchPaths = true;
    public int StartNode = 0;
    public int EndNode = 0;
    public List<UnityNode> SearchPath = new List<UnityNode>();

    private bool bEnabled = true;

    int Width = 0;
    int Height = 0;

    public void TestPath()
    {
        var Search = new Search_AStar<UnityNode, UnityEdge>(Graph, Heuristic, StartNode, EndNode);
        Search.Search();
        SearchPath.Clear();

        if (Search.bFound)
        {
            foreach (var NodeIndex in Search.GetPathToTarget())
            {
                SearchPath.Add(Graph.GetNode(NodeIndex));
            }

            SearchPath.Reverse();
        }
    }

    void OnEnable()
    {
        bEnabled = true;

    }

    void OnDisable()
    {
        bEnabled = false;
    }

    public void TestCalc()
    {
        var Distance = Heuristic.Calculate(Graph, Graph.GetNode(0).NodeIndex, Graph.GetNode(2).NodeIndex);

        Debug.Log("Distance: " + Distance);
    }

    public void RemoveNode(int NodeIndex)
    {
        if (Graph == null)
            return;

        Graph.RemoveNode(NodeIndex);
    }

    [ExecuteInEditMode]
    void Start()
    {

    }

    void OnGUI()
    {
       
    }

    static void drawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();
        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height , size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }

    private void OnDrawGizmos()
    {
        if (Graph == null || !bEnabled)
            return;

        foreach (var Node in Graph.Nodes)
        {
            UnityNode GraphNode = null;

            GraphNode = (UnityNode)Graph.GetNode(Node.NodeIndex);

            if (GraphNode == null)
                continue;

            if (DrawNodeIndex)
            {
                var StringPosition = new Vector3(transform.position.x + Node.Position.x, transform.position.y + Node.Position.y, transform.position.z + Node.Position.z);
                StringPosition.y += 1.5f;
                drawString(Node.NodeIndex.ToString(), StringPosition, Color.white);
            }

            if (DrawNodes)
            {
                Gizmos.color = DefaultTileColor;
                Vector3 CubePosition = new Vector3(transform.position.x + Node.Position.x, transform.position.y + Node.Position.y, transform.position.z + Node.Position.z);
                Vector3 CubeSize = new Vector3(TileWidth * (1 - TilePadding), .01f, TileHeight * (1 - TilePadding));
                Gizmos.DrawCube(CubePosition, CubeSize);
            }

            if (DrawEdges)
            {
                foreach (var Edge in Graph.Edges[Node.NodeIndex])
                {
                    var FromNode = Graph.GetNode(Edge.FromNodeIndex) as UnityNode;
                    var ToNode = Graph.GetNode(Edge.ToNodeIndex) as UnityNode;
                    var FromPosition = new Vector3(transform.position.x + FromNode.Position.x, transform.position.y, transform.position.z + FromNode.Position.z);
                    var ToPosition = new Vector3(transform.position.x + ToNode.Position.x, transform.position.y, transform.position.z + ToNode.Position.z);

                    Gizmos.color = DefaultEdgeColor;
                    Gizmos.DrawLine(FromPosition, ToPosition);
                }
            }

        }

        if (DrawSearchPaths)
        {
            Gizmos.color = DefaultSearchPathColor;
            foreach (var Node in SearchPath)
            {
                Vector3 CubePosition = new Vector3(transform.position.x + Node.Position.x, transform.position.y + (Node.Position.y + TileWidth / 4), transform.position.z + Node.Position.z);

                Vector3 CubeSize = new Vector3(TileWidth/2 * (1 - TilePadding), .1f, TileHeight/2 * (1 - TilePadding));
               // Gizmos.DrawSphere(CubePosition, PathSphereSize);
                Gizmos.DrawCube(CubePosition, CubeSize);
            }
        }
    }

    #region Grid Graph Stuff

    public void Rebuild()
    {
        // Reset the graph (resets nodes and edges connecting them)
        ResetGraph();

        // Find all walls and update our graph with them
        BuildWalls();


    }

    public void BuildWalls()
    {
        var Walls = FindObjectsOfType<Wall>();

        foreach (var Wall in Walls)
        {
            var Origin = Wall.transform.position;
            var Length = Wall.Length;
            var Width = Wall.Width;

            if (Wall.Length > 0)
            {
                for (int WallSection = 0; WallSection < Wall.Length; WallSection++)
                {
                    var Node = GetNodeAtPosition(Origin);

                    Graph.RemoveNode(Node.NodeIndex);

                    Origin.x += TileWidth;
                }
            }
        }
    }

    public void ResetGraph()
    {
        Graph = new SparseGraph<UnityNode, UnityEdge>(false);

        Width = TileWidth * NumTilesX;
        Height = TileHeight * NumTilesY;

        float MidX = TileWidth / 2;
        float MidY = TileHeight / 2;

        for (int Row = 0; Row < NumTilesY; ++Row)
        {
            for (int Col = 0; Col < NumTilesX; ++Col)
            {
                var NodeIndex = Graph.AddNode(new UnityNode(Graph.GetNextFreeNodeIndex(), new Vector3(MidX + (Col * TileWidth), 0, MidY + (Row * TileWidth))));
                Graph.Edges.Insert(NodeIndex, new List<UnityEdge>());
            }
        }

        for (int Row = 0; Row < NumTilesY; ++Row)
        {
            for (int Col = 0; Col < NumTilesX; ++Col)
            {
                AddAllNeighborsToGridNode(Row, Col, NumTilesX, NumTilesY);
            }
        }
    }

    public static bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
    {
        return !((x < 0) || (x >= NumCellsX) || (y < 0) || (y >= NumCellsY));
    }

    public void AddAllNeighborsToGridNode(int Row, int Col, int CellsX, int CellsY)
    {
        for (int i = -1; i < 2; ++i)
        {
            for (int j = -1; j < 2; ++j)
            {
                int NodeX = (Col + j);
                int NodeY = (Row + i);

                if ((i == 0) && (j == 0))
                    continue;

                if (ValidNeighbor(NodeX, NodeY, CellsX, CellsY))
                {
                    var Node = (UnityNode)Graph.GetNode(Row * CellsX + Col);

                    if (Node.NodeIndex == -(int)ENodeType.InvalidNodeIndex)
                        continue;

                    var NeighborNode = (UnityNode)Graph.GetNode(NodeY * CellsX + NodeX);

                    if (NeighborNode.NodeIndex == (int)ENodeType.InvalidNodeIndex)
                        continue;

                    var PosNode = new Vector3(Node.X, 1, Node.Y);
                    var PosNeighborNode = new Vector3(NeighborNode.X, 1, NeighborNode.Y);

                    double Distance = Vector3.Distance(PosNode, PosNeighborNode);
                    var NewEdge = new UnityEdge(Node.NodeIndex, NeighborNode.NodeIndex, Distance);

                    Graph.AddEdge(NewEdge);

                    if (!Graph.IsDigraph())
                    {
                        UnityEdge Edge = new UnityEdge(NeighborNode.NodeIndex, Node.NodeIndex, Distance);
                        Graph.AddEdge(Edge);
                    }
                }
            }
        }
    }
    // Test
    #endregion


    public UnityNode GetNodeAtPosition(Vector3 Position)
    {
        var LocalOrigin = Position - transform.position;
        int NodeIndex = ((int)(LocalOrigin.z / TileHeight) * NumTilesX) + ((int)LocalOrigin.x / TileWidth);
        var Node = Graph.GetNode(NodeIndex);

        Debug.LogFormat("{0} : {1}", LocalOrigin, Node.NodeIndex);

        return Node;
    }

    #region Serialization

    // This is called when this object is selected in the editor -- it serializes the 
    // graph into a byte array during property editing.  This allows us to work on our graph in edit mode.
    public void OnBeforeSerialize()
    {
        var StopWatch = new System.Diagnostics.Stopwatch();
        StopWatch.Start();

        BinaryFormatter bf = new BinaryFormatter();

        foreach (var Node in Graph.Nodes)
        {
            Node.OnBeforeSerialize();
        }

        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, Graph);
            SerializedGraph = ms.ToArray();
        }

        StopWatch.Stop();
       // Debug.Log("OnBeforeSerialize: " + StopWatch.Elapsed);
    }

    public void OnAfterDeserialize()
    {
        if (SerializedGraph == null)
            return;

        var StopWatch = new System.Diagnostics.Stopwatch();
        StopWatch.Start();

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(SerializedGraph))
        {
            Graph = (SparseGraph<UnityNode, UnityEdge>)bf.Deserialize(ms);
            foreach (var Node in Graph.Nodes)
            {
                Node.OnAfterDeserialize();
            }
        }
        StopWatch.Stop();

     //   Debug.Log("OnAfterDeserialize: " + StopWatch.Elapsed);
    }
}
#endregion


