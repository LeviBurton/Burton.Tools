using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public int CellWidth;
        public int CellHeight;
        List<NavGraphNode> Path = new List<NavGraphNode>();
        List<GraphEdge> TraversedEges = new List<GraphEdge>();
        public EBrushType CurrentBrushType;

        public static bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
        {
            return !((x < 0) || (x >= NumCellsX) || (y < 0) || (y >= NumCellsY));
        }

        public static void AddAllNeighborsToGridNode(SparseGraph<GraphNode, GraphEdge> Graph, int Row, int Col, int CellsX, int CellsY)
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
                    }
                }
            }
        }

        public static void CreateGrid(SparseGraph<GraphNode, GraphEdge> Graph, int CellsX, int CellsY)
        {
            int GridWidthPx = 700;
            int GridHeightPx = 700;

            int CellWidth = GridWidthPx / CellsX;
            int CellHeight = GridHeightPx / CellsY;

            double MidX = CellWidth / 2;
            double MidY = CellHeight / 2;

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    Graph.AddNode(new NavGraphNode(Graph.GetNextFreeNodeIndex(),
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
            Graph = new SparseGraph<GraphNode, GraphEdge>(true);
            CurrentBrushType = EBrushType.Source;

            CreateGrid(Graph, 10, 10);
            Path.Clear();

            Graph.RemoveNode(50);
            Graph.RemoveNode(20);
            Graph.RemoveNode(65);
            Graph.RemoveNode(53);
            Graph.RemoveNode(85);

            var DFS = new GraphSearchDFS(Graph, 30, 57);
            TraversedEges.Clear();

            var PathToTarget = DFS.GetPathToTarget();
            TraversedEges = DFS.TraversedEdges;

            foreach (var NodeIndex in PathToTarget)
            {
                var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                Path.Add(Node);
            }

            Console.Write("NomActiveNodes: {0}\nNumEdges: {1}\n", Graph.ActiveNodeCount(), Graph.EdgeCount());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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

                //e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(new Point((int)Node.LocationX - 35, (int)Node.LocationY - 35), new Size(70, 70)));

                e.Graphics.DrawString(string.Format("{0}", Node.NodeIndex), Font, Brushes.Black, new PointF((float)Node.LocationX - 15.0f, (float)Node.LocationY - 15.0f));
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new RectangleF((float)Node.LocationX - 5, (float)Node.LocationY - 5, 10, 10));

                foreach (var Edge in Graph.Edges[Node.NodeIndex])
                {
                    var FromNode = Graph.GetNode(Edge.FromNodeIndex) as NavGraphNode;
                    var ToNode = Graph.GetNode(Edge.ToNodeIndex) as NavGraphNode;

                    e.Graphics.DrawLine(new Pen(Color.LightGray), new PointF((float)FromNode.LocationX, (float)FromNode.LocationY), new PointF((float)ToNode.LocationX, (float)ToNode.LocationY)); 
                }
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
                e.Graphics.FillEllipse(new SolidBrush(Color.Green), new RectangleF((float)Node.LocationX - 7.5f, (float)Node.LocationY - 7.5f, 15, 15));
            }
        }

        #region Brush Type Selection
        private void SourceButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CurrentBrushType = EBrushType.Source;
            }
        }

        private void TargetButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CurrentBrushType = EBrushType.Target;
            }
        }

        private void ObstacleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CurrentBrushType = EBrushType.Obstacle;
            }
        }

        private void WaterButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CurrentBrushType = EBrushType.Water;
            }
        }

        private void MudButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CurrentBrushType = EBrushType.Mud;
            }
        }

        #endregion
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
