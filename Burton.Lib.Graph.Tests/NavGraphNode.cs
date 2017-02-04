using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burton.Lib.Graph.Tests
{
    public class NavGraphNode : GraphNode
    {
        public double LocationX;
        public double LocationY;

        public NavGraphNode(int Index, double LocationX, double LocationY)
        {
            this.NodeIndex = Index;
            this.LocationX = LocationX;
            this.LocationY = LocationY;
        }
    }
}
