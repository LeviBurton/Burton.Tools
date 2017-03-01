using Burton.Lib.Graph;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapEditor_WPF
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return this.Name + ", " + this.Age + " years old";
        }
        public string Mail { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SparseGraph<GraphNode, GraphEdge> Graph;

        List<EBrushType> Terrain = new List<EBrushType>();
        List<NavGraphNode> Path = new List<NavGraphNode>();
        List<GraphEdge> SubTree = new List<GraphEdge>();

        public EBrushType CurrentBrushType;

        public int SourceNode;
        public int TargetNode;
        public int GridWidthPx = 675;
        public int GridHeightPx = 655;
        public int NumCellsX = 10;
        public int NumCellsY = 10;
        public int BigCircle = 12;
        public int MediumCircle = 5;
        public int SmallCircle = 2;
        public double CellWidth;
        public double CellHeight;

        public bool bIsPaintingTerrain;

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            Setup();
        }

        private void Setup()
        {
            Graph = new SparseGraph<GraphNode, GraphEdge>(false, NumCellsX * NumCellsY);

            CurrentBrushType = EBrushType.Source;

            bIsPaintingTerrain = false;

            for (int i = 0; i < NumCellsX * NumCellsY; i++)
            {
                Terrain.Insert(i, EBrushType.Normal);
            }

            CreateGrid(Graph, NumCellsX, NumCellsY);

            //  GridPanel.Width = CellWidth * NumCellsX;
            //   GridPanel.Height = CellHeight * NumCellsY;
            //  Size = new System.Drawing.Size(GridPanel.Width + 35, GridPanel.Height + 100);


            Path.Clear();
            SubTree.Clear();

            SourceNode = 5;
            TargetNode = 60;

            ////CreatePathDFS();
            // CreatePathBFS();
            //CreatePathDijkstra();
            CreatePathAStar();
        }

        private void CreatePathAStar()
        {
            SubTree.Clear();
            Path.Clear();
            SearchPath.Points.Clear();

            var AStar = new GraphSearchAStar(Graph, SourceNode, TargetNode);
            AStar.Search();

            if (AStar.bFound)
            {
                var PathToTarget = AStar.GetPathToTarget();
                SubTree = AStar.ShortestPathTree;

                foreach (var NodeIndex in PathToTarget)
                {
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);
                    Path.Add(Node);
                    SearchPath.Points.Add(new Point(Node.X, Node.Y));
                }
            }
        }

        #region Menu Click Handlers
        private void File_Save_Click(object sender, EventArgs e)
        {

        }

        private void File_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Map file | *.map";
            OpenFileDialog.Title = "Open a Map File";

            if (OpenFileDialog.ShowDialog() == true)
            {
                Console.WriteLine("OpenFile: {0}", OpenFileDialog.FileName);
            }

        }
        #endregion

        #region Old, but still in use!  Need to generalize and put somewhere more useful!

        public void CreateGrid(SparseGraph<GraphNode, GraphEdge> Graph, int CellsX, int CellsY)
        {
            CellWidth = GridWidthPx / CellsX;
            CellHeight = GridHeightPx / CellsY;
            // Size = new System.Drawing.Size(GridPanel.Width + 35, GridPanel.Height + 100);
            double MidX = CellWidth / 2;
            double MidY = CellHeight / 2;

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    var NodeIndex = Graph.AddNode(new NavGraphNode(Graph.GetNextFreeNodeIndex(),
                                                                   MidX + (Col * CellWidth), MidY + (Row * CellHeight)));
                    var Node = (NavGraphNode)Graph.GetNode(NodeIndex);

                    var VisualGraphNode = new VisualGraphNode()
                    {
                        CenterPoint = new Point(Node.X, Node.Y),
                        Name = Node.NodeIndex.ToString(),
                        BoundingRect = new Rect(Node.X - CellWidth / 2, Node.Y - CellHeight / 2, CellWidth, CellHeight),
                        CenterX = Node.X,
                        CenterY = Node.Y,
                        Top = Node.Y - CellHeight / 2,
                        Left = Node.X - CellWidth / 2,
                        Width = CellHeight,
                        Height = CellWidth,
                        Color = Color.FromArgb(255, 0, 0, 0)
                    };

        
                    GraphNodes.Items.Add(VisualGraphNode);
                    GraphNodeBounds.Items.Add(VisualGraphNode);
                    GraphNodeNames.Items.Add(VisualGraphNode);

                }
            }

            for (int Row = 0; Row < CellsY; ++Row)
            {
                for (int Col = 0; Col < CellsX; ++Col)
                {
                    AddAllNeighborsToGridNode(Graph, Row, Col, CellsX, CellsY);
                }
            }

            var StartNode = (NavGraphNode)Graph.GetNode(10);
            var EndNode = (NavGraphNode)Graph.GetNode(4);
            PathTest.Items.Add(new { StartPoint = new Point(StartNode.X, StartNode.Y), EndPoint = new Point(EndNode.X, EndNode.Y) });
            StartNode = (NavGraphNode)Graph.GetNode(1);
            EndNode = (NavGraphNode)Graph.GetNode(13);
            PathTest.Items.Add(new { StartPoint = new Point(StartNode.X, StartNode.Y), EndPoint = new Point(EndNode.X, EndNode.Y) });

            StartNode = (NavGraphNode)Graph.GetNode(9);
            EndNode = (NavGraphNode)Graph.GetNode(3);
            PathTest.Items.Add(new { StartPoint = new Point(StartNode.X, StartNode.Y), EndPoint = new Point(EndNode.X, EndNode.Y) });
            // This
            //for (int CurNodeIndex = 0; CurNodeIndex < Graph.NodeCount(); ++CurNodeIndex)
            //{
            //    var Node = (NavGraphNode)Graph.GetNode(CurNodeIndex);
            //    GraphItems.Items.Add(new VisualGraphNode
            //    {
            //        X = Node.X,
            //        Y = Node.Y,
            //        Width = CellHeight,
            //        Height = CellWidth,
            //        Color = Color.FromArgb(255, 0, 0, 0)
            //    });

            //}
        }

        public bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
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

        #endregion
    }

    #region Old, but still in use!  Replace!
    public enum EBrushType
    {
        Normal = 0,
        Obstacle = 1,
        Water = 2,
        Mud = 3,
        Source = 4,
        Target = 5
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

            return (double)Math.Sqrt(ySeparation * ySeparation + xSeparation * xSeparation);
        }
    }

    #endregion
}

