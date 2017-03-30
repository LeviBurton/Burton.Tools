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
            //// TestOne();
            var Roller = new DiceRoller();

            AbilityModifierTable.InitTable();
            DifficultyClassesTable.InitTable();

            var Char = new Character();
            var Str = Char.GetAbility(EAbility.Strength);

            foreach (var CharAbility in Char.Abilities)
            {
                // Roll 4D6 and remove the lowest die.
                var Roll = Roller.Roll(4, 6);
                Roll.Sort();
                Roll.RemoveAt(0);
                CharAbility.CurrentValue = Roll.Sum();
                Console.WriteLine("{0}: {1} ({2}) ", CharAbility.ShortName, CharAbility.CurrentValue, CharAbility.GetModifier());
            }

            var Check = Roller.Roll(1, 20);
            var Ability = Char.GetAbility(EAbility.Strength);
            var Modifier = Ability.GetModifier();
            var Difficulty = DifficultyClassesTable.GetDifficultyByClass(EDifficultyClass.Easy);

            var TheFinalResult = Check.Sum() + Modifier;


            Console.Read();
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
            for (int i = 0; i < 10; i++)
            {
                var Skill = new Skill(string.Format("Skill {0}", i));
                Skills.Add(Skill);
            }

            Skills.Save("Skills.sdb");
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
