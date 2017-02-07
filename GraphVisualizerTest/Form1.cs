using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizerTest
{
    public partial class Form1 : Form
    {
        public SparseGraph<GraphNode, GraphEdge> Graph;

        List<EBrushType> Terrain = new List<EBrushType>();
        List<NavGraphNode> Path = new List<NavGraphNode>();
        List<GraphEdge> TraversedEges = new List<GraphEdge>();

        public EBrushType CurrentBrushType;

        public int SourceNode;
        public int TargetNode;
        public int GridWidthPx = 600;
        public int GridHeightPx = 600;
        public int NumCellsX = 50;
        public int NumCellsY = 50;
        public int BigCircle = 10;
        public int MediumCircle = 5;
        public int SmallCircle = 2;
        public int CellWidth;
        public int CellHeight;

        public bool bIsPaintingTerrain;

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
                        var NeighborNode = (NavGraphNode)Graph.GetNode(NodeY * CellsX + NodeX);

                        var PosNode = new Vector2(Node.LocationX, Node.LocationY);
                        var PosNeighborNode = new Vector2(NeighborNode.LocationX, NeighborNode.LocationY);

                        double Distance = PosNode.Distance(PosNeighborNode);

                        GraphEdge NewEdge = new GraphEdge((Row) * CellsX + Col, NodeY * CellsY + NodeX, Distance);

                        Graph.AddEdge(NewEdge);

                        if (!Graph.IsDigraph())
                        {
                            GraphEdge Edge = new GraphEdge(NodeY * NumCellsX + NodeX, Row * NumCellsX + Col, Distance);
                            Graph.AddEdge(Edge);
                        }
                    }
                }
            }
        }

        public void CreateGrid(SparseGraph<GraphNode, GraphEdge> Graph, int CellsX, int CellsY)
        {
            CellWidth = GridWidthPx / CellsX;
            CellHeight = GridHeightPx / CellsY;

            double MidX = CellWidth / 2;
            double MidY = CellHeight / 2;
            Terrain.Capacity = CellsX * CellsY;

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    var NodeIndex = Graph.AddNode(new NavGraphNode(Graph.GetNextFreeNodeIndex(),
                                                    MidX + (Col * CellWidth), MidY + (Row * CellHeight)));

                    Terrain.Insert(NodeIndex, EBrushType.Normal);
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

        private void Form1_Load(object sender, EventArgs e)
        {     
            Graph = new SparseGraph<GraphNode, GraphEdge>(false);
            CurrentBrushType = EBrushType.Source;

            bIsPaintingTerrain = false;

            CreateGrid(Graph, NumCellsX, NumCellsY);
            Path.Clear();

            SourceNode = 8;
            TargetNode = 40;

            //CreatePathDFS();
            CreatePathBFS();

            this.GridPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseMove);
            this.GridPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseDown);
            this.GridPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridPanel_MouseUp);
        }

        private void GridPanel_Paint(object sender, PaintEventArgs e)
        {
            for (int CurNodeIndex = 0; CurNodeIndex < Graph.NodeCount(); ++CurNodeIndex)
            {
                var Node = (NavGraphNode)Graph.GetNode(CurNodeIndex);
                if (Node.NodeIndex == (int)ENodeType.InvalidNodeIndex)
                    continue;

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;

                if (Terrain[Node.NodeIndex] == EBrushType.Normal)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point((int)Node.LocationX - (CellWidth / 2), (int)Node.LocationY - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Obstacle)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point((int)Node.LocationX - (CellWidth / 2), (int)Node.LocationY - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
                }

              //  e.Graphics.DrawString(string.Format("{0}", Node.NodeIndex), Font, Brushes.Black, new PointF((float)Node.LocationX - 15.0f, (float)Node.LocationY - 15.0f));
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new RectangleF((float)Node.LocationX - SmallCircle, (float)Node.LocationY - SmallCircle, SmallCircle*2, SmallCircle*2));

                foreach (var Edge in Graph.Edges[Node.NodeIndex])
                {
                    var FromNode = Graph.GetNode(Edge.FromNodeIndex) as NavGraphNode;
                    var ToNode = Graph.GetNode(Edge.ToNodeIndex) as NavGraphNode;
                    e.Graphics.DrawLine(new Pen(Color.LightGray), new PointF((float)FromNode.LocationX, (float)FromNode.LocationY), new PointF((float)ToNode.LocationX, (float)ToNode.LocationY)); 
                }

                if (Node.NodeIndex == SourceNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Green), new RectangleF((float)Node.LocationX - BigCircle, (float)Node.LocationY - BigCircle, BigCircle*2, BigCircle*2));
                }

                else if (Node.NodeIndex == TargetNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), new RectangleF((float)Node.LocationX - BigCircle, (float)Node.LocationY - BigCircle, BigCircle*2, BigCircle*2));
                }

                e.Graphics.DrawRectangle(new Pen(Color.DarkGray), new Rectangle(new Point((int)Node.LocationX - (CellWidth / 2), (int)Node.LocationY - (CellHeight / 2)), new Size(CellWidth, CellHeight)));
            }

            foreach (var Edge in TraversedEges)
            {
                var FromNode = Graph.GetNode(Edge.FromNodeIndex) as NavGraphNode;
                var ToNode = Graph.GetNode(Edge.ToNodeIndex) as NavGraphNode;
                var EdgePen = new Pen(Color.Black, 2);

                e.Graphics.DrawLine(EdgePen, new PointF((float)FromNode.LocationX, (float)FromNode.LocationY), new PointF((float)ToNode.LocationX, (float)ToNode.LocationY));
            }

            foreach (var Node in Path)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), new RectangleF((float)Node.LocationX - MediumCircle, (float)Node.LocationY - MediumCircle, MediumCircle * 2, MediumCircle * 2));
            }


        }

        private void CreatePathBFS()
        {
            TraversedEges.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var BFS = new GraphSearchBFS(Graph, SourceNode, TargetNode);
            BFS.Search();
            Stopwatch.Stop();

            if (BFS.bFound)
            {
                var PathToTarget = BFS.GetPathToTarget();
                TraversedEges = BFS.TraversedEdges;

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
            TraversedEges.Clear();
            Path.Clear();

            Stopwatch Stopwatch = new Stopwatch();
            Stopwatch.Start();
            var DFS = new GraphSearchDFS(Graph, SourceNode, TargetNode);    
            DFS.Search();
            Stopwatch.Stop();
           
            if (DFS.bFound)
            {
                var PathToTarget = DFS.GetPathToTarget();
                TraversedEges = DFS.TraversedEdges;

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

            if (TileIndex < 0 || TileIndex > NumCellsX * NumCellsY)
            {
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
                UpdateGraphFromBrush(CurrentBrushType, TileIndex);
                bShouldSearch = true;
            }

            if (bShouldSearch)
            {
                //CreatePathDFS();
                CreatePathBFS();
            }

          //  Console.Write(string.Format("{0} {1}\n", TileIndex, CurrentBrushType.ToString()));
        }

        public void UpdateGraphFromBrush(EBrushType Brush, int TileIndex)
        {
            Terrain[TileIndex] = Brush;

            if (Brush == EBrushType.Obstacle)
            {
                Graph.RemoveNode(TileIndex);
            }
            else
            {
                //make the node active again if it is currently inactive
                if (!Graph.IsNodePresent(TileIndex))
                {
                    int y = TileIndex / NumCellsY;
                    int x = TileIndex - (y * NumCellsY);
                    double MidX = CellWidth / 2;
                    double MidY = CellHeight / 2;
                    Console.WriteLine(string.Format("{0} {1} {2}", TileIndex, x, y));

                    Vector2 Position = new Vector2(MidX + (x * CellWidth), MidY + (y * CellHeight));
                    var NodeIndex = Graph.AddNode(new NavGraphNode(TileIndex, Position.x, Position.y));


                    AddAllNeighborsToGridNode(Graph, y, x, NumCellsX, NumCellsY);


                }
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

        #endregion

        private void NormalButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ChangeBrush(EBrushType.Normal);
            }
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
    public class NavGraphNode : GraphNode
    {
        public double LocationX;
        public double LocationY;

        public NavGraphNode(int Index, double LocationX, double LocationY)
        {
            this.NodeIndex = Index;
            this.LocationX = LocationX;
            this.LocationY = LocationY;
        }
    }

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
