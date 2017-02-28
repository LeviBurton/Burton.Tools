using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    class HeuristicEuclid<T> : IHeuristic<T> where T: SparseGraph<GraphNode, GraphEdge>
    {
        private double Distance(double x1, double y1, double x2, double y2)
        {
            double y = x2 - x1;
            double x = y2 - y1;

            return Math.Sqrt(y * y + x * x);
        }

        public double Calculate(T Graph, int Node1, int Node2)
        {
            var StartNode = (NavGraphNode) Graph.GetNode(Node1);
            var EndNode = (NavGraphNode) Graph.GetNode(Node2);
            return Distance(StartNode.X, StartNode.Y, EndNode.X, EndNode.Y);
        }
    }
}
