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

    private void OnDrawGizmos()
    {
        //if (DrawSearchPaths)
        //{
        //    foreach (var SearchRequest in SearchRequests)
        //    {
        //        var PathToTarget = SearchRequest.Search.GetPathToTarget();

        //        foreach (var NodeIndex in PathToTarget)
        //        {
        //            Debug.LogFormat("{0}", NodeIndex);

        //        }
        //    }
        //}
    }

    void Start()
    {
        SearchRequests = new List<UnityPathPlanner>();
	}

	void Update ()
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
            ESearchStatus Result = (SearchRequests[CurSearchIndex]).CycleOnce();

            if (Result == ESearchStatus.TargetFound)
            {
                var PathToTarget = SearchRequests[CurSearchIndex].Search.GetPathToTarget();

                //  Notify all interested parties that we found a path to the target.
                Debug.Log("\n");
                foreach (var p in PathToTarget)
                {
                    Debug.LogFormat("PathToTarget: {0}", p);
                }

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
