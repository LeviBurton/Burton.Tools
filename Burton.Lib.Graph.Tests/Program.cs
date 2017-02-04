using Burton.Lib.Graph;
using System;

namespace Burton.Lib.Graph.Tests
{
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

    class Program
    {

        public static bool ValidNeighbor(int x, int y, int NumCellsX, int NumCellsY)
        {
            return !((x <= 0) || (x >= NumCellsX) || (y <= 0) || (y >= NumCellsY));
        }

        public static void AddAllNeighborsToGridNode(SparseGraph<GraphNode, GraphEdge> Graph, int Row, int Col, int CellsX, int CellsY)
        {
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    int NodeX = Col + j;
                    int NodeY = Row + i;

                    if ((i == 0) && (j == 0))
                        continue;

                    if (ValidNeighbor(NodeX, NodeY, CellsX, CellsY))
                    {
                        var Node = (NavGraphNode)Graph.GetNode(Row * CellsX + Col);
                        var NeighborNode = (NavGraphNode)Graph.GetNode(NodeY * CellsX + NodeX);

                        var PosNode = new Vector2(Node.LocationX, Node.LocationY);
                        var PosNeighborNode = new Vector2(NeighborNode.LocationX, NeighborNode.LocationY);

                        double Distance = PosNode.Distance(PosNeighborNode);

                        GraphEdge NewEdge = new GraphEdge(Row * CellsX + Col, NodeY * CellsY + NodeX, Distance);

                        Graph.AddEdge(NewEdge);
                    }
                }
            }
        }

        public static void CreateGrid(SparseGraph<GraphNode, GraphEdge> Graph, int CellsX, int CellsY)
        {
            int GridWidthPx = 1000;
            int GridHeightPx = 1000;

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

            for (int Row = 1; Row < CellsY; ++Row)
            {
                for (int Col = 1; Col < CellsX; ++Col)
                {
                    AddAllNeighborsToGridNode(Graph, Row, Col, CellsX, CellsY);
                }
            }

        }

        static void Main(string[] args)
        {
            SparseGraph<GraphNode, GraphEdge> Graph = new SparseGraph<GraphNode, GraphEdge>(true);

            CreateGrid(Graph, 5, 5);

            Console.ReadKey();
        }
    }
}
