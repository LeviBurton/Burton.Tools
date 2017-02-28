using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    [Serializable]
    public class NavGraphNode : GraphNode
    {
        public double X;
        public double Y;

        public NavGraphNode() { }

        public NavGraphNode(int Index, double LocationX, double LocationY)
        {
            this.NodeIndex = Index;
            this.X = LocationX;
            this.Y = LocationY;
        }
    }
}
