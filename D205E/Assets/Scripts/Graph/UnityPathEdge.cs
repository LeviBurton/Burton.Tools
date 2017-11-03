using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPathEdge
{
    public Vector3 Source;
    public Vector3 Destination;

    public int Behavior;

    public UnityPathEdge(Vector3 Source, Vector3 Destination, int Behavior)
    {
        this.Source = Source;
        this.Destination = Destination;
        this.Behavior = Behavior;
    }
}
