﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Graph
{
    public enum ESearchStatus { TargetFound, TargetNotFound, SearchIncomplete };

    public class PathManager<TPathPlanner> where TPathPlanner : PathPlanner
    {   
        // Active search requests
        private List<TPathPlanner> SearchRequests;

        // total number of search cycles allocated to the manager.
        // each update step these are divided equally amongst all registerd path
        // requests
        int NumSearchCyclesPerUpdate;


        public PathManager(int NumCyclesPerUpdate)
        {
            NumSearchCyclesPerUpdate = NumCyclesPerUpdate;
            SearchRequests = new List<TPathPlanner>();
        }

        // every time this is called the total amount of search cycles
        // available will be shared out equally between all the active 
        // path requests.  if a search completes sucessfully or fails
        // the method will notify the relevant bot
        public void UpdateSearches()
        {
            int NumCyclesRemaining = NumSearchCyclesPerUpdate;
            int CurSearchIndex = 0;

            while (NumCyclesRemaining-- > 0 && SearchRequests.Any())
            {
                int Result = (SearchRequests[CurSearchIndex]).CycleOnce();

                if ((Result == (int)ESearchStatus.TargetFound) ||
                    (Result == (int)ESearchStatus.TargetNotFound))
                {
                    // Remove path from list
                    SearchRequests.RemoveAt(CurSearchIndex);
                }
                else
                {
                    // go to next path
                    CurSearchIndex++;
                }

                // if we are at the end, reset to beginning.
                if (CurSearchIndex >= SearchRequests.Count())
                    CurSearchIndex = 0;
            }
        }

        public void Register(TPathPlanner PathPlanner)
        {
            if (!SearchRequests.Contains(PathPlanner))
            {
                SearchRequests.Add(PathPlanner);
            }
        }

        public void UnRegister(TPathPlanner PathPlanner)
        {
            SearchRequests.Remove(PathPlanner);
        }

        public int GetNumActiveSearches()
        {
            return SearchRequests.Count();
        }
    }
}