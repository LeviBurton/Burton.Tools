using Burton.Lib.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class Cleric : Class
    {
        public Cleric() : base(8, EAbility.Constitution)
        {
            // HitDice: 0 -> NumDice, 1 -> NumSides
            // Cleric gets 1D8 Hit Dice.
            HitDice.Add(1); HitDice.Add(8);

            MaxSkillChoices = 2;

            SkillChoices.Add(SkillsDB.Instance.Get("History"));
            SkillChoices.Add(SkillsDB.Instance.Get("Insight"));
            SkillChoices.Add(SkillsDB.Instance.Get("Medicine"));
            SkillChoices.Add(SkillsDB.Instance.Get("Persuasion"));
            SkillChoices.Add(SkillsDB.Instance.Get("Religion"));

            SavingThrows.Add(EAbility.Wisdom);
            SavingThrows.Add(EAbility.Charisma);
        }
    }

    public class Class 
    {
        public string Name { get; set; }

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
         
        public Class(int StartingHitPoints, EAbility StartingHitPointsModifier)
        {
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
