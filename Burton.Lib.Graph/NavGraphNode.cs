using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Burton.Lib.Graph
{
    [Serializable]
    public class NavGraphNode : GraphNode
    {
        public Vector2 Position = new Vector2(0,0);

        public double X
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }
        public double Y
        {
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }
      
        public NavGraphNode() { }

        public NavGraphNode(int Index, Vector2 Position)
        {
            this.NodeIndex = Index;
            this.Position = Position;
        }

        public NavGraphNode(int Index, double LocationX, double LocationY)
        {
            this.NodeIndex = Index;
            this.X = LocationX;
            this.Y = LocationY;
        }
    }
}
