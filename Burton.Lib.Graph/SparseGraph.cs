﻿using System;
using System.Collections.Generic;

namespace Burton.Lib.Graph
{
    public enum ENodeType { InvalidNodeIndex = -1 }

    public class SparseGraph<NodeType, EdgeType> where NodeType : GraphNode
                                                 where EdgeType : GraphEdge
    {
        /// <summary>
        /// Vector/array nodes that comprise this Graph
        /// </summary>
        private List<NodeType> Nodes = new List<NodeType>(1024 * 1024);

        /// <summary>
        /// A list of adjacency edge lists.
        /// Each node index keys into the list of edges associated with that node
        /// </summary>
        public AdjacencyList<GraphEdge> Edges = null;

        /// <summary>
        /// Is this a directed graoh?
        /// </summary>
        private bool bIsDigraph;

        /// <summary>
        /// The index of the next node to be added;
        /// </summary>
        private int NextNodeIndex;


        /// <summary>
        /// Returns the node at the given index
        /// </summary>
        /// <param name="NodeIndex"></param>
        /// <returns></returns>
        public NodeType GetNode(int NodeIndex)
        {
            if (NodeIndex > Nodes.Count)
                return null;

            NodeType Node = Nodes[NodeIndex];
            return Node;
        }

        /// <summary>
        /// Returns the edge From To
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public EdgeType GetEdge(int From, int To)
        {
            var Edge = (EdgeType) Edges.GetEdge(From, To);
            return Edge;
        }

        /// <summary>
        /// Returns the next free node index
        /// </summary>
        /// <returns></returns>
        public int GetNextFreeNodeIndex()
        {
            return NextNodeIndex;
        }

        /// <summary>
        /// Adds a node to the graph and returns it
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public int AddNode(NodeType Node)
        {
            if (Node.NodeIndex < Nodes.Count)
            {
                if (Nodes[Node.NodeIndex].NodeIndex != (int)ENodeType.InvalidNodeIndex)
                {
                    throw new ArgumentException("Invalid Node Index: " + Node.NodeIndex);
                }

                Nodes[Node.NodeIndex] = Node;

                return NextNodeIndex;
            }
            else
            {
                if (Node.NodeIndex != NextNodeIndex)
                {
                    throw new ArgumentException("Invalid Node Index: " + Node.NodeIndex);
                }

                Nodes.Add(Node);

                return NextNodeIndex++;
            }
        }

        /// <summary>
        /// Sets the node at NodeIndex to invalid and removes any edges connected to this node.
        /// </summary>
        /// <param name="NodeIndex"></param>
        public void RemoveNode(int NodeIndex)
        {
            if (NodeIndex > Nodes.Count)
            {
                throw new ArgumentException("Invalid Node Index: " + NodeIndex);
            }

            Nodes[NodeIndex].NodeIndex = (int)ENodeType.InvalidNodeIndex;

            Edges.RemoveNodeEdges(NodeIndex);
        }

        public void AddEdge(EdgeType Edge)
        {
            Edges.AddEdgeAtEnd(Edge);
        }

        public void RemoveEdge(int From, int To)
        {
            Edges.RemoveEdge(From, To);
        }

        /// <summary>
        /// Returns the number of active + inactive enodes present in the graph.
        /// </summary>
        /// <returns></returns>
        public int NumNodes()
        {
            return Nodes.Count;
        }

        // returns the number of active nodes present in the graph
        public int NumActiveNodes()
        {
            int ActiveNodeCount = 0;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].NodeIndex != (int)ENodeType.InvalidNodeIndex)
                    ActiveNodeCount++;
            }

            return ActiveNodeCount;
        }

        /// <summary>
        /// Returns the number of edges in the graph
        /// </summary>
        /// <returns>Count of edges in the graph</returns>
        public int NumEdges()
        {
            int EdgeCount = 0;
            for (int i = 0; i < NumNodes(); i++)
            {
                if (Nodes[i].NodeIndex != (int)ENodeType.InvalidNodeIndex)
                {
                    EdgeCount += Edges[Nodes[i].NodeIndex].Count;
                }
            }

            return EdgeCount;
        }

        // returns true if this is a directed graph
        public bool IsDigraph()
        {
            return bIsDigraph;
        }

        // returns true if the graph contains no nodes
        public bool IsEmpty()
        {
            return true;
        }

        // methods for loading saving graphs from an open file
        public bool Save(string FileName)
        {
            return false;
        }

        public bool Load(string FileName)
        {
            return false;
        }

        public SparseGraph(bool bIsDigraph)
        {
            this.bIsDigraph = bIsDigraph;
            NextNodeIndex = 0;
            Edges = new AdjacencyList<GraphEdge>(1024 * 1024);
        }
    }
}
