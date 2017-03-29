using System.Collections.Generic;

using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;

namespace Burton.Lib.Characters
{
    public class Character
    {
        public string Name { get; set; }

        // Ability Scores
        // ===============================
        // When creating a new character, roll 4d6, discard the lowest result,
        // then add the 3 together.
        // Repeat for all 6, then assign them to your abilities
        //
        //
        // Ability Checks
        // ===============================
        // Task Difficulty      DC
        // ------------------------
        // Very Easy            5
        // Easy                 10
        // Medium               15
        // Hard                 20
        // Very Hard            25
        // Nearly Impossible    30

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        // Weight in pounds they can carry.
        public int CarryingCapacity { get { return Strength * 15;  } }

        // Character skills
        public List<Skill> Skills;

        // Character quirks
        public List<Quirk> Quirks;
    }
}
