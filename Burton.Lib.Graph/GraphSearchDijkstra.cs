using System;
using System.Collections.Generic;
using System.Text;
using Burton.Lib.Alg;

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
            var PriQueue = new PriorityQueue<float>();
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
