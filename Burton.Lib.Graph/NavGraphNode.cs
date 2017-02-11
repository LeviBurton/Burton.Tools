using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    [Serializable]
    public class NavGraphNode : GraphNode
    {
        public float X;
        public float Y;

        public NavGraphNode() { }

        public NavGraphNode(int Index, float LocationX, float LocationY)
        {
            this.NodeIndex = Index;
            this.X = LocationX;
            this.Y = LocationY;
        }
    }
}
