using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Dice
{
    public class DiceRoller
    {
        private Random Random;
        public int Seed;

        public DiceRoller()
        {
            Seed = Environment.TickCount;
            this.Random = new Random(Seed);
        }

        public DiceRoller(int Seed)
        {
            this.Seed = Seed;
            this.Random = new Random(this.Seed);
        }

        public int Roll(int NumDice, int NumSides)
        {
            int Sum = 0;

            for (int Roll = 0; Roll < NumDice; Roll++)
            {
                // Max is exclusive upper bound, so we need to add 1 to it.
                // Min is inclusive lower bound.
                Sum += Random.Next(1, NumSides + 1);
            }

            return Sum;
        }
    }
}
