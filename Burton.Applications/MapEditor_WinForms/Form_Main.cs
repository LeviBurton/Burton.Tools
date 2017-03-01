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
        public SparseGraph<GraphNode, GraphEdge> Graph;
      
        // List of brush type ids that represent the tiles brush.
        List<EBrushType> Terrain = new List<EBrushType>();
        List<NavGraphNode> Path = new List<NavGraphNode>();
        List<GraphEdge> SubTree = new List<GraphEdge>();

        ResourceManager ResourceManager;

        public List<TileBrushManager> TileBrushManagers = new List<GraphVisualizerTest.TileBrushManager>();

        public EBrushType CurrentBrushType;

        public int SourceNode;
        public int TargetNode;
        public int GridWidthPx = 0;
        public int GridHeightPx = 0;
        public int NumCellsX = 10;
        public int NumCellsY = 5;
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
            ResourceManager = new ResourceManager();
            ResourceManager.LoadResources("Content_NoSC");

            Setup();
        }

        private void Setup()
        {
            
            Graph = new SparseGraph<GraphNode, GraphEdge>(false, NumCellsX * NumCellsY);

            var ContentFiles = Directory.GetDirectories("Content_NoSC");

            foreach (var File in ContentFiles)
            {
                Console.WriteLine(File.ToString());
            }

            GridWidthPx = CellWidth * NumCellsX;
            GridHeightPx = CellHeight * NumCellsY;

            CurrentBrushType = EBrushType.Source;

            bIsPaintingTerrain = false;

            for (int i = 0; i < NumCellsX * NumCellsY; i++)
            {
                Terrain.Insert(i, EBrushType.Normal);
            }

            CreateGrid(Graph, NumCellsX, NumCellsY);

            GridPanel.Width = GridWidthPx;
            GridPanel.Height = GridHeightPx;

            Size = new System.Drawing.Size(GridPanel.Width + 35, GridPanel.Height + 100);

            Path.Clear();
            SubTree.Clear();

            SourceNode = 0;
            TargetNode = Graph.NodeCount() - 1;

            var BrushTool = new Form_Palette_Brush();

            var SimpleBrushManager = new TileBrushManager("Simple");
            SimpleBrushManager.AddBrush(new TileBrush(Color.Red.ToString(), Color.Red, 64, 64));
            SimpleBrushManager.AddBrush(new TileBrush(Color.Green.ToString(), Color.Green, 64, 64));
            SimpleBrushManager.AddBrush(new TileBrush(Color.Blue.ToString(), Color.Blue, 64, 64));
            //TileBrushManagers.Add(SimpleBrushManager);

            var ColorfulBrushManager = new TileBrushManager("Colorful");
            ColorfulBrushManager.AddBrush(new TileBrush(Color.Cyan.ToString(), Color.Cyan, 64, 64));
            ColorfulBrushManager.AddBrush(new TileBrush(Color.Magenta.ToString(), Color.Magenta, 64, 64));
            ColorfulBrushManager.AddBrush(new TileBrush(Color.Yellow.ToString(), Color.Yellow, 64, 64));
            ColorfulBrushManager.AddBrush(new TileBrush(Color.DarkSlateGray.ToString(), Color.DarkSlateGray, 64, 64));
            //TileBrushManagers.Add(ColorfulBrushManager);

            //SimpleBrushManager.SaveBrushes("rgb.brushes");
            //ColorfulBrushManager.SaveBrushes("colorful.brushes");

            // load the brushes from the file system into their brush managers. 
            SimpleBrushManager.SaveBrushes("rgb.brushes");
            TileBrushManagers.Add(SimpleBrushManager);

            ColorfulBrushManager.SaveBrushes("colorful.brushes");
            TileBrushManagers.Add(ColorfulBrushManager);

            var Window_TileBrushManager = new Window_TileBrushManager(ColorfulBrushManager);
            Window_TileBrushManager.Owner = this;
            Window_TileBrushManager.Text = ColorfulBrushManager.BrushesFile;
            Window_TileBrushManager.Show();

            var Window_SimpleBrushManager = new Window_TileBrushManager(SimpleBrushManager);
            Window_SimpleBrushManager.Owner = this;
            Window_SimpleBrushManager.Text = SimpleBrushManager.BrushesFile;
            Window_SimpleBrushManager.Show();

            ////CreatePathDFS();
            // CreatePathBFS();
            //CreatePathDijkstra();
            CreatePathAStar();

            this.GridPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseMove);
            this.GridPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseDown);
            this.GridPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseUp);
        }

        public static bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
        {
            return !((x < 0) || (x >= NumCellsX) || (y < 0) || (y >= NumCellsY));
        }

        public void AddAllNeighborsToGridNode(SparseGraph<GraphNode, GraphEdge> Graph, int Row, int Col, int CellsX, int CellsY)
        {
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    int NodeX = (Col + j);
                    int NodeY = (Row + i);

                    if ((i == 0) && (j == 0))
                        continue;

                    if (ValidNeighbor(NodeX, NodeY, CellsX, CellsY))
                    {
                        var Node = (NavGraphNode)Graph.GetNode(Row * CellsX + Col);

                        if (Node.NodeIndex == -(int)ENodeType.InvalidNodeIndex)
                            continue;

                        var NeighborNode = (NavGraphNode)Graph.GetNode(NodeY * CellsX + NodeX);

                        if (NeighborNode.NodeIndex == (int)ENodeType.InvalidNodeIndex)
                            continue;

                        var PosNode = new Vector2(Node.X, Node.Y);
                        var PosNeighborNode = new Vector2(NeighborNode.X, NeighborNode.Y);

                        double Distance = PosNode.Distance(PosNeighborNode);
                   
                        GraphEdge NewEdge = new GraphEdge(Node.NodeIndex, NeighborNode.NodeIndex, Distance);
                        Graph.AddEdge(NewEdge);

                        if (!Graph.IsDigraph())
                        {
                            GraphEdge Edge = new GraphEdge(NeighborNode.NodeIndex, Node.NodeIndex, Distance);
                            Graph.AddEdge(Edge);
                        }
                    }
                }
            }
        }

        public void WeightNavGraphNodeEdges(SparseGraph<GraphNode, GraphEdge> Graph, int NodeIndex, double Weight)
        {
            if (NodeIndex > Graph.NodeCount())
            {
                throw new ArgumentException(string.Format("{0} out of bounds: {1}", NodeIndex, Graph.NodeCount()));
            }

            foreach (var Edge in Graph.Edges[NodeIndex])
            {
                var NodeFrom = (NavGraphNode)Graph.GetNode(Edge.FromNodeIndex);
                var NodeTo = (NavGraphNode)Graph.GetNode(Edge.ToNodeIndex);
                var PosFrom = new Vector2(NodeFrom.X, NodeFrom.Y);
                var PosTo = new Vector2(NodeTo.X, NodeTo.Y);

                double Distance = PosFrom.Distance(PosTo);

                Graph.SetEdgeCost(Edge.FromNodeIndex, Edge.ToNodeIndex, Distance * Weight);
            }
        }

        public void CreateGrid(SparseGraph<GraphNode, GraphEdge> Graph, int CellsX, int CellsY)
        {
            //CellWidth = GridWidthPx / CellsX;
            //CellHeight = GridHeightPx / CellsY;

            //CellWidth = 32;
            //CellHeight = 32;

         
            float MidX = CellWidth / 2;
            float MidY = CellHeight / 2;

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    var NodeIndex = Graph.AddNode(new NavGraphNode(Graph.GetNextFreeNodeIndex(),
                                                                   MidX + (Col * CellWidth), MidY + (Row * CellHeight)));
  
                }
            }

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    AddAllNeighborsToGridNode(Graph, Row, Col, CellsX, CellsY);
                }
            }
        }


        private void GridPanel_Paint(object sender, PaintEventArgs e)
        {
            // Draw Grid, Terrain, Nodes, Edges and Labels
            for (int CurNodeIndex = 0; CurNodeIndex < Graph.NodeCount(); ++CurNodeIndex)
            {
                var Node = (NavGraphNode)Graph.GetNode(CurNodeIndex);

                if (Node.NodeIndex == (int)ENodeType.InvalidNodeIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                    continue;
                }

                if (Terrain[Node.NodeIndex] == EBrushType.Normal)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Obstacle)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Water)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Mud)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;

               // e.Graphics.DrawString(string.Format("{0}", Node.NodeIndex), Font, Brushes.Black, new PointF((float)Node.X - 2.0f, (float)Node.Y - 10.0f));
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new RectangleF((float)Node.X - SmallCircle, (float)Node.Y - SmallCircle, SmallCircle*2, SmallCircle*2));

                foreach (var Edge in Graph.Edges[Node.NodeIndex])
                {
                    var FromNode = Graph.GetNode(Edge.FromNodeIndex) as NavGraphNode;
                    var ToNode = Graph.GetNode(Edge.ToNodeIndex) as NavGraphNode;
                    e.Graphics.DrawLine(new Pen(Color.LightGray), new PointF((float)FromNode.X, (float)FromNode.Y), new PointF((float)ToNode.X, (float)ToNode.Y)); 
                }

                if (Node.NodeIndex == SourceNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Green), new RectangleF((float)Node.X - BigCircle, (float)Node.Y - BigCircle, BigCircle*2, BigCircle*2));
                }
                else if (Node.NodeIndex == TargetNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), new RectangleF((float)Node.X - BigCircle, (float)Node.Y - BigCircle, BigCircle*2, BigCircle*2));
                }

                e.Graphics.DrawRectangle(new Pen(Color.DarkGray), new Rectangle(new Point((int)Node.X - (CellWidth / 2), (int)Node.Y - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
            }

            // Draw subtree of searched nodes 
            if (SubTree != null && SubTree.Count > 0)
            {
                for (int i = 0; i < SubTree.Count; ++i)
                {
                    if (SubTree[i] != null)
                    {
                        var FromNode = Graph.GetNode(SubTree[i].FromNodeIndex) as NavGraphNode;
                        var ToNode = Graph.GetNode(SubTree[i].ToNodeIndex) as NavGraphNode;
                        var EdgePen = new Pen(Color.FromArgb(255, 25, 25, 25), 1);

                        e.Graphics.DrawLine(EdgePen, new PointF((float)FromNode.X, (float)FromNode.Y), new PointF((float)ToNode.X, (float)ToNode.Y));
                    }
                }
            }

            // Draw Path
            if (Path.Count > 0)
            {
                for (int i = 0; i < Path.Count - 1; i++)
                {
                    e.Graphics.DrawLine(new Pen(Color.Blue, 3), new PointF((float)Path[i].X, (float) Path[i].Y), 
                                                                new PointF((float)Path[i + 1].X, (float)Path[i + 1].Y));
                }
            }
        }

        private void CreatePathAStar()
        {
            SubTree.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var AStar = new GraphSearchAStar(Graph, SourceNode, TargetNode);
            AStar.Search();
            Stopwatch.Stop();

            if (AStar.bFound)
            {
                var PathToTarget = AStar.GetPathToTarget();
                SubTree = AStar.ShortestPathTree;

                foreach (var NodeIndex in PathToTarget)
                {
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                    Path.Add(Node);
                }

                // Movement cost is simply the number of nodes in the path to the target.
                int MovementCost = PathToTarget.Count - 1;
                Console.WriteLine(MovementCost);
            }

            GridPanel.Refresh();
        }
        private void CreatePathDijkstra()
        {
            SubTree.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var Dijkstra = new GraphSearchDijkstra(Graph, SourceNode, TargetNode);
            Dijkstra.Search();
            Stopwatch.Stop();

            if (Dijkstra.bFound)
            {
                var PathToTarget = Dijkstra.GetPathToTarget();
                SubTree = Dijkstra.ShortestPathTree;

                foreach (var NodeIndex in PathToTarget)
                {
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                    Path.Add(Node);
                }
            }

            GridPanel.Refresh();
        }

        private void CreatePathBFS()
        {
            SubTree.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var BFS = new GraphSearchBFS(Graph, SourceNode, TargetNode);
            BFS.Search();
            Stopwatch.Stop();

            if (BFS.bFound)
            {
                var PathToTarget = BFS.GetPathToTarget();
                SubTree = BFS.TraversedEdges;

                foreach (var NodeIndex in PathToTarget)
                {
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                    Path.Add(Node);
                }
            }

            GridPanel.Refresh();
        }

        private void CreatePathDFS()
        {
            SubTree.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var DFS = new GraphSearchDFS(Graph, SourceNode, TargetNode);    
            DFS.Search();
            Stopwatch.Stop();
           
            if (DFS.bFound)
            {
                var PathToTarget = DFS.GetPathToTarget();
                SubTree = DFS.SpanningTree;

                foreach (var NodeIndex in PathToTarget)
                {
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                    Path.Add(Node);
                }
            }

            GridPanel.Refresh();
            //Console.WriteLine(string.Format("Elapsed Search Time: {0}", Stopwatch.Elapsed.ToString()));
        }

        private void ChangeBrush(EBrushType NewBrush)
        {
            CurrentBrushType = NewBrush;
        }

        private void PaintTerrain(PointF Point)
        { 
            int TileIndex = (int)Point.Y / CellHeight  * NumCellsX + (int)Point.X / CellWidth;

            if ((Point.X > NumCellsX * CellWidth || Point.Y > NumCellsY * CellHeight) ||
                (Point.X < 0 || Point.Y < 0) || 
                TileIndex >= Graph.NodeCount())
            {
                Console.WriteLine("Ignoreing: {0} {1}", TileIndex, NumCellsX * NumCellsY);
                return;
            }

            bool bShouldSearch = false;

            if ( (CurrentBrushType == EBrushType.Source) || (CurrentBrushType == EBrushType.Target))
            {
                if (CurrentBrushType == EBrushType.Source)
                {
                    SourceNode = TileIndex;
                    bShouldSearch = true;
                }
                else if (CurrentBrushType == EBrushType.Target)
                {
                    TargetNode = TileIndex;
                    bShouldSearch = true;
                }
            }
            else
            {
                UpdateGraphFromBrush(TileIndex, CurrentBrushType);
                bShouldSearch = true;
            }

            if (bShouldSearch)
            {
               // CreatePathBFS();
                // CreatePathDFS();
                //CreatePathBFS();
               // CreatePathDijkstra();
               CreatePathAStar();
            }
        }

        public void UpdateGraphFromBrush(int TileIndex, EBrushType Brush)
        {
            Terrain[TileIndex] = Brush;

            if (Brush == EBrushType.Obstacle)
            {
                Graph.RemoveNode(TileIndex);
            }
            else
            {
                // make the node active again if it is currently inactive
                if (!Graph.IsNodePresent(TileIndex))
                {
                    int y = (TileIndex / NumCellsX) ;
                    int x = TileIndex - (y * NumCellsX);
                    float MidX = CellWidth / 2;
                    float MidY = CellHeight / 2;

                    Vector2 Position = new Vector2(MidX + (x * CellWidth), MidY + (y * CellHeight));
                    var NodeIndex = Graph.AddNode(new NavGraphNode(TileIndex, Position.x, Position.y));

                    AddAllNeighborsToGridNode(Graph, y, x, NumCellsX, NumCellsY);
                }

                double TerrainCost = 1.0;

                if (CurrentBrushType == EBrushType.Water)
                {
                    TerrainCost = 2.0;
                }
                else if (CurrentBrushType == EBrushType.Normal)
                {
                    TerrainCost = 1.0;
                }
                else if (CurrentBrushType == EBrushType.Mud)
                {
                    TerrainCost = 1.5;
                }

                WeightNavGraphNodeEdges(Graph, TileIndex, TerrainCost);

            }

            Terrain[TileIndex] = Brush;
        }

        public void WeightNavGraphNodeEdges(int NodeIndex, float Weight)
        {

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
                using (Stream InStream = File.Open(OpenFileDialog.FileName, FileMode.Open))
                {
                    var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    NumCellsX = (int)BinaryFormatter.Deserialize(InStream);
                    NumCellsY = (int)BinaryFormatter.Deserialize(InStream);

                    Terrain = null;
                    Graph = null;

                    Graph = new SparseGraph<GraphNode, GraphEdge>(false, NumCellsX * NumCellsY);
                    Terrain = (List<EBrushType>)BinaryFormatter.Deserialize(InStream);
               
                    CreateGrid(Graph, NumCellsX, NumCellsY);

                    for (int i = 0; i < NumCellsX * NumCellsY; i++)
                    {
                        UpdateGraphFromBrush(i, Terrain[i]);
                    }

                    CreatePathAStar();
                
                    GridPanel.Refresh();
                }
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
 
    public class Vector2
    {
        public double x;
        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //------------------------------------------------------------------------
        public double Distance(Vector2 v2)
        {
            double ySeparation = v2.y - y;
            double xSeparation = v2.x - x;

            return Math.Sqrt(ySeparation * ySeparation + xSeparation * xSeparation);
        }
    }
    #endregion

}
