using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters.Skills
{
    [Serializable]
    public class Skill : DbItem
    {
        public string Description { get; set; }
        public EAbility AbilityModifier { get; set; }

        public Skill()
        {     
        }

        public Skill(string Name, EAbility AbilityModifier)
        {
            this.Name = Name;
            this.AbilityModifier = AbilityModifier;
        }
    }
}
