using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Dice
{
    public class DiceRoller
    {
        public Random Random;
        public int Seed;
        
        private static DiceRoller _Instance = null;
        
        public static DiceRoller Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DiceRoller();
                }

                return _Instance;
            }
        }

        public DiceRoller()
        {
            this.Seed = Environment.TickCount;
            this.Random = new Random(Seed);
        }

        public DiceRoller(int Seed)
        {
            this.Seed = Seed;
            this.Random = new Random(this.Seed);
        }

        public void SetSeed(int Seed)
        {
            this.Seed = Seed;
            this.Random = new Random(Seed);
        }


        // Returns an array containing the results of each dice roll.
        public List<int> Roll(int NumDice, int NumSides)
        {
            List<int> Result = new List<int>(NumDice);

            for (int Roll = 0; Roll < NumDice; Roll++)
            {
                Result.Add(DiceRoller.Instance.Random.Next(1, NumSides + 1));
            }

            return Result;
        }

        public List<int> Roll(int[] Dice)
        {
            List<int> Result = new List<int>(Dice[0]);

            for (int Roll = 0; Roll < Dice[0]; Roll++)
            {
                Result.Add(DiceRoller.Instance.Random.Next(1, Dice[1] + 1));
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
