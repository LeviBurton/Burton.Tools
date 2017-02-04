using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph
{
    class HeuristicEuclid<T> : IHeuristic<T> where T: SparseGraph<GraphNode, GraphEdge>
    {
        public double Calculate(T Graph, int Node1, int Node2)
        {
            return Graph.GetNode(Node1).NodeIndex + Graph.GetNode(Node2).NodeIndex;           
        }
    }
}
