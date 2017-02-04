using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph
{
    public class GraphSearchDFS
    {
       public enum NodeStatus { Visited, Unvisited, NoParentAssigned };

        SparseGraph<GraphNode, GraphEdge> Graph;
        List<int> VisitedNodes;
        List<int> Route;

        public List<GraphEdge> TraversedEdges;

        int SourceNodeIndex;
        int TargetNodeIndex;
        public bool bFound;

        public GraphSearchDFS(SparseGraph<GraphNode,GraphEdge> Graph, int Source, int Target)
        {
            this.Graph = Graph;
            this.bFound = false;
            this.SourceNodeIndex = Source;
            this.TargetNodeIndex = Target;

            TraversedEdges = new List<GraphEdge>();

            VisitedNodes = new List<int>();
            for (int i = 0; i < Graph.NodeCount(); i++)
            {
                VisitedNodes.Insert(i, (int)NodeStatus.Unvisited);
            }

            Route = new List<int>(Graph.NodeCount());
            for (int i = 0; i < Graph.NodeCount(); i++)
            {
                Route.Insert(i, (int)NodeStatus.NoParentAssigned);
            }

            bFound = Search();
        }

        public bool Search()
        {
            Stack<GraphEdge> Stack = new Stack<GraphEdge>();
            TraversedEdges.Clear();

            GraphEdge Dummy = new GraphEdge(SourceNodeIndex, SourceNodeIndex, 0);

            Stack.Push(Dummy);

            while (Stack.Count > 0)
            {
                GraphEdge Next = Stack.Pop();
                Route[Next.ToNodeIndex] = Next.FromNodeIndex;
                TraversedEdges.Add(Next);

                if (Next != Dummy)
                {

                }

                VisitedNodes[Next.ToNodeIndex] = (int)NodeStatus.Visited;
                if (Next.ToNodeIndex == TargetNodeIndex)
                {
                    return true;
                }

                foreach (var Edge in Graph.Edges[Next.ToNodeIndex])
                {
                    if (VisitedNodes[Edge.ToNodeIndex] ==(int) NodeStatus.Unvisited)
                    {
                        Stack.Push(Edge);
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
