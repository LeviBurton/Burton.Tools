using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Graph
{
    public class CellSpacePartition<TEntity> 
    {
        // The required amount of cells in the space
        List<Cell<TEntity>> Cells = null;

        // This is used to store any valid neighbors when an agent 
        // searches its neighboring space
        List<TEntity> Neighbors = null;

        // the width and height of the world space the entities inhabit
        double SpaceWidth;
        double SpaceHeight;

        // the number of cells the space is going to be divided up into
        int NumCellsX;
        int NumCellsY;

        double CellSizeX;
        double CellSizeY;

        public int PositionToIndex(Vector2 Pos)
        {
            int Index = (int)(NumCellsX * Pos.X / SpaceWidth) +
                        ((int)((NumCellsY) * Pos.Y / SpaceHeight) * NumCellsX);

            Index = System.Math.Min(Index, Cells.Count - 1);

            //if (Index > Cells.Count - 1)
            //    Index = Cells.Count - 1;

            return Index;
        }

        public CellSpacePartition(double Width, double Height, int CellsX, int CellsY, int MaxEntities)
        {
            this.SpaceWidth = Width;
            this.SpaceHeight = Height;
            this.NumCellsX = CellsX;
            this.NumCellsY = CellsY;
            this.Neighbors = new List<TEntity>(MaxEntities);

            CellSizeX = Width / CellsX;
            CellSizeY = Height / CellsY;

            Cells = new List<Cell<TEntity>>();

            for (int y = 0; y < NumCellsY; ++y)
            {
                for (int x = 0; x < NumCellsX; ++x)
                {
                    double left = x * CellSizeX;
                    double right = left + CellSizeX;
                    double top = y * CellSizeY;
                    double bot = top + CellSizeY;

                    Cells.Add(new Cell<TEntity>(new Vector2(left, top), new Vector2(right, bot)));
                }
            }
        }

        //adds entities to the class by allocating them to the appropriate cell
        public void AddEntity(TEntity Entity)
        {
            if (Entity == null)
                throw new ArgumentException("Entity can't be null!");

            int Size = Cells.Count;
      
        }

        //update an entity's cell by calling this from your entity's Update method 
        public void UpdateEntity(TEntity Entity, Vector2 OldPos)
        {

        }

        //this method calculates all a target's neighbors and stores them in
        //the neighbor vector. After you have called this method use the begin, 
        //next and end methods to iterate through the vector.
        public void CalculateNeighbors(Vector2 TargetPos, double QueryRadius)
        {

        }

        //empties the cells of entities
        public void EmptyCells()
        {

        }
    }
}
