
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Burton.Lib.Alg;

/// <summary>
/// Represents the search state of a Node
/// </summary>
public enum NodeState
{
    /// <summary>
    /// The node has not yet been considered in any possible paths
    /// </summary>
    Untested,
    /// <summary>
    /// The node has been identified as a possible step in a path
    /// </summary>
    Open,
    /// <summary>
    /// The node has already been included in a path and will not be considered again
    /// </summary>
    Closed
}

public class PathNode : IComparable<PathNode>
{
    private PathNode InternalParentNode;

    /// <summary>
    ///  Estimated total cost (F = G + H)
    /// </summary>
    public float F { get { return G + H; } }

    /// <summary>
    /// Cost from start to here
    /// </summary>
    public float G { get; set; }

    /// <summary>
    /// Estimated cost from here to goal
    /// </summary>
    public float H { get; set; }

    public float Cost { get; set; }

    /// <summary>
    /// The nodes location in the grid
    /// </summary>
    public Point Location { get; set; }

    /// <summary>
    /// Tru when the node may be traversed, otherwise false
    /// </summary>
    public bool IsWalkable { get; set; }

    /// <summary>
    /// Flags whtehr the node is open, closed or untested 
    /// </summary>
    public NodeState State { get; set; }

    public PathNode ParentNode
    {
        get { return InternalParentNode; }
        set
        {
            // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
            InternalParentNode = value;
            G = InternalParentNode.G + GetTraversalCost(Location, InternalParentNode.Location);
        }
    }

    internal static float GetTraversalCost(Point location, Point otherLocation)
    {
        float deltaX = otherLocation.X - location.X;
        float deltaY = otherLocation.Y - location.Y;
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public PathNode()
    {
    }

    public override string ToString()
    {
        return string.Format("{0}, {1}: {2}", Location.X, Location.Y, State);
    }

    public int CompareTo(PathNode other)
    {
        if (F < other.F) return -1;
        else if (F > other.F) return 1;
        else return 0;
    }
}

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public List<Tile> Tiles;

        public int MapWidth;
        public int MapHeight;

        public int Rows = 20;
        public int Columns = 20;
        public int TileWidth = 25;
        public int TileHeight = 25;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Double buffer our panel.  This allows us to do it without subclassing Panel.
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                GridPanel,
                new object[] { true });
        }

        void ResetGrid()
        {
            Tiles = new List<Tile>(Rows * Columns);

            for (int Row = 0; Row < Rows; Row++)
            {
                for (int Col = 0; Col < Columns; Col++)
                {
                    var Index = Row * Rows + Col;
                    var Tile = new Tile();
                    Tile.OuterRect = new Rectangle(Col * TileWidth, Row * TileHeight, TileWidth, TileHeight);
                    Tile.InnerRect = Tile.OuterRect;
                    Tile.Location = Tile.OuterRect.Location;
                    Tile.TileName = string.Format("{0} {1}", Row, Col);
                    Tile.TileIndex = Index;
                    Tiles.Insert(Index, Tile);
                }
            }

            MapWidth = TileWidth * Columns;
            MapHeight = TileHeight * Rows;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ResetGrid();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Tiles.ForEach(x => x.OnPaint(e.Graphics));
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            var ClickedTile = Tiles.SingleOrDefault(x => x.OuterRect.Contains(e.Location));
            bool bRefreshGridPanel = false;

            if (ClickedTile == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                Tiles.Where(x => x.bIsGoalTile).ToList().ForEach(x => x.bIsGoalTile = false);
                bRefreshGridPanel = ClickedTile.OnRightMouseClick();
            }
            else if (e.Button == MouseButtons.Left)
            {
                Tiles.Where(x => x.bIsStartTile).ToList().ForEach(x => x.bIsStartTile = false);
                bRefreshGridPanel = ClickedTile.OnLeftMouseClick();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                bRefreshGridPanel = ClickedTile.OnMiddleMouseClick();
            }

            if (bRefreshGridPanel)
            {
                GridPanel.Refresh();
            }
        }

        private void Button_ResetTilesClicked(object sender, EventArgs e)
        {
            Tiles.ForEach(x => x.Reset());
            GridPanel.Refresh();
        }


        private void Button_FindPathClicked(object sender, EventArgs e)
        {
            Tiles.Where(x => x.TileType != TileType.Unwalkable).ToList().ForEach(x => x.Reset());
            Tiles.Where(x => x.TileType == TileType.Unwalkable).ToList().ForEach(x => { x.G = 0; x.H = 0; });
            PriorityQueue<Tile> OpenList = new PriorityQueue<Tile>();
            List<Tile> ClosedList = new List<Tile>();

            Tile StartingTile = null;
            Tile GoalTile = null;

            Tiles.ForEach(x =>
            {
                x.SearchList = SearchList.None;
                if (x.bIsStartTile)
                {
                    StartingTile = x;
                }
                else if (x.bIsGoalTile)
                {
                    GoalTile = x;
                }
            });

            StartingTile.SearchList = SearchList.Open;
            StartingTile.State = NodeState.Open;
     
            OpenList.Enqueue(StartingTile);

            do
            {
                // Get tile with lowest F score
                var CurrentTile = OpenList.Peek();

                // Add it to the closed list
                CurrentTile.State = NodeState.Closed;

                // Remove it from the OpenList
                OpenList.Dequeue();

                if (CurrentTile == GoalTile)
                    break;
              
                // Get Adjacent tiles to the current tile
                var AdjacentTiles = GetAdjacentWalkableTiles(CurrentTile);

                foreach (var Tile in AdjacentTiles)
                {
                    Tile.SearchList = SearchList.Open;
                    OpenList.Enqueue(Tile);
                }

            } while (OpenList.Count() > 0);

            List<Tile> Path = new List<Tile>();
            Tile PathTile = GoalTile;

            while (PathTile != StartingTile)
            {
                Path.Add(PathTile);
                PathTile = (Tile)PathTile.ParentNode;
            }
            Path.Reverse();

            foreach (var Tmp in Path)
            {
                Tmp.TileType = TileType.Path;
            }

            GridPanel.Refresh();
        }

        private List<Tile> GetAdjacentWalkableTiles(Tile StartingTile)
        {
            List<Tile> WalkableNodes = new List<Tile>();
            var NextTiles = GetAdjacentTiles(StartingTile);

            foreach (var Tile in NextTiles)
            {
                if (!Tile.IsWalkable)
                    continue;

                if (Tile.State == NodeState.Closed)
                    continue;

                if (Tile.State == NodeState.Open)
                {
                    float traversalCost = Tile.GetTraversalCost(Tile.Location, Tile.ParentNode.Location);
                    float gTemp = StartingTile.G + traversalCost;
                    if (gTemp < Tile.G)
                    {
                        if (Tile.ParentNode != StartingTile)
                            Tile.ParentNode = StartingTile;

                        WalkableNodes.Add(Tile);
                    }
                }
                else
                {
                    Tile.ParentNode = StartingTile;
                    Tile.State = NodeState.Open;
                    WalkableNodes.Add(Tile);
                }
            }

            return WalkableNodes;
        }

        private List<Tile> GetAdjacentTiles(Tile StartingTile)
        {
            int StartTileIndex = StartingTile.TileIndex;
            var AdjacentTiles = new List<Tile>();
            Tile CurrentTile = null;

            if (StartingTile.OuterRect.Location.Y - TileHeight >= 0)
            {
                var TopIndex = StartTileIndex - Rows;
                CurrentTile = Tiles[TopIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            if (StartingTile.OuterRect.Location.X + TileWidth < MapWidth &&
                StartingTile.OuterRect.Location.Y - TileWidth >= 0)
            {
                var TopRightIndex = (StartTileIndex - Rows) + 1;
                CurrentTile = Tiles[TopRightIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            if (StartingTile.OuterRect.Location.X + TileWidth < MapWidth)
            {
                var RightIndex = (StartTileIndex + 1);
                CurrentTile = Tiles[RightIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            if (StartingTile.OuterRect.Location.X + TileWidth < MapWidth &&
                StartingTile.OuterRect.Location.Y + TileHeight < MapHeight)
            {
                var BottomRightIndex = (StartTileIndex + Rows) + 1;
                CurrentTile = Tiles[BottomRightIndex];
                AdjacentTiles.Add(CurrentTile);
            }

         
            if (StartingTile.OuterRect.Location.Y + TileHeight < MapHeight)
            {
                var BottomIndex = (StartTileIndex + Rows);
                CurrentTile = Tiles[BottomIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            if (StartingTile.OuterRect.Location.Y + TileHeight < MapHeight &&
                StartingTile.OuterRect.Location.X > 0)
            {
                var BottomLeftIndex = (StartTileIndex + Rows) - 1;
                CurrentTile = Tiles[BottomLeftIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            if (StartingTile.OuterRect.Location.X > 0)
            {
                var LeftIndex = (StartTileIndex) - 1;
                CurrentTile = Tiles[LeftIndex];
                AdjacentTiles.Add(CurrentTile);
            }

       
            if (StartingTile.OuterRect.Location.X > 0 &&
                StartingTile.OuterRect.Location.Y > 0)
            {
                var TopLeftIndex = (StartTileIndex - Rows) - 1;
                CurrentTile = Tiles[TopLeftIndex];
                AdjacentTiles.Add(CurrentTile);
            }

            return AdjacentTiles;
        }
    }
}
