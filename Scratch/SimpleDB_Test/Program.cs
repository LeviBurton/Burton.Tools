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
            SpellManager.Instance.Import("Spells.tsv.txt");
            SpellManager.Instance.SaveChanges();

            ItemManager.Instance.ImportSpellComponents("SpellComponents.tsv");
            ItemManager.Instance.SaveChanges();

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

        static void TestSpellMethods()
        {
            var assembly = typeof(SpellMethods).Assembly;


            Dictionary<string, MethodInfo> methods = assembly
                        .GetTypes()
                        .SelectMany(x => x.GetMethods())
                        .Where(y => y.GetCustomAttributes(true).OfType<SpellMethodAttribute>().Any())
                        .ToDictionary(z => z.Name);

            var Class = new Class(8, EAbility.Intelligence);
            var c1 = new Character(Class);
            var c2 = new Character(Class);
            c1.Name = "Sally";
            c2.Name = "Jimmy";

            c1.RollAbilities();
            c2.RollAbilities();

        
            SpellManager.Instance.Load();

            var Bless = SpellManager.Instance.Find<Spell>(x => x.Name == "Bless").SingleOrDefault();
            var Burning_Hands = SpellManager.Instance.Find<Spell>(x => x.Name == "Burning Hands").SingleOrDefault();
            var Fireball = SpellManager.Instance.Find<Spell>(x => x.Name == "Fireball").SingleOrDefault();

            Bless.SetSpellMethod<SpellMethods>("Bless");
            Burning_Hands.SetSpellMethod<SpellMethods>("Burning_Hands");
            Fireball.SetSpellMethod<SpellMethods>("Fireball");

            Bless.Cast(c1);
            Bless.Cast(c2);

            Burning_Hands.Cast(c1);
            Fireball.Cast(c2);
        }

        static void RebuilDBs()
        {
            SpellManager.Instance.Bootstrap();
            SpellManager.Instance.Import("Spells.tsv.txt");
            SpellManager.Instance.SaveChanges();


            ItemManager.Instance.Bootstrap();
            ItemManager.Instance.ImportSpellComponents("SpellComponents.tsv");
            ItemManager.Instance.SaveChanges();

            SkillManager.Instance.Bootstrap();
            SkillManager.Instance.SaveChanges();
        }

        static void Main(string[] args)
        {
            //RunTimingTests();
            //Test1();
            TestSpellMethods();

            RebuilDBs();
        //    Console.ReadKey();
        }     
    }
}
