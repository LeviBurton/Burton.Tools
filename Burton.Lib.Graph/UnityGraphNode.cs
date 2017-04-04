using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Burton.Lib.Graph
{
    // This guy is meant to be used in the unity scene graph.
    public class UnityGraphNode : GraphNode
    {
        public Vector3 Position;

        public UnityGraphNode(Vector3 Position)
        {
            this.Position = Position;
        }
    }
}
