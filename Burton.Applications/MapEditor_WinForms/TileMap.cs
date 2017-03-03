using Burton.Lib.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizerTest
{
    public class TileMap
    {
        public SparseGraph<GraphNode, GraphEdge> Graph;

        // List of brush type ids that represent the tiles brush.
        public List<EBrushType> Terrain = new List<EBrushType>();
        public List<NavGraphNode> Path = new List<NavGraphNode>();
        public List<GraphEdge> SubTree = new List<GraphEdge>();

        TileImageManager TileImageManager;

        public List<TileBrushManager> TileBrushManagers = new List<GraphVisualizerTest.TileBrushManager>();

        // Testing pathing
        public int SourceNode;
        public int TargetNode;

        // calculated from TileWidth * NumTiles
        public int TileMapWidth = 0;
        public int TileMapHeight = 0;

        public int NumCellsX = 10;
        public int NumCellsY = 10;

        public int TileWidth = 64;
        public int TileHeight = 64;

        public int BigCircle = 12;
        public int MediumCircle = 5;
        public int SmallCircle = 2;

        public bool bIsPaintingTerrain;

        public bool bCanDraw = false;

        public TileMap(int Rows, int Columns)
        {
            NumCellsX = Columns;
            NumCellsY = Rows;
            bCanDraw = false;
        }

        public void Setup()
        {
            bCanDraw = false;

            Graph = new SparseGraph<GraphNode, GraphEdge>(false, NumCellsX * NumCellsY);

            CreateGrid();

            TileMapWidth = TileWidth * NumCellsX;
            TileMapHeight = TileHeight * NumCellsY;

            bCanDraw = true;
        }

        public void ClearTerrain()
        {
            for (int i = 0; i < NumCellsX * NumCellsY; i++)
            {
                Terrain.Insert(i, EBrushType.Normal);
            }
        }

        public void CreateGrid()
        {
            float MidX = TileWidth / 2;
            float MidY = TileHeight / 2;

            for (int Row = 0; Row < NumCellsY; ++Row)
            {
                for (int Col = 0; Col < NumCellsX; ++Col)
                {
                    var NodeIndex = Graph.AddNode(new NavGraphNode(Graph.GetNextFreeNodeIndex(),
                                                                   MidX + (Col * TileWidth), MidY + (Row * TileWidth)));
                }
            }

            for (int Row = 0; Row < NumCellsY; ++Row)
            {
                for (int Col = 0; Col < NumCellsX; ++Col)
                {
                    AddAllNeighborsToGridNode(Row, Col, NumCellsX, NumCellsY);
                }
            }
        }

        public static bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
        {
            return !((x < 0) || (x >= NumCellsX) || (y < 0) || (y >= NumCellsY));
        }

        public void AddAllNeighborsToGridNode(int Row, int Col, int CellsX, int CellsY)
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

        public void WeightNavGraphNodeEdges(int NodeIndex, double Weight)
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
                    int y = (TileIndex / NumCellsX);
                    int x = TileIndex - (y * NumCellsX);
                    float MidX = TileWidth / 2;
                    float MidY = TileHeight / 2;

                    Vector2 Position = new Vector2(MidX + (x * TileWidth), MidY + (y * TileHeight));
                    var NodeIndex = Graph.AddNode(new NavGraphNode(TileIndex, Position.x, Position.y));

                    AddAllNeighborsToGridNode(y, x, NumCellsX, NumCellsY);
                }

                double TerrainCost = 1.0;

                if (Brush == EBrushType.Water)
                {
                    TerrainCost = 2.0;
                }
                else if (Brush == EBrushType.Normal)
                {
                    TerrainCost = 1.0;
                }
                else if (Brush == EBrushType.Mud)
                {
                    TerrainCost = 1.5;
                }

                WeightNavGraphNodeEdges(TileIndex, TerrainCost);

            }

            Terrain[TileIndex] = Brush;
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            if (!bCanDraw)
                return;

            // Draw Grid, Terrain, Nodes, Edges and Labels
            for (int CurNodeIndex = 0; CurNodeIndex < Graph.NodeCount(); ++CurNodeIndex)
            {
                var Node = (NavGraphNode)Graph.GetNode(CurNodeIndex);

                if (Node.NodeIndex == (int)ENodeType.InvalidNodeIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
                    continue;
                }

                if (Terrain[Node.NodeIndex] == EBrushType.Normal)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Obstacle)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Water)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
                }
                if (Terrain[Node.NodeIndex] == EBrushType.Mud)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
                }

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;

                // e.Graphics.DrawString(string.Format("{0}", Node.NodeIndex), Font, Brushes.Black, new PointF((float)Node.X - 2.0f, (float)Node.Y - 10.0f));
                e.Graphics.FillEllipse(new SolidBrush(Color.Black), new RectangleF((float)Node.X - SmallCircle, (float)Node.Y - SmallCircle, SmallCircle * 2, SmallCircle * 2));

                foreach (var Edge in Graph.Edges[Node.NodeIndex])
                {
                    var FromNode = Graph.GetNode(Edge.FromNodeIndex) as NavGraphNode;
                    var ToNode = Graph.GetNode(Edge.ToNodeIndex) as NavGraphNode;
                    e.Graphics.DrawLine(new Pen(Color.LightGray), new PointF((float)FromNode.X, (float)FromNode.Y), new PointF((float)ToNode.X, (float)ToNode.Y));
                }

                if (Node.NodeIndex == SourceNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Green), new RectangleF((float)Node.X - BigCircle, (float)Node.Y - BigCircle, BigCircle * 2, BigCircle * 2));
                }
                else if (Node.NodeIndex == TargetNode)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), new RectangleF((float)Node.X - BigCircle, (float)Node.Y - BigCircle, BigCircle * 2, BigCircle * 2));
                }

                e.Graphics.DrawRectangle(new Pen(Color.DarkGray), new Rectangle(new Point((int)Node.X - (TileWidth / 2), (int)Node.Y - (TileHeight / 2)), new Size(TileWidth, TileHeight)));
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
                    e.Graphics.DrawLine(new Pen(Color.Blue, 3), new PointF((float)Path[i].X, (float)Path[i].Y),
                                                                new PointF((float)Path[i + 1].X, (float)Path[i + 1].Y));
                }
            }
        }

        public void Load(string FileName)
        {
            using (Stream InStream = File.Open(FileName, FileMode.Open))
            {
                var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                NumCellsX = (int)BinaryFormatter.Deserialize(InStream);
                NumCellsY = (int)BinaryFormatter.Deserialize(InStream);

                Terrain = null;
                Graph = null;

                Setup();
                Terrain = (List<EBrushType>)BinaryFormatter.Deserialize(InStream);

                for (int i = 0; i < NumCellsX * NumCellsY; i++)
                {
                    UpdateGraphFromBrush(i, Terrain[i]);
                }
            }
        }

        public void Save(string FileName)
        {

        }
    }
}
