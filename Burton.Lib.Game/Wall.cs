using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib
{
    public class Wall
    {
        public Vector2 vA;
        public Vector2 vB;
        public Vector2 vN;

        public Wall()
        { }

        public Wall(Vector2 A, Vector2 B)
        {
            vA = A;
            vB = B;
        }

        public void CalculateNormals()
        {
            Vector2 Tmp = (vA - vB).Normalize();
            vN.X = -Tmp.Y;
            vN.Y = -Tmp.X;
        }
    }
}
