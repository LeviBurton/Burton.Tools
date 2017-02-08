using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph
{
    public class GraphSearchDijkstra
    {
       public enum NodeStatus { Visited, Unvisited, NoParentAssigned };

        SparseGraph<GraphNode, GraphEdge> Graph;
        public List<GraphEdge> ShortestPathTree;
        public List<GraphEdge> TraversedEdges;
        public List<GraphEdge> SearchFrontier;

        public List<int> VisitedNodes;
        public List<int> Route;
        public List<float> CostToThisNode;

        public int SourceNodeIndex;
        public int TargetNodeIndex;

        public bool bFound;

        public GraphSearchDijkstra(SparseGraph<GraphNode,GraphEdge> Graph, int Source, int Target)
        {
            this.Graph = Graph;
            this.bFound = false;

            SourceNodeIndex = Source;
            TargetNodeIndex = Target;

            int ActiveNodeCount = Graph.ActiveNodeCount();
            int NodeCount = Graph.NodeCount();

            ShortestPathTree = new List<GraphEdge>();
            TraversedEdges = new List<GraphEdge>();
            SearchFrontier = new List<GraphEdge>();

            VisitedNodes = new List<int>(NodeCount);
            Route = new List<int>(ActiveNodeCount);
            CostToThisNode = new List<float>(ActiveNodeCount);
            Route = new List<int>(Graph.NodeCount());

            for (int i = 0; i < Graph.NodeCount(); i++)
            {
                VisitedNodes.Insert(i, (int)NodeStatus.Unvisited);
            }

            for (int i = 0; i < Graph.NodeCount(); i++)
            {
                Route.Insert(i, (int)NodeStatus.NoParentAssigned);
            }
        }

        public bool Search()
        {
            Queue<GraphEdge> Queue = new Queue<GraphEdge>();
            TraversedEdges.Clear();
            bFound = false;
            GraphEdge Dummy = new GraphEdge(SourceNodeIndex, SourceNodeIndex, 0);

            Queue.Enqueue(Dummy);

            while (Queue.Count > 0)
            {
                GraphEdge Next = Queue.Dequeue();

                Route[Next.ToNodeIndex] = Next.FromNodeIndex;

                TraversedEdges.Add(Next);

                if (Next.ToNodeIndex == TargetNodeIndex)
                {
                    bFound = true;
                    return true;
                }

                foreach (var Edge in Graph.Edges[Next.ToNodeIndex])
                {
                    if (VisitedNodes[Edge.ToNodeIndex] ==(int) NodeStatus.Unvisited)
                    {
                        Queue.Enqueue(Edge);

                        // mark node as visited before it is examined
                        // ensures a maximum of N edges are ever placed in the queue, rather than E edges
                        VisitedNodes[Edge.ToNodeIndex] = (int)NodeStatus.Visited;
                    }
                }
            }

            return false;
        }

        public Stack<int> GetPathToTarget()
        {
            var Path = new Stack<int>();

            if (!bFound || TargetNodeIndex < 0)
                return Path;

            int Node = TargetNodeIndex;

            Path.Push(Node);

            while (Node != SourceNodeIndex)
            {
                Node = Route[Node];
                Path.Push(Node);
            }

            return Path;
        }
    }
}
