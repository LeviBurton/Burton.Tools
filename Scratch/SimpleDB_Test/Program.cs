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

namespace SimpleDB_Test
{
    class Program
    {
        static void RunTest()
        {
            var Iterations = 1000000/20;

            // Fire up the db.
            ItemManager.Instance.Refresh();

            var Stopwatch = new Stopwatch();
            Stopwatch.Start();

            List<Spell> Items = null;

            for (int i = 0; i < Iterations; i++)
            {
                Items = SpellManager.Instance.Find<Spell>().ToList();
                // var Test = ItemManager.Instance.Find<Spell>(x => x.Name.Contains("B")).ToList();
                //   var Test = ItemManager.Instance.Find<Spell>().ToList();
                //var SingleItem = SpellManager.Instance.Find<Spell>(x => x.Name.Contains("Bless")).SingleOrDefault();
            }

            Stopwatch.Stop();

            Console.WriteLine(string.Format("{0,-9} {1,-12} {2,-4}", Iterations, Stopwatch.Elapsed.TotalSeconds, Items.Count ));
        }

        static void RunTimingTests()
        {
            ConsoleKey Key;

            do
            {
                Key = Console.ReadKey(true).Key;
                if (Key == ConsoleKey.R)
                {
                    RunTest();
                }
            } while (Key != ConsoleKey.Escape);
        }

        static void Test1()
        {
            //SpellManager.Instance.Import("Spells.tsv.txt");
            //SpellManager.Instance.SaveChanges();

            //ItemManager.Instance.ImportSpellComponents("SpellComponents.tsv");
            //ItemManager.Instance.SaveChanges();

            var ChainLightning = SpellManager.Instance.Find<Spell>(x => x.Name == "Chain Lightning").SingleOrDefault();

            ConsoleKey Key;

            do
            {
                Key = Console.ReadKey(true).Key;

                if (Key == ConsoleKey.T)
                {
                    var Weapon = ItemManager.Instance.Find<Weapon>(x => x.ID == 11).SingleOrDefault();
                    Weapon.Description = "This should get saved to disk";
                    ItemManager.Instance.UpdateItem<Weapon>(Weapon);
                    ItemManager.Instance.SaveChanges();
                }
                if (Key == ConsoleKey.R)
                {

                }
            } while (Key != ConsoleKey.Escape);
        }

        static void Main(string[] args)
        {
            RunTimingTests();
            //Test1();
        }     
    }
}
