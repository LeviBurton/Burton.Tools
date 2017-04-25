using Burton.Lib;
using Burton.Lib.Characters;
using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;
using Burton.Lib.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Burton.Lib.Characters.Combat;

namespace SimpleDB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var Action in CombatActionManager.Instance.Actions)
            {
                Action.Execute(Action, null);
            }

            Console.Read();
        }     
    }
}
