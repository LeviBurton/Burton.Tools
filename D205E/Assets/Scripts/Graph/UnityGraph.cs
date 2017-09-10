using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class UnityGraph : MonoBehaviour, ISerializationCallbackReceiver
{
    public string Name = "UnityGraph";

    public SparseGraph<UnityNode, UnityEdge> Graph;

    private byte[] GraphByteArray;
    private string GraphXml;

    public int NumTilesX = 25;
    public int NumTilesY = 25;
    public int TileWidth = 10;      // TODO: What units do these represent?
    public int TileHeight = 10;     // TODO: What units do these represent?
    public float TilePadding = .01f;

    public Color DefaultTileColor;
    public Color DefaultEdgeColor;

    public List<UnityNode> Nodes
    {
        get { return Graph.Nodes; }
        set { }
    }

    int Width = 0;
    int Height = 0;

    public UnityGraph()
    {
      
    }

    public void RemoveNode(int NodeIndex)
    {
        if (Graph == null)
            return;

        Graph.RemoveNode(NodeIndex);
    }

    public void Init()
    {

    }

    public void BuildDefaultGraph()
    {
        Debug.Log("Building: " + Name);

        Graph = new SparseGraph<UnityNode, UnityEdge>(false);

        Width = TileWidth * NumTilesX;
        Height = TileHeight * NumTilesY;

        float MidX = TileWidth / 2;
        float MidY = TileHeight / 2;

        for (int Row = 0; Row < NumTilesY; ++Row)
        {
            for (int Col = 0; Col < NumTilesX; ++Col)
            {
                var NodeIndex = Graph.AddNode(new UnityNode(Graph.GetNextFreeNodeIndex(), new Vector3(MidX + (Col * TileWidth), 1, MidY + (Row * TileWidth))));
                Graph.Edges.Insert(NodeIndex, new List<GraphEdge>());
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

    private void OnDrawGizmos()
    {
        if (Graph == null)
            return;

        int NumGraph = 0;
        float Height = 5;

        foreach (var Node in Graph.Nodes)
        {
            UnityNode GraphNode = null;

            GraphNode = (UnityNode)Graph.GetNode(Node.NodeIndex);

            if (GraphNode == null)
                continue;

            //if (DrawTiles)
            //{
            Gizmos.color = DefaultTileColor;
            Vector3 CubePosition = new Vector3(transform.position.x + Node.Position.x, (Height * NumGraph) + transform.position.y + Node.Position.y - 1f, transform.position.z + Node.Position.z);

            Vector3 CubeSize = new Vector3(TileWidth * (1 - TilePadding), .01f, TileHeight * (1 - TilePadding));
            Gizmos.DrawCube(CubePosition, CubeSize);
            // }

            //if (DrawEdges)
            //{
            foreach (var Edge in Graph.Edges[Node.NodeIndex])
            {
                var FromNode = Graph.GetNode(Edge.FromNodeIndex) as UnityNode;
                var ToNode = Graph.GetNode(Edge.ToNodeIndex) as UnityNode;
                var FromPosition = new Vector3(transform.position.x + FromNode.Position.x, transform.position.y + (Height * NumGraph), transform.position.z + FromNode.Position.z);
                var ToPosition = new Vector3(transform.position.x + ToNode.Position.x, transform.position.y + (Height * NumGraph), transform.position.z + ToNode.Position.z);

                Gizmos.color = DefaultEdgeColor;
                Gizmos.DrawLine(FromPosition, ToPosition);
            }
            //}
        }
    }

    public void OnBeforeSerialize()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (Graph != null)
        {
            // Hack to serialize nodes
            foreach (var Node in Graph.Nodes)
            {
                Node.OnBeforeSerialize();
            }

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, Graph);
                GraphByteArray = ms.ToArray();
            }
        }
    }


    public void OnAfterDeserialize()
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(GraphByteArray, 0, GraphByteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            Graph = binForm.Deserialize(memStream) as SparseGraph<UnityNode, UnityEdge>;

            // Hack to restore nodes
            foreach (var Node in Graph.Nodes)
            {
                Node.OnAfterDeserialize();
            }
        }

    }
}
