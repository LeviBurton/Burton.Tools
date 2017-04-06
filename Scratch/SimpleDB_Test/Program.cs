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

        static void Main(string[] args)
        {
            //AbilityModifierTable.InitTable();
            DifficultyClassesTable.InitTable();

            // All dice rolls go through the single roller instance.
            // We can pass a seed to guaruntee the same results on every roll.
            // DiceRoller.Instance.SetSeed(100);

            var Char = new Character(new Cleric());
            var ItemManager = new ItemManager();
            var SkillManager = new SkillManager();
            var SpellManager = new SpellManager();

           // SpellManager.Import("spells.tsv.txt");

            var Skills = SkillManager.GetItemsCopy();

            Console.WriteLine("Spells");
            var Spells = SpellManager.GetItemsCopy().AsQueryable();     
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
                foreach (var Skill in Skills.Where(x => x.Ability == (EAbility)Enum.Parse(typeof(EAbility), Ability)))
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
    }
}
