using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burton.Lib.Graph;
using Burton.Lib.Math;
using Burton.Lib.GameEntity;

namespace Burton.Lib
{
    // This class will be responsible for quite a lot of things.
    // Make sure to document it well, and spend some time up-front
    // trying to get it "right."
    public class Map
    {
        // The maps associated navigation graph
        public SparseGraph<GraphNode, GraphEdge> NavGraph;

        // Spawn positions on the map where a bot could randomly spawn.
        public List<Vector2> SpawnPoints;

        // Cell space partitioner here.


        // Size of the search radius the cellspace partition uses when 
        // looking for neighbors.
        double CellSpaceNeighborhoodRange;

        public int SizeX;
        public int SizeY;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Map()
        {
            Log.Info("Map CTOR");
            Reset();
        }

        public void Reset()
        {
            Log.Info("Setup Starting");
            NavGraph = new SparseGraph<GraphNode, GraphEdge>(false);
            BaseGameEntity.ResetNextValidID();
    
            Log.Info("Setup Done");
        }

        public void PartitionNavGraph()
        {
          
        }

        public void SetToStartMap()
        {
            PartitionNavGraph();

        }

        public void LoadMap(string FileName)
        {
            Log.InfoFormat("LoadMap.FileName: {0}", FileName);

            Reset();

            NavGraph.Load(FileName);
        }
    }
}
