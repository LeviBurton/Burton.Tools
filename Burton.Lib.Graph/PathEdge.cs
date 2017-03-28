using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Graph
{
    public class PathEdge
    {
        public Vector2 Source;
        public Vector2 Destination;
        public int Behavior;

        // FIXME: what is a door
        public int DoorID;

        public PathEdge(Vector2 Source, Vector2 Destination, int Behavior, int DoorID = 0)
        {
            this.Source = Source;
            this.Destination = Destination;
            this.Behavior = Behavior;
            this.DoorID = DoorID;
        }
    }
}
