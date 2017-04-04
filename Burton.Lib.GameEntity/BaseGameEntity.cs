using Burton.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.GameEntity
{
    //public static enum { DefaultEntityType = -1 }

    public class BaseGameEntity
    {
        public static int NextValidID;
        public int ID;
        public int Type;
        bool bTag;
        public double BoundingRadius;
        public Vector2 Position;
        public Vector2 Scale;

        public BaseGameEntity(int ID)
        {
            SetID(ID);
        }

        public void Update()
        {

        }

        public void SetID(int Val)
        {
            if (Val >= NextValidID)
            {
                throw new ArgumentException(string.Format("Val:{0} > NextValidID: {1}", Val, NextValidID));
            }

            ID = Val;
            NextValidID = ID + 1;
        }

        public static int GetNextValidID()
        {
            return NextValidID;
        }

        public static void ResetNextValidID()
        {
            NextValidID = 0;
        }

        public virtual void Read() { }
        public virtual void  Write() { }
    }
}
