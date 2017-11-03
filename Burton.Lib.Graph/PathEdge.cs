using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Graph
{
    public class PathEdge
    {
        public int SourceIndex;
        public int DestinationIndex;
        public int Behavior;

        // FIXME: what is a door
        public int DoorID;

        public PathEdge(int SourceIndex, int DestinationIndex, int Behavior, int DoorID = 0)
        {
            this.SourceIndex = SourceIndex;
            this.DestinationIndex = DestinationIndex;
            this.Behavior = Behavior;
            this.DoorID = DoorID;
        }
    }
}
