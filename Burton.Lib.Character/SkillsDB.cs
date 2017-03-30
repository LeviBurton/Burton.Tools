using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters.Skills
{
    // Serializes a list of available skills that can be used by PCs and NPCss
    public class SkillsDB : SimpleDB<Skill>
    {
        private static SkillsDB _Instance;

        public static SkillsDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SkillsDB();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public SkillsDB()
        {
            InitBaseSkills();
        }

        public void InitBaseSkills()
        {
            Add(new Skill("Athletics", EAbility.Strength));
            Add(new Skill("Acrobatics", EAbility.Dexterity));
            Add(new Skill("Sleight of Hand", EAbility.Dexterity));
            Add(new Skill("Stealth", EAbility.Dexterity));
            Add(new Skill("Arcana", EAbility.Intelligence));
            Add(new Skill("History", EAbility.Intelligence));
            Add(new Skill("Investigation", EAbility.Intelligence));
            Add(new Skill("Nature", EAbility.Intelligence));
            Add(new Skill("Religion", EAbility.Intelligence));
            Add(new Skill("Animal Handling", EAbility.Wisdom));
            Add(new Skill("Insight", EAbility.Wisdom));
            Add(new Skill("Medicine", EAbility.Wisdom));
            Add(new Skill("Perception", EAbility.Wisdom));
            Add(new Skill("Survival", EAbility.Wisdom));
            Add(new Skill("Deception", EAbility.Charisma));
            Add(new Skill("Intimidation", EAbility.Charisma));
            Add(new Skill("Performance", EAbility.Charisma));
            Add(new Skill("Persuasion", EAbility.Charisma));
        }
    }
}
