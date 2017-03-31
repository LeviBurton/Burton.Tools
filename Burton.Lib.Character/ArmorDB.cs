using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class ArmorDB : SimpleDB<Armor>
    {
        private static ArmorDB _Instance;

        public static ArmorDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ArmorDB();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public ArmorDB()
        {
            InitBase();
        }

        public void InitBase()
        {
            Items.Clear();

        }
    }
}
