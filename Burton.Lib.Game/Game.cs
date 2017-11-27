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

        public Game()
        {
 
        }

        public void Setup()
        {
         
            LoadDefaultMap();

        }

        public void LoadDefaultMap()
        {
  
            Clear();

            PathManager = new PathManager<PathPlanner>(MaxSearchCyclesPerUpdateStep);

            Map = new Map();

            EntityManager.Instance.Reset();

        }

        public void LoadMap(string FileName)
        {
        
            Clear();

            PathManager = new PathManager<PathPlanner>(MaxSearchCyclesPerUpdateStep);
            Map = new Map();

  
            Map.LoadMap(FileName);

        }

        public void Clear()
        { }


    }
}
