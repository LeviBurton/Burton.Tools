//using Burton.Lib.Alg;
//using Burton.Lib.Graph;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Burton.Lib.Graph
//{
//    public class SearchAStar_TimeSliced<TEdgeType>: Graph_SearchTimeSliced<TEdgeType>
//    {
//        SparseGraph<GraphNode, GraphEdge> Graph;
//        public IHeuristic<SparseGraph<GraphNode, GraphEdge>> Heuristic;

//        public List<GraphEdge> ShortestPathTree;
//        public List<GraphEdge> SearchFrontier;

//        public List<double> CostToThisNode;
//        public List<double> GCosts;
//        public List<double> FCosts;

//        public int SourceNodeIndex;
//        public int TargetNodeIndex;

//        IndexedPriorityQueueLow<double> PQ;

//        public bool bFound;

//        public SearchAStar_TimeSliced(SparseGraph<GraphNode, GraphEdge> Graph, ESearchType Type, int Source, int Target) : base(Type)
//        {
//            this.Graph = Graph;
//            this.bFound = false;

//            //Heuristic = new HeuristicEuclid<SparseGraph<GraphNode, GraphEdge>>();
            
//            SourceNodeIndex = Source;
//            TargetNodeIndex = Target;

//            int ActiveNodeCount = Graph.ActiveNodeCount();
//            int NodeCount = Graph.NodeCount();

//            ShortestPathTree = new List<GraphEdge>(NodeCount);
//            SearchFrontier = new List<GraphEdge>(NodeCount);
//            CostToThisNode = new List<double>(NodeCount);
//            GCosts = new List<double>(NodeCount);
//            FCosts = new List<double>(NodeCount);

//            PQ = new IndexedPriorityQueueLow<double>(FCosts, NodeCount);

//            // not sure i need to initialize these...
//            for (int i = 0; i < NodeCount; i++)
//            {
//                ShortestPathTree.Insert(i, null);
//                SearchFrontier.Insert(i, null);
//                CostToThisNode.Insert(i, 0);
//                FCosts.Insert(i, 0);
//                GCosts.Insert(i, 0);
//            }
//        }

//        public override int CycleOnce()
//        {
//            // if the PQ is empty the target has not been found
//            if (PQ.IsEmpty())
//            {
//                return (int)ESearchStatus.TargetNotFound;
//            }

//            int NextClosestNode = PQ.Pop();
//            ShortestPathTree[NextClosestNode] = SearchFrontier[NextClosestNode];

//            if (NextClosestNode == TargetNodeIndex)
//            {
//                return (int)ESearchStatus.TargetFound;
//            }

//            foreach (var Edge in Graph.Edges[NextClosestNode])
//            {
//                double NewCost = CostToThisNode[NextClosestNode] + Edge.EdgeCost;
//                double HCost = Heuristic.Calculate(Graph, TargetNodeIndex, Edge.ToNodeIndex);
//                double GCost = GCosts[NextClosestNode] + Edge.EdgeCost;

//                if (SearchFrontier[Edge.ToNodeIndex] == null)
//                {
//                    FCosts[Edge.ToNodeIndex] = GCost + HCost;
//                    GCosts[Edge.ToNodeIndex] = GCost;
//                    PQ.Insert(Edge.ToNodeIndex);
//                    SearchFrontier[Edge.ToNodeIndex] = Edge;
//                }
//                else if ((GCost < GCosts[Edge.ToNodeIndex]) &&
//                          (ShortestPathTree[Edge.ToNodeIndex] == null))
//                {
//                    FCosts[Edge.ToNodeIndex] = GCost + HCost;
//                    GCosts[Edge.ToNodeIndex] = GCost;
//                    PQ.ChangePriority(Edge.ToNodeIndex);
//                    SearchFrontier[Edge.ToNodeIndex] = Edge;
//                }
//            }

//            return (int)ESearchStatus.SearchIncomplete;
//        }
    

//        public override double GetCostToTarget()
//        {
//            throw new NotImplementedException();
//        }

//        public override List<PathEdge> GetPathAsPathEdges()
//        {
//            List<PathEdge> Path = new List<PathEdge>();

//            if (TargetNodeIndex < 0)
//            {
//                return Path;
//            }

//            int Node = TargetNodeIndex;
//            while ((Node != SourceNodeIndex) && (ShortestPathTree[Node] != null) && Path.Count <= Graph.NodeCount())
//            {
//                var PathEdge = new PathEdge(
//                    (Graph.GetNode(ShortestPathTree[Node].FromNodeIndex) as NavGraphNode).Position,
//                    (Graph.GetNode(ShortestPathTree[Node].ToNodeIndex) as NavGraphNode).Position,
//                    0, 0);

//                Path.Insert(0, PathEdge);
//                Node = ShortestPathTree[Node].FromNodeIndex;
//            }

//            return Path;
//        }

//        public override List<int> GetPathToTarget()
//        {
//            var Path = new List<int>(Graph.NodeCount());

//            if (TargetNodeIndex < 0)
//                return Path;

//            int Node = TargetNodeIndex;

//            Path.Insert(0, Node);

//            int NodeCount = Graph.NodeCount();

//            while ((Node != SourceNodeIndex) && (ShortestPathTree[Node] != null) && Path.Count <= NodeCount)
//            {
//                Node = ShortestPathTree[Node].FromNodeIndex;
//                Path.Insert(0, Node);
//            }

//            return Path;
//        }

//        public override List<TEdgeType> GetSPT()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
