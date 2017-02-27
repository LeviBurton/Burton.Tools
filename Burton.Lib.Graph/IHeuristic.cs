using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    public interface IHeuristic<T>
    {
        double Calculate(T Graph, int Node1, int Node2);
    }
}
