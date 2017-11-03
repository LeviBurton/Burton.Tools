using Burton.Lib.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlanStatus { NoClosestNodeFound = -1 }

public class UnityPathPlanner
{
    public UnityGraph UnityGraph;
    public SparseGraph<UnityNode, UnityEdge> Graph;
    public Search_AStar<UnityNode, UnityEdge> Search;
    public List<PathEdge> PathToTarget = new List<PathEdge>();

    public UnityPathPlanner(UnityGraph UnityGraph, Search_AStar<UnityNode, UnityEdge> CurrentSearch)
    {
        this.Graph = UnityGraph.Graph;
        this.UnityGraph = UnityGraph;
        this.Search = CurrentSearch;
    }

    public ESearchStatus CycleOnce()
    {
        ESearchStatus Result = Search.CycleOnce();
        return Result;
    }
}
