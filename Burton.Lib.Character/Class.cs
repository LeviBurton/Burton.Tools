using Burton.Lib.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EClassType
    {
        Bard,
        Paladin,
        Cleric,
        Fighter,
        Rogue,
        Wizard,
        Sorcerer,
        Druid,
        Ranger,
        Warlock
    }

    public class Wizard : Class
    {
        public Wizard() : base(EClassType.Wizard, 6, EAbility.Intelligence)
        {
            // HitDice: 0 -> NumDice, 1 -> NumSides
            // Cleric gets 1D8 Hit Dice.
            HitDice.Add(1); HitDice.Add(6);

            MaxSkillChoices = 2;
            SavingThrows.Add(EAbility.Intelligence);
        }
    }

    public class Cleric : Class
    {
        public Cleric() : base(EClassType.Cleric, 8, EAbility.Constitution)
        {
            // HitDice: 0 -> NumDice, 1 -> NumSides
            // Cleric gets 1D8 Hit Dice.
            HitDice.Add(1); HitDice.Add(8);

            MaxSkillChoices = 2;

            SkillChoices.Add(SkillManager.Instance.Find<Skill>(x => x.Name == "History").SingleOrDefault());
            SkillChoices.Add(SkillManager.Instance.Find<Skill>(x => x.Name == "Insight").SingleOrDefault());
            SkillChoices.Add(SkillManager.Instance.Find<Skill>(x => x.Name == "Medicine").SingleOrDefault());
            SkillChoices.Add(SkillManager.Instance.Find<Skill>(x => x.Name == "Persuasion").SingleOrDefault());
            SkillChoices.Add(SkillManager.Instance.Find<Skill>(x => x.Name == "Religion").SingleOrDefault());
 
            SavingThrows.Add(EAbility.Wisdom);
            SavingThrows.Add(EAbility.Charisma);
        }
    }

    public class Class 
    {
        public string Name;
        public EClassType ClassType;

        // HitDice: 0 -> NumDice, 1 -> NumSides
        public List<int> HitDice = new List<int>(2);

        public int StartingHitPoints;
        public EAbility StartingHitPointsModifier;
        public ClassProficiencyTable ClassProficiencyTable;

        // Selected Skills
        public List<Skill> SkillProficiencies = new List<Skill>();

        // Available Skills to pick from
        public List<Skill> SkillChoices = new List<Skill>();

        // Saving throw proficiencies
        public List<EAbility> SavingThrows = new List<EAbility>();

        // Max number of skills we can pick from Skill Choices)
        public int MaxSkillChoices;
         
        public Class(EClassType ClassType, int StartingHitPoints, EAbility StartingHitPointsModifier)
        {
            this.ClassType = ClassType;
            this.StartingHitPoints = StartingHitPoints;
            this.StartingHitPointsModifier = StartingHitPointsModifier;
        }
    }

    public class ClassProficiencyTable
    {
        public DataTable Table;

        public ClassProficiencyTable()
        {
            Table = new DataTable("ClassProficiencyTable");
            Table.Columns.Add(new DataColumn("ProficiencyBonus", typeof(int)));

        }
    }
}
