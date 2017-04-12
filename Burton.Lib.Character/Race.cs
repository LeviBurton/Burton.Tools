using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class Race : DbItem 
    {
        public int Age;
        public EAlignmentAttitudeType RacialAlignmentAttitudeType;
        public EAlignmentMoralityType RacialAlignmentMoralityType;
        public ECreatureSizeType SizeType;
        public float Speed;
        public string Description;

        public Race()
        {
           
        }
    }
}
