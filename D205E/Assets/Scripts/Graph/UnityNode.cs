using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnityNode : GraphNode, ISerializationCallbackReceiver
{
    [NonSerialized]
    public Vector3 Position;

    // For serialization since Unity can't serialize Vector3
    public float Y;
    public float X;
    public float Z;

    public UnityNode()
    {
    }

    public UnityNode(int NodeIndex, Vector3 Position)
        : base(NodeIndex)
    {
        this.Position = Position;    
    }

    public void OnBeforeSerialize()
    {
        Y = Position.y;
        X = Position.x;
        Z = Position.z;
    }

    public void OnAfterDeserialize()
    {
        Position = new Vector3(X, Y, Z);
    }
}
