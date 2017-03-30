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

        // Returns an array containing the results of each dice roll.
        public List<int> Roll(int NumDice, int NumSides)
        {
            List<int> Result = new List<int>(NumDice);

            for (int Roll = 0; Roll < NumDice; Roll++)
            {
                Result.Add(Random.Next(1, NumSides + 1));
            }

            return Result;
        }

        // Returns the sum of the dice rolls.
        public int RollSum(int NumDice, int NumSides)
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
