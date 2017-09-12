using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GraphVisualizerTest
{
    public partial class Form_Main : Form
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SparseGraph<NavGraphNode, GraphEdge> Graph;
        public TileMap CurrentMap;

        // List of brush type ids that represent the tiles brush.
        List<EBrushType> Terrain = new List<EBrushType>();
        List<NavGraphNode> Path = new List<NavGraphNode>();
        List<GraphEdge> SubTree = new List<GraphEdge>();

        TileImageManager TileImageManager;

        public List<TileBrushManager> TileBrushManagers = new List<GraphVisualizerTest.TileBrushManager>();

        public EBrushType CurrentBrushType;

        public int SourceNode;
        public int TargetNode;
        public int GridWidthPx = 0;
        public int GridHeightPx = 0;
        public int NumCellsX = 13;
        public int NumCellsY = 7;
        public int BigCircle = 12;
        public int MediumCircle = 5;
        public int SmallCircle = 2;
        public int CellWidth = 128 ;
        public int CellHeight = 128;

        public bool bIsPaintingTerrain;

        public Form_Main()
        {
            InitializeComponent();
            InitDoubleBuffering();
        }

        private void InitDoubleBuffering()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Double buffer our panel.  This allows us to do it without subclassing Panel.
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                GridPanel,
                new object[] { true });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            Setup();
        }

        private void Setup()
        {
            CurrentMap = new TileMap(NumCellsY, NumCellsX);
            CurrentMap.Setup();
            CurrentMap.ClearTerrain();

            CurrentBrushType = EBrushType.Source;
            TileImageManager = new TileImageManager();
            bIsPaintingTerrain = false;
            GridPanel.Width = CurrentMap.TileMapWidth;
            GridPanel.Height = CurrentMap.TileMapHeight;
           
            Size = new System.Drawing.Size(GridPanel.Width + 35, GridPanel.Height + 100);
            Width = GridPanel.Width;
            Height = GridPanel.Height;

            var BrushTool = new Form_Palette_Brush();

            var SimpleBrushManager = new TileBrushManager("Simple");
            SimpleBrushManager.Add(new TileBrush(Color.Red.ToString(), Color.Red, 64, 64));
            SimpleBrushManager.Add(new TileBrush(Color.Green.ToString(), Color.Green, 64, 64));
            SimpleBrushManager.Add(new TileBrush(Color.Blue.ToString(), Color.Blue, 64, 64));
            //TileBrushManagers.Add(SimpleBrushManager);

            var ColorfulBrushManager = new TileBrushManager("Colorful");
            ColorfulBrushManager.Add(new TileBrush(Color.Cyan.ToString(), Color.Cyan, 64, 64));
            ColorfulBrushManager.Add(new TileBrush(Color.Magenta.ToString(), Color.Magenta, 64, 64));
            ColorfulBrushManager.Add(new TileBrush(Color.Yellow.ToString(), Color.Yellow, 64, 64));
            ColorfulBrushManager.Add(new TileBrush(Color.DarkSlateGray.ToString(), Color.DarkSlateGray, 64, 64));
            //TileBrushManagers.Add(ColorfulBrushManager);

            //SimpleBrushManager.SaveBrushes("rgb.brushes");
            //ColorfulBrushManager.SaveBrushes("colorful.brushes");

            // load the brushes from the file system into their brush managers. 
            SimpleBrushManager.Save("rgb.brushes");
            TileBrushManagers.Add(SimpleBrushManager);

            ColorfulBrushManager.Save("colorful.brushes");
            TileBrushManagers.Add(ColorfulBrushManager);

            var Window_TileBrushManager = new Window_TileBrushManager(ColorfulBrushManager);
            Window_TileBrushManager.Owner = this;
            Window_TileBrushManager.Text = ColorfulBrushManager.Path;
          //  Window_TileBrushManager.Show();

            var Window_SimpleBrushManager = new Window_TileBrushManager(SimpleBrushManager);
            Window_SimpleBrushManager.Owner = this;
            Window_SimpleBrushManager.Text = SimpleBrushManager.Path;
          //  Window_SimpleBrushManager.Show();

            this.GridPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseMove);
            this.GridPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseDown);
            this.GridPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseUp);
        }

        private void GridPanel_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentMap != null)
            {
                CurrentMap.Paint(sender, e);
            }
        }

        private void CreatePathAStar()
        {
            //CurrentMap.SubTree.Clear();
            //CurrentMap.Path.Clear();

            //Stopwatch Stopwatch = new Stopwatch();
            //Stopwatch.Start();
            //var AStar = new GraphSearchAStar<NavGraphNode, GraphEdge>(CurrentMap.Graph, CurrentMap.SourceNode, CurrentMap.TargetNode);
            //AStar.Search();
            //Stopwatch.Stop();

            //if (AStar.bFound)
            //{
            //    var PathToTarget = AStar.GetPathToTarget();
            //    CurrentMap.SubTree = AStar.ShortestPathTree;

            //    foreach (var NodeIndex in PathToTarget)
            //    {
            //        var Node = (NavGraphNode)CurrentMap.Graph.GetNode(NodeIndex);
            //        CurrentMap.Path.Add(Node);
            //    }

            //    // Movement cost is simply the number of nodes in the path to the target.
            //    int MovementCost = PathToTarget.Count - 1;
            //    Console.WriteLine(MovementCost);
            //}

            //GridPanel.Refresh();
        }
        //private void CreatePathDijkstra()
        //{
        //    SubTree.Clear();
        //    Path.Clear();

        //    Stopwatch Stopwatch = new Stopwatch();
        //    Stopwatch.Start();
        //    var Dijkstra = new GraphSearchDijkstra(Graph, SourceNode, TargetNode);
        //    Dijkstra.Search();
        //    Stopwatch.Stop();

        //    if (Dijkstra.bFound)
        //    {
        //        var PathToTarget = Dijkstra.GetPathToTarget();
        //        SubTree = Dijkstra.ShortestPathTree;

        //        foreach (var NodeIndex in PathToTarget)
        //        {
        //            var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
        //            Path.Add(Node);
        //        }
        //    }

        //    GridPanel.Refresh();
        //}
        //private void CreatePathBFS()
        //{
        //    SubTree.Clear();
        //    Path.Clear();

        //    Stopwatch Stopwatch = new Stopwatch();
        //    Stopwatch.Start();
        //    var BFS = new GraphSearchBFS(Graph, SourceNode, TargetNode);
        //    BFS.Search();
        //    Stopwatch.Stop();

        //    if (BFS.bFound)
        //    {
        //        var PathToTarget = BFS.GetPathToTarget();
        //        SubTree = BFS.TraversedEdges;

        //        foreach (var NodeIndex in PathToTarget)
        //        {
        //            var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
        //            Path.Add(Node);
        //        }
        //    }

        //    GridPanel.Refresh();
        //}
        //private void CreatePathDFS()
        //{
        //    SubTree.Clear();
        //    Path.Clear();

        //    Stopwatch Stopwatch = new Stopwatch();
        //    Stopwatch.Start();
        //    var DFS = new GraphSearchDFS(Graph, SourceNode, TargetNode);    
        //    DFS.Search();
        //    Stopwatch.Stop();
           
        //    if (DFS.bFound)
        //    {
        //        var PathToTarget = DFS.GetPathToTarget();
        //        SubTree = DFS.SpanningTree;

        //        foreach (var NodeIndex in PathToTarget)
        //        {
        //            var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
        //            Path.Add(Node);
        //        }
        //    }

        //    GridPanel.Refresh();
        //    //Console.WriteLine(string.Format("Elapsed Search Time: {0}", Stopwatch.Elapsed.ToString()));
        //}

        private void ChangeBrush(EBrushType NewBrush)
        {
            CurrentBrushType = NewBrush;
        }

        private void PaintTerrain(PointF Point)
        {
            if (CurrentMap == null || CurrentMap.Graph == null)
                return;

            int TileIndex = (int)Point.Y / CurrentMap.TileWidth  * CurrentMap.NumCellsX + (int)Point.X / CurrentMap.TileHeight;

            if ((Point.X > CurrentMap.NumCellsX * CurrentMap.TileWidth || Point.Y > CurrentMap.NumCellsY * CurrentMap.TileHeight) ||
                (Point.X < 0 || Point.Y < 0) || 
                TileIndex >= CurrentMap.Graph.NodeCount())
            {
                Console.WriteLine("Ignoreing: {0} {1}", TileIndex, CurrentMap.Graph.NodeCount());
                return;
            }

            bool bShouldSearch = false;

            if ( (CurrentBrushType == EBrushType.Source) || (CurrentBrushType == EBrushType.Target))
            {
                if (CurrentBrushType == EBrushType.Source)
                {
                    SourceNode = TileIndex;
                    CurrentMap.SourceNode = TileIndex;
                    bShouldSearch = true;
                }
                else if (CurrentBrushType == EBrushType.Target)
                {
                    CurrentMap.TargetNode = TileIndex;
                    bShouldSearch = true;
                }
            }
            else
            {
                CurrentMap.UpdateGraphFromBrush(TileIndex, CurrentBrushType);
                bShouldSearch = true;
            }

            GridPanel.Refresh();

            if (bShouldSearch)
            {
                
                // CreatePathBFS();
                // CreatePathDFS();
                //CreatePathBFS();
                // CreatePathDijkstra();
                CreatePathAStar();
            }
        }

        #region GridPanel Mouse Events
        private void GridPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (bIsPaintingTerrain)
                PaintTerrain(e.Location);
        }

        private void GridPanel_MouseDown(object sender, MouseEventArgs e)
        {
            bIsPaintingTerrain = true;

            PaintTerrain(e.Location);
        }

        private void GridPanel_MouseUp(object sender, MouseEventArgs e)
        {
            bIsPaintingTerrain = false;
        }

        #endregion

        #region Brush Type Selection
        private void SourceButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Source);
            }
        }

        private void TargetButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Target);
            }
        }

        private void ObstacleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Obstacle);
            }
        }

        private void WaterButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Water);
            }
        }

        private void MudButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Mud);
            }
        }

        private void NormalButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Normal);
            }
        }


        #endregion

        private void MenuItem_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Map file | *.map";
            OpenFileDialog.Title = "Open a Map File";
            OpenFileDialog.ShowDialog();

            if (OpenFileDialog.FileName != "")
            {
                CurrentMap.Load(OpenFileDialog.FileName);
                GridPanel.Width = CurrentMap.TileMapWidth;
                GridPanel.Height = CurrentMap.TileMapHeight;
                GridPanel.Refresh();
            }
        }

        private void MenuItem_SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "Map file | *.map";
            SaveFileDialog.Title = "Save a Map File";
            SaveFileDialog.ShowDialog();

            if (SaveFileDialog.FileName != "")
            {
                using (Stream OutStream = File.Open(SaveFileDialog.FileName, FileMode.Create))
                {
                    var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    BinaryFormatter.Serialize(OutStream, NumCellsX);
                    BinaryFormatter.Serialize(OutStream, NumCellsY);
                    BinaryFormatter.Serialize(OutStream, Terrain);
                }
            }
        }

        private void MenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuItem_View_Brushes_Click(object sender, EventArgs e)
        {

        }

        private void MenuItem_View_BrushManager_Click(object sender, EventArgs e)
        {

        }
    }

    public enum EBrushType
    {
        Normal = 0,
        Obstacle = 1,
        Water = 2,
        Mud = 3,
        Source = 4,
        Target = 5
    }

    #region Misc
 
    #endregion

}
