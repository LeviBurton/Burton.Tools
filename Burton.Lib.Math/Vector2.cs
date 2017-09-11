using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Math
{
    [Serializable]
    public class Vector2 
    {
        public double X;
        public double Y;

        public Vector2(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            var Result = new Vector2(lhs.X, lhs.Y);

            Result.X -= rhs.X;
            Result.Y -= rhs.Y;

            return Result;
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            var Result = new Vector2(lhs.X, lhs.Y);

            Result.X += rhs.X;
            Result.Y += rhs.Y;

            return Result;
        }

        public double Length()
        {
            return System.Math.Sqrt(X * X + Y * Y);
        }

        public double LengthSq()
        {
            return (X * X + Y * Y);
        }

        public Vector2 Normalize()
        {
            Vector2 V = new Vector2(X, Y);
            double Length = V.Length();

            if (Length > 1.0)
            {
                V.X /= Length;
                V.Y /= Length;
            }

            return V;
        }

        public static double Distance(Vector2 v1, Vector2 v2)
        {
            double ydist = v2.Y - v1.Y;
            double xdist = v2.X - v1.X;

            return System.Math.Sqrt(ydist * ydist + xdist * xdist);
        }

        public double Distance(Vector2 v2)
        {
            double ydist = v2.Y - this.Y;
            double xdist = v2.X - this.X;

            return System.Math.Sqrt(ydist * ydist + xdist * xdist);
        }
    }
}
