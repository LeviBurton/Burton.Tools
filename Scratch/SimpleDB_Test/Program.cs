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

            ConsoleKey Key;

            // Lots of testing stuff.
            var Armor = (ItemDB.Instance.Get("Chain Mail") as Armor);

            var Armors = ItemDB.Instance.Items.Where(item => item.Type == EItemType.Armor).ToList();
            var Weapons = ItemDB.Instance.Items.Where(item => item.Type == EItemType.Weapon).ToList();
            var Test = ItemDB.Instance.Items.Where(item => item.GetType() == typeof(Armor)).ToList();
            var RangePropertyWeapons = Weapons.Where(weapon => (weapon as Weapon).WeaponProperties.Contains(EWeaponProperty.Range)).ToList();
            var RangedWeapon = Weapons.Where(weapon => (weapon as Weapon).SubType == EItemSubType.Martial_Ranged ||
                                                              (weapon as Weapon).SubType == EItemSubType.Simple_Ranged).ToList();

            var Bow = (ItemDB.Instance.Items.Where(item => item.Name == "Longbow").Single() as Weapon);

            var RareWeapons = ItemDB.Instance.Items.Where(item => item.Rarity == EItemRarity.Rare && 
                                                                       item.Type == EItemType.Weapon).ToList();

            do
            {
                Key = Console.ReadKey(true).Key;

                if (Key == ConsoleKey.R)
                {
                    Char.RollAbilities();

                    Char.Equipment.Add(Bow);
                    Char.Equipment.Add(Armor);

                    for (int i = 0; i < 10; i++)
                    {
                        var Attack = DiceRoller.Instance.Roll(Bow.Damage).Sum();
                        Console.WriteLine(string.Format("{0}, {1}D{2}, damage: {3}", Bow.Name, Bow.Damage[0], Bow.Damage[1], Attack));
                    }

                    if (Armor != null && Char.CanEquip(Armor))
                    {
                        Char.EquipArmor(Armor);
                    }
                 
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
