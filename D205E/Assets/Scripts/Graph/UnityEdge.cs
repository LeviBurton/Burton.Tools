using System.Collections;
using System.Collections.Generic;
using Burton.Lib.Graph;

using UnityEngine;

public class UnityEdge : GraphEdge
{
    public UnityEdge(int FromNodeIndex, int ToNodeIndex, double EdgeCost)
        :base(FromNodeIndex, ToNodeIndex, EdgeCost)
    {
    }
}
