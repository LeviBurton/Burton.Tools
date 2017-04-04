using Burton.Lib.GameEntity;
using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib
{
    public class Game
    {        
        // game map
        public Map Map;

        // enemies, etc
        public List<Bot> Bots;
        public Bot SelectedBot;

        // Cycles all the registered paths in the game.
        public PathManager<PathPlanner> PathManager;

        // are we paused?
        public bool bPaused;


        // Config properties

        // How many cycles a path planner can use during a step.
        // Higher values will favor throughput while lower values
        // will favor lower latency update cycles.
        // A good middle ground is 10.
        public int MaxSearchCyclesPerUpdateStep= 10;

        private static readonly log4net.ILog Log =
          log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Game()
        {
 
        }

        public void Setup()
        {
            Log.Info("Setup Starting");

            LoadDefaultMap();

            Log.Info("Setup Done");
        }

        public void LoadDefaultMap()
        {
            Log.Info("LoadDefaultMap Starting");

            Clear();

            PathManager = new PathManager<PathPlanner>(MaxSearchCyclesPerUpdateStep);

            Map = new Map();

            EntityManager.Instance.Reset();

            Log.Info("LoadDefaultMap Done");
        }

        public void LoadMap(string FileName)
        {
            Log.InfoFormat("LoadMap Starting");
            Log.InfoFormat("LoadMap Loading '{0}'", FileName);

            Clear();

            PathManager = new PathManager<PathPlanner>(MaxSearchCyclesPerUpdateStep);
            Map = new Map();

            EntityManager.Instance.Reset();

            Map.LoadMap(FileName);

            Log.InfoFormat("LoadMap Done");
        }

        public void Clear()
        { }


    }
}
