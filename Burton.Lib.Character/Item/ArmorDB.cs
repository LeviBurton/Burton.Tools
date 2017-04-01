using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    // NOT USED
    public class ArmorManager 
    {
        private static ArmorManager _Instance;

        public static ArmorManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArmorManager();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public ArmorManager()
        {
          //  InitBase();
        }
    }
}
