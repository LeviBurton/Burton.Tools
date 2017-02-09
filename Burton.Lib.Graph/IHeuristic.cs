using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    public interface IHeuristic<T>
    {
        float Calculate(T Graph, int Node1, int Node2);
    }
}
