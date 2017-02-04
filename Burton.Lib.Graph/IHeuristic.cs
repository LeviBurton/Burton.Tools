using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph
{
    public interface IHeuristic<T>
    {
        double Calculate(T Graph, int Node1, int Node2);
    }
}
