using System;
using System.Collections.Generic;

namespace Burton.Lib.Graph
{
    public class AdjacencyList<EdgeType> where EdgeType : GraphEdge, new()
    {
        public LinkedList<EdgeType>[] EdgeVector;
        
        // ctor: create empty adjacency list
        public AdjacencyList(int Vertices) 
        {
            EdgeVector = new LinkedList<EdgeType>[Vertices];

            for (int i = 0; i < EdgeVector.Length; ++i)
            {
                EdgeVector[i] = new LinkedList<EdgeType>();
            }
        }

        // Appends an edge to the linked list
        public void AddEdgeAtEnd(EdgeType Edge)
        {
            EdgeVector[Edge.FromNodeIndex].AddLast(Edge);
        }

        // Adds a new Edge to the linked list from the front
        public void AddEdgeAtBegin(EdgeType Edge)
        {
            EdgeVector[Edge.FromNodeIndex].AddFirst(Edge);
        }

        public EdgeType GetEdge(int From, int To)
        {
            EdgeType EdgeToReturn = null;

            foreach (var Edge in EdgeVector[From])
            {
                if (Edge.ToNodeIndex == To)
                {
                    EdgeToReturn = Edge;
                }
            }

            return EdgeToReturn;
        }

        public void RemoveNodeEdges(int From)
        {
            // Foreach edge leading from this node...
            foreach (var FromEdge in EdgeVector[From])
            {
                // get the list of edges it is pointing to
                var ToEdge = EdgeVector[FromEdge.ToNodeIndex].First;

                // foreach edge it is pointing to, check if it is us.
                while (ToEdge != null)
                {
                    // if the node it is pointg to is the node we want to delete, then delete that edge from the node.
                    if (ToEdge.Value.ToNodeIndex == From)
                    {
                        EdgeVector[FromEdge.ToNodeIndex].Remove(ToEdge);
                    }

                    ToEdge = ToEdge.Next;
                }
            }

            // Clear the nodes edges
            EdgeVector[From].Clear();
        }

        public void RemoveEdge(int From, int To)
        {
            EdgeType EdgeToRemove = null;

            foreach (var Edge in EdgeVector[From])
            {
                if (Edge.ToNodeIndex == To)
                {
                    EdgeToRemove = Edge;
                    break;
                }
            }

            EdgeVector[From].Remove(EdgeToRemove);
        }

        // Removmes the first occurence of an edge and returns true if there was any change 
        // in the collection, otherwise false.
        public bool RemoveEdge(EdgeType Edge)
        {
            return EdgeVector[Edge.FromNodeIndex].Remove(Edge);
        }

        // Returns the number of vertices
        public int NumVertices()
        {
            return EdgeVector.Length;
        }

        // Returns a copy of the linked list of outward edges from a vertex
        public LinkedList<EdgeType> this[int index]
        {
            get
            {
                LinkedList<EdgeType> EdgeList = new LinkedList<EdgeType>(EdgeVector[index]);
                return EdgeList;
            }
        }

        public void PrintAdjacencyList()
        {
            int i = 0;
            foreach (LinkedList<EdgeType> list in EdgeVector)
            {
                Console.Write("AdjacencyList[" + i + "] -> ");
                foreach(EdgeType edge in list)
                {
                    Console.Write(edge.FromNodeIndex + "(" + edge.ToNodeIndex + ")");
                }
                ++i;
                Console.WriteLine();
            }
        }

    }
}
