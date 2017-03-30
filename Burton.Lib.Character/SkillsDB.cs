using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters.Skills
{
    // Serializes a list of available skills that can be used by PCs and NPCss
    public class SkillsDB : SimpleDB<Skill>
    {
        public SkillsDB()
        {
        }

    }
}
