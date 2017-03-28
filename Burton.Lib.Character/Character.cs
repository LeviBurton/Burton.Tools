using System.Collections.Generic;

using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;

namespace Burton.Lib.Characters
{
    public class Character
    {
        public string Name { get; set; }

        // Basic Attributes
        public int Strength { get; set; }
        public int Health { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }

        public double BasicSpeed { get { return (Health + Dexterity) / 4;  } }
        public double Movement { get { return BasicSpeed; } }

        // Character skills
        public List<Skill> Skills;

        // Character quirks
        public List<Quirk> Quirks;
    }
}
