using System.Drawing;
using Burton.Lib.Alg;

namespace WindowsFormsApplication1
{
    public enum TileType
    {
        Walkable,
        Unwalkable,
        Path
    }

    public enum SearchList
    {
        Open,
        Closed,
        None
    }

    public class Tile : PathNode
    {
        public Rectangle OuterRect;
        public Rectangle InnerRect;
        public int PenWidth;
        public Color Color;
        public Pen Pen;
        public Brush FillBrush;
        public string TileName;
        public bool IsSelected;
        public TileType TileType;
        public bool bIsStartTile;
        public bool bIsGoalTile;
        public Font Font;
        public Pen PathPen;

        public int TileIndex;
        public Brush WalkableFillBrush;
        public Brush UnwalkableFillBrush;
        public Pen WalkablePen;
        public Pen OpenListPen;
        public Pen UnwalkablePen;
        public Pen ClosedListPen;
        public bool bUseAdjacencyPen;
        public int AdacentPenWidth;

        public SearchList SearchList;

        public Tile()
        {
            Reset();
        }

        public void OnPaint(Graphics Graphics)
        {
            if (TileType == TileType.Unwalkable)
            {
                FillBrush = UnwalkableFillBrush;
                Pen = UnwalkablePen;
            }
            else if (TileType == TileType.Walkable)
            {
                FillBrush = WalkableFillBrush;
                Pen = WalkablePen;
            }
            else if (TileType == TileType.Path)
            {
                FillBrush = new SolidBrush(Color.LightBlue);
               
            }

            if (bIsStartTile)
            {
                FillBrush = new SolidBrush(Color.Green);
            }

            if (bIsGoalTile)
            {
                FillBrush = new SolidBrush(Color.Red);
            }

            if (SearchList == SearchList.Open)
            { 
            //    Pen = OpenListPen;
            }
            else if (SearchList == SearchList.Closed)
            {
                Pen = ClosedListPen;
            }
            else
            {
                Pen = WalkablePen;
            }


            Graphics.FillRectangle(FillBrush, OuterRect);
            Graphics.DrawRectangle(Pen, InnerRect);
            Graphics.FillEllipse(new SolidBrush(Color.Black), new RectangleF(OuterRect.Location.X  + OuterRect.Width / 2, OuterRect.Location.Y + OuterRect.Height / 2, 
                2, 2));

            //if (F > 0)
            //{
            //    StringFormat sf = new StringFormat();
            //    sf.LineAlignment = StringAlignment.Near;
            //    sf.Alignment = StringAlignment.Near;
            //    Graphics.DrawString(string.Format("H{0:0.0}", H), Font, Brushes.Black, InnerRect, sf);

            //    sf.LineAlignment = StringAlignment.Far;
            //    sf.Alignment = StringAlignment.Near;
            //    Graphics.DrawString(string.Format("G{0:0.0}", G), Font, Brushes.Black, InnerRect, sf);

            //    sf.LineAlignment = StringAlignment.Center;
            //    sf.Alignment = StringAlignment.Center;
            //    Graphics.DrawString(string.Format("F{0:0.0}", F), Font, Brushes.Black, InnerRect, sf);
            //}
        }

        public bool OnMiddleMouseClick()
        {
            bool bNeedsRefresh = true;
            IsWalkable = !IsWalkable;

            if (TileType == TileType.Unwalkable)
                TileType = TileType.Walkable;
            else
                TileType = TileType.Unwalkable;

            return bNeedsRefresh;
        }

        public bool OnLeftMouseClick()
        {
            bool bNeedsRefresh = true;
            TileType = TileType.Walkable;
            bIsStartTile = !bIsStartTile;
            return bNeedsRefresh;
        }

        public bool OnRightMouseClick()
        {
            bool bNeedsRefresh = true;
            
            bIsGoalTile = !bIsGoalTile;
            return bNeedsRefresh;    
        }

        public void OnSelected()
        {
            TileType = TileType.Unwalkable;
            IsSelected = true;
        }

        public void OnDeselected()
        {
            TileType = TileType.Walkable;
            IsSelected = false;
        }

        public void Reset()
        {
            PenWidth = 1;
            AdacentPenWidth = 5;
            FillBrush = new SolidBrush(Color.White);
            WalkableFillBrush = new SolidBrush(Color.White);
            UnwalkableFillBrush = new SolidBrush(Color.DarkGray);
            WalkablePen = new Pen(Color.Black, PenWidth);
            OpenListPen = new Pen(Color.Yellow, AdacentPenWidth);
            ClosedListPen = new Pen(Color.Blue);
            UnwalkablePen = new Pen(Color.Red, PenWidth);
            PathPen = new Pen(Color.Blue, PenWidth);

            Color = Color.Black;
            Pen = WalkablePen;
            SearchList = SearchList.None;

            this.TileType = TileType.Walkable;
            Font = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
            OpenListPen = new Pen(Color.Yellow);
            G = 0; H = 0; 
            State = NodeState.Untested;
            IsWalkable = true;
        }
    }

    public class WallTile : Tile
    {
        public WallTile()
        {
            FillBrush = new SolidBrush(Color.Blue);
        }
    }

}
