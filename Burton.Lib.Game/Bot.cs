using Burton.Lib.GameEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib
{
    public class Bot : BaseGameEntity
    {
        public Bot() : base(BaseGameEntity.GetNextValidID())
        {

        }
    }
}
