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
            var Iterations = 1000;

            // Fire up the db.
            ItemManager.Instance.Refresh();

            Console.WriteLine("Running Timing test...");

            var Stopwatch = new Stopwatch();
            Stopwatch.Start();

            for (int i = 0; i < Iterations; i++)
            {
                //var Test = ItemManager.Instance.Find<Spell>(x => x.MagicSchool == EMagicSchoolType.Abjuration).ToList();
                // var Test = ItemManager.Instance.Find<Spell>(x => x.Name.Contains("B")).ToList();
                //   var Test = ItemManager.Instance.Find<Spell>().ToList();
                var SingleItem = ItemManager.Instance.Find<Weapon>(x => x.Name.Contains("sword")).ToList();
            }

            Stopwatch.Stop();

            Console.WriteLine(string.Format("{0} {1} {2}", Iterations, Stopwatch.Elapsed.TotalSeconds, Stopwatch.Elapsed.Ticks/Stopwatch.Frequency));
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
            //AbilityModifierTable.InitTable();
            DifficultyClassesTable.InitTable();

            // All dice rolls go through the single roller instance.
            // We can pass a seed to guaruntee the same results on every roll.
            // DiceRoller.Instance.SetSeed(100);

            var Char = new Character(new Cleric());

            // SpellManager.Import("spells.tsv.txt");

            var Skills = SkillManager.Instance.Find<Skill>().ToList();

            //ItemManager.Instance.ImportSpellComponents("SpellComponents.tsv");
            //ItemManager.Instance.SaveChanges();

            Console.WriteLine("Spells");
       
            Func<Spell, bool> Where = null;
            Where = x => x.MagicSchool == EMagicSchoolType.Divination && x.Level > 1 && x.SpellRange.RangeType == ESpellRangeType.Touch;
            var Spells = SpellManager.Instance.Find<Spell>(Where).ToList();
            var AllSpells = SpellManager.Instance.Find<Spell>().ToList();

            foreach (var Spell in Spells)
            {
                Console.WriteLine(string.Format("{0,-30} {1,-6} {2,-20} {3,-10}", Spell.Name, Spell.Level, Spell.MagicSchool.ToString(), Spell.SpellRange.RangeType.ToString()));
            }
            Console.WriteLine();

            Console.WriteLine("Skills");
            foreach (var Ability in Enum.GetNames(typeof(EAbility)))
            {
                Console.WriteLine(Ability);
                foreach (var Skill in SkillManager.Instance.Find<Skill>(x => x.Ability == (EAbility)Enum.Parse(typeof(EAbility), Ability)))
                {
                    Console.WriteLine("-- {0}", Skill.Name);
                }
                Console.WriteLine();
            }

            Console.WriteLine();

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
                    Char.RollAbilities();
                    int avg = 0;

                    foreach (var CharAbility in Char.Abilities)
                    {
                        avg += CharAbility.CurrentValue;

                        Console.WriteLine("{0}: {1} ({2}) ", CharAbility.ShortName, CharAbility.CurrentValue, CharAbility.GetModifier());
                    }

                    avg /= 6;
                    Console.WriteLine("AVG: {0}", avg);

                    Console.WriteLine();
                }
            } while (Key != ConsoleKey.Escape);
        }

        static void Main(string[] args)
        {
            //RunTimingTests();
            Test1();
        }     
    }
}
