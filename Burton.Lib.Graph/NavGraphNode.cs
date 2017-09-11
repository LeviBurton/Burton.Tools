using Burton.Lib.Math;
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
        public float Z;

        public NavGraphNode() { }


     
        public NavGraphNode(int Index, float LocationX, float LocationY, float LocationZ)
        {
            this.NodeIndex = Index;
            this.X = LocationX;
            this.Y = LocationY;
            this.Z = LocationZ;
        }
    }
}
