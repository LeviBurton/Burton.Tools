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
            SpellManager.Instance.Refresh();

            Console.WriteLine("Running Timing test...");

            var Stopwatch = new Stopwatch();
            Stopwatch.Start();

            for (int i = 0; i < Iterations; i++)
            {
                //var Test = SpellManager.Instance.Find<Spell>(x => x.MagicSchool == EMagicSchoolType.Abjuration).ToList();
                // var Test = SpellManager.Instance.Find<Spell>(x => x.Name.Contains("B")).ToList();
                //   var Test = SpellManager.Instance.Find<Spell>().ToList();
                var SingleItem = SpellManager.Instance.Find<Spell>(x => x.Name.Contains("Fire")).ToList();
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

            var Skills = SkillManager.Instance.GetItemsCopy();


            Console.WriteLine("Spells");
            var Spells = SpellManager.Instance.GetItemsCopy().AsQueryable();

            Func<Spell, bool> Where = null;
            Where = x => x.MagicSchool == EMagicSchoolType.Divination && x.Level == 1;
            var Test = SpellManager.Instance.Find<Spell>(Where).ToList();
            var AllSpells = SpellManager.Instance.Find<Spell>().ToList();

            Spells = Spells.Where(spell => spell.MagicSchool == EMagicSchoolType.Divination);
            Spells = Spells.Where(spell => spell.Classes.Contains(EClassType.Cleric));
            Spells = Spells.Where(spell => spell.Classes.Contains(EClassType.Paladin));
            Spells = Spells.Where(spell => spell.SpellRange.RangeType == ESpellRangeType.Self);
            Spells = Spells.OrderBy(spell => spell.Level);

            var FilteredSpells = Spells.ToList();


            foreach (var Spell in Spells)
            {
                Console.WriteLine(string.Format("{0,-30} {1,-6} {2,-20} {3,-10}", Spell.Name, Spell.Level, Spell.MagicSchool.ToString(), Spell.SpellRange.RangeType.ToString()));
            }
            Console.WriteLine();

            Console.WriteLine("Skills");
            foreach (var Ability in Enum.GetNames(typeof(EAbility)))
            {
                Console.WriteLine(Ability);
                foreach (var Skill in SkillManager.Instance.GetItemsCopy().Where(x => x.Ability == (EAbility)Enum.Parse(typeof(EAbility), Ability)))
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
                    var Weapon = ItemManager.Instance.GetItemCopy<Weapon>(11);
                    Weapon.Description = "This should get saved to disk";
                    ItemManager.Instance.UpdateItem<Weapon>(Weapon);
                    ItemManager.Instance.SaveChanges();
                }
                if (Key == ConsoleKey.R)
                {
                    Char.RollAbilities();

                    int avg = 0;

                    var AllItems = ItemManager.Instance.GetItemsCopy();
                    Weapon LongBow = ItemManager.Instance.GetItemCopy<Weapon>(11);
                    Armor LeatherArmor = ItemManager.Instance.GetItemCopy<Armor>(1);

                    foreach (var Item in AllItems)
                    {
                        Console.WriteLine("{0} {1}", Item.ID, Item.Description);
                    }

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
            RunTimingTests();
        }     
    }
}
