using Burton.Lib;
using Burton.Lib.Characters;
using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;
using Burton.Lib.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDB_Test
{
    class Program
    {
        public static SkillsDB Skills = new SkillsDB();
        public static QuirksDB Quirks = new QuirksDB();

        static void TestOne()
        {
            SaveQuirksTest();

            LoadSkills();
            LoadQuirks();

            foreach (var Skill in Skills.Items)
            {
                if (Skill == null)
                    continue;

                Console.WriteLine(Skill.Name);
            }
        }

        static void Main(string[] args)
        {
            //AbilityModifierTable.InitTable();
            DifficultyClassesTable.InitTable();

            // All dice rolls go through the single roller instance.
            // We can pass a seed to guaruntee the same results on every roll.
            // DiceRoller.Instance.SetSeed(100);

            var Char = new Character(new Cleric());
            var ItemManager = new ItemManager();

            ConsoleKey Key;

            do
            {
                Key = Console.ReadKey(true).Key;

                if (Key == ConsoleKey.T)
                {
                    var Weapon = ItemManager.GetItemCopy<Weapon>(11);
                    Weapon.Description = "This should get saved to disk";
                    ItemManager.UpdateItem<Weapon>(Weapon);
                    ItemManager.SaveChanges();
                }
                if (Key == ConsoleKey.R)
                {
                    Char.RollAbilities();
                   
                    int avg = 0;

                    var AllItems = ItemManager.GetItemsCopy();
                    Weapon LongBow = ItemManager.GetItemCopy<Weapon>(11);
                    Armor LeatherArmor = ItemManager.GetItemCopy<Armor>(1);

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

        static void LoadQuirks()
        {
            Quirks = new QuirksDB();
            Quirks.Load("Quirks.sdb");
        }

        static void LoadSkills()
        {
            Skills = new SkillsDB();
            Skills.Load("Skills.sdb");
        }

        static void SaveSkillsTest()
        {
          
        }

        static void SaveQuirksTest()
        {
            for (int i = 0; i < 5; i++)
            {
                var Foo = new Quirk();
                Quirks.Add(Foo);
            }

            Quirks.Save("Quirks.sdb");
        }
    }
}
