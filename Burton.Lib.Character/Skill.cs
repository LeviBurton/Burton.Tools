using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters.Skills
{
    [Serializable]
    public class Skill : DbItem
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Skill()
        {     
        }

        public Skill(string Name)
        {
            this.Name = Name;
        }
    }
}
