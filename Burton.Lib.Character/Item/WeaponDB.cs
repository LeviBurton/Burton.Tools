using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class WeaponManager
    {
        private static WeaponManager _Instance;
        public static WeaponManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new WeaponManager();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public WeaponManager()
        {
          //  Console.WriteLine("Weapon Manager, using ItemDB: " + ItemDB.Instance.ToString());
        }

        public void Init()
        {

        }
    }
}
