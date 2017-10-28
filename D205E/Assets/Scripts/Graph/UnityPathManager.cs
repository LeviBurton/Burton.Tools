using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UnityPathManager : MonoBehaviour
{
    public Action<List<int>> OnTargetFound;
    public Action OnTargetNotFound;

    public bool DrawSearchPaths = true;
    public int NumSearchCyclesPerUpdate;
    public Color StartNodeColor;
    public Color EndNodeColor;
    public Color DefaultSearchPathColor;

    public List<UnityPathPlanner> SearchRequests = new List<UnityPathPlanner>();
    public List<UnityPathPlanner> TargetFoundSearchRequests = new List<UnityPathPlanner>();

    public List<Search> Searches = new List<Search>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int CurSearchIndex = 0; CurSearchIndex < TargetFoundSearchRequests.Count; CurSearchIndex++)
        {
            var CurrentSearchRequest = TargetFoundSearchRequests[CurSearchIndex];

            var StartingNode = CurrentSearchRequest.Graph.GetNode(CurrentSearchRequest.Search.SourceNodeIndex);
            var EndNode = CurrentSearchRequest.Graph.GetNode(CurrentSearchRequest.Search.TargetNodeIndex);

            var PathToTarget = new List<UnityNode>();

            foreach (var NodeIndex in CurrentSearchRequest.Search.GetPathToTarget())
            {
                PathToTarget.Add(CurrentSearchRequest.Graph.GetNode(NodeIndex));
            }

            UnityNode CurrentNode = null;
            UnityNode NextNode = null;

            for (int i = 0; i < PathToTarget.Count; i++)
            {
                CurrentNode = PathToTarget.ElementAtOrDefault(i);
                NextNode = PathToTarget.ElementAtOrDefault(i + 1);

                Vector3 SpherePosition = new Vector3(CurrentNode.Position.x, (CurrentNode.Position.y + CurrentSearchRequest.UnityGraph.TileWidth / 4), CurrentNode.Position.z);
                Gizmos.DrawSphere(SpherePosition + (Vector3.up * 0.5f), 0.10f);

                if (NextNode != null)
                {
                    var FromPosition = new Vector3(CurrentNode.Position.x, 0.5f, CurrentNode.Position.z);
                    var ToPosition = new Vector3(NextNode.Position.x, 0.5f, NextNode.Position.z);

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(FromPosition, ToPosition);
                }
            }
        }
    }

    void Start()
    {
        SearchRequests = new List<UnityPathPlanner>();
        TargetFoundSearchRequests = new List<UnityPathPlanner>();
    }

    void Update()
    {
        UpdateSearches();
    }

    // FIXME -- interface? Generic?
    public void Register(UnityPathPlanner PathPlanner)
    {
        if (!SearchRequests.Contains(PathPlanner))
        {
            //  OnTargetFound += PathPlanner.OnTargetFound;
            SearchRequests.Add(PathPlanner);
        }
    }

    public void UnRegister(UnityPathPlanner PathPlanner)
    {
        //  OnTargetFound -= PathPlanner.OnTargetFound;
        SearchRequests.Remove(PathPlanner);
    }

    public int GetNumActiveSearches()
    {
        return SearchRequests.Count;
    }

    public void UpdateSearches()
    {
        int NumCyclesRemaining = NumSearchCyclesPerUpdate;
        int CurSearchIndex = 0;

        while (NumCyclesRemaining-- > 0 && SearchRequests.Any())
        {
            var SearchRequest = SearchRequests[CurSearchIndex];

            ESearchStatus Result = SearchRequest.CycleOnce();

            if (Result == ESearchStatus.TargetFound)
            {
                TargetFoundSearchRequests.Add(SearchRequest);
                SearchRequests.RemoveAt(CurSearchIndex);
            }
            else if (Result == ESearchStatus.TargetNotFound)
            {
                SearchRequests.RemoveAt(CurSearchIndex);
            }
            else
            {
                // go to next path
                CurSearchIndex++;
            }

            // if we are at the end, reset to beginning.
            if (CurSearchIndex >= SearchRequests.Count)
            {
                CurSearchIndex = 0;
            }
        }
    }
}
