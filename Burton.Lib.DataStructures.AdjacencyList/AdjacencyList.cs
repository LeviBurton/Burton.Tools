using Burton.Lib.Types;
using System;
using System.Collections.Generic;


namespace Burton.Lib.DataStructures
{
    public class AdjacencyList
    {
        private LinkedList<Tuple<int, int>>[] InternalAdjacencyList;
        
        // ctor: create empty adjacency list
        public AdjacencyList(int Vertices) 
        {
            InternalAdjacencyList = new LinkedList<Tuple<int, int>>[Vertices];

            for (int i = 0; i < InternalAdjacencyList.Length; ++i)
            {
                InternalAdjacencyList[i] = new LinkedList<Tuple<int, int>>();
            }
        }

        // Appends an edge to the linked list
        public void AddEdgeAtEnd(int StartVertex, int EndVertex, int Weight)
        {
            InternalAdjacencyList[StartVertex - 1].AddLast(new Tuple<int, int>(EndVertex, Weight));
        }

        // Adds a new Edge to the linked list from the front
        public void AddEdgeAtBegin(int StartVertex, int EndVertex, int Weight)
        {
            InternalAdjacencyList[StartVertex - 1].AddFirst(new Tuple<int, int>(EndVertex, Weight));
        }

        // Removmes the first occurence of an edge and returns true if there was any change 
        // in the collection, otherwise false.
        public bool RemoveEdge(int StartVertex, int EndVertex, int Weight)
        {
            Tuple<int, int> Edge = new Tuple<int, int>(EndVertex, Weight);
            return InternalAdjacencyList[StartVertex - 1].Remove(Edge);
        }

        // Returns the number of vertices
        public int NumVertices()
        {
            return InternalAdjacencyList.Length;
        }

        // Returns a copy of the linked list of outward edges from a vertex
        public LinkedList<Tuple<int, int>> this[int index]
        {
            get
            {
                LinkedList<Tuple<int, int>> EdgeList = new LinkedList<Tuple<int, int>>(InternalAdjacencyList[index - 1]);
                return EdgeList;
            }
        }

        public void PrintAdjacencyList()
        {
            int i = 0;
            foreach (LinkedList<Tuple<int, int>> list in InternalAdjacencyList)
            {
                Console.Write("AdjacencyList[" + i + "] -> ");
                foreach(Tuple<int, int> edge in list)
                {
                    Console.Write(edge.Item1 + "(" + edge.Item2 + ")");
                }
                ++i;
                Console.WriteLine();
            }
        }

    }
}
