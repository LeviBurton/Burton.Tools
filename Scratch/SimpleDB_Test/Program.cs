using Burton.Lib;
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
            //var Roller = new DiceRoller();

            //int Target = 8;
            //int Roll = Roller.Roll(1, 20);

            //if (Roll <= Target)
            //{
            //    Console.Write("win!");
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(Roller.Roll(1, 12));
            //}

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
