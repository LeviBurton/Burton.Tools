using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    class HeuristicEuclid<T> : IHeuristic<T> where T: SparseGraph<GraphNode, GraphEdge>
    {
        private float Distance(float x1, float y1, float x2, float y2)
        {
            float y = x2 - x1;
            float x = y2 - y1;

            return (float)Math.Sqrt(y * y + x * x);
        }

        public float Calculate(T Graph, int Node1, int Node2)
        {
            var StartNode = (NavGraphNode) Graph.GetNode(Node1);
            var EndNode = (NavGraphNode) Graph.GetNode(Node2);
            return Distance(StartNode.X, StartNode.Y, EndNode.X, EndNode.Y);
        }
    }
}
