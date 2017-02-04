using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph
{ 
    public enum NodeType
    {
        InvalidNodeIndex = -1
    }

    public class GraphNode
    { 
        public int NodeIndex { get; set; }

        public GraphNode() { NodeIndex = (int)NodeType.InvalidNodeIndex; }
        public GraphNode(int Index) { NodeIndex = Index; }


    }
}
