using Burton.Lib.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityNode : GraphNode
{
    public Vector3 Position;

    public float X
    {
        get
        {
            return Position.x;
        }
        set
        {
            Position.x = value;
        }
    }
    public float Y
    {
        get
        {
            return Position.y;
        }
        set
        {
            Position.y = value;
        }
    }

    public UnityNode(int NodeIndex, Vector3 Position)
        : base(NodeIndex)
    {
        this.Position = Position;    
    }
}
