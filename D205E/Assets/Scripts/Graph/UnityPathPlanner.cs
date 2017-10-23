using Burton.Lib.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlanStatus { NoClosestNodeFound = -1 }

public class UnityPathPlanner
{
    public SparseGraph<UnityNode, UnityEdge> Graph;

    //Search_AStar<UnityNode, UnityEdge>(Graph, Heuristic, StartNodeIndex, EndNodeIndex);
    public Search_AStar<UnityNode, UnityEdge> Search;

    public UnityPathPlanner(SparseGraph<UnityNode, UnityEdge> Graph, Search_AStar<UnityNode, UnityEdge> CurrentSearch)
    {
        this.Graph = Graph;
        this.Search = CurrentSearch;
    }

    public ESearchStatus CycleOnce()
    {
        ESearchStatus Result = Search.CycleOnce();

        return Result;
    }
}
