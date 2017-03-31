using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    [Serializable]
    public class Armor : Equipment
    {
        public int ArmorClass;
   
        public Armor(EEquipmentSubType SubType, EEquipmentRarity Rarity, EAbility ModifierType, int ArmorClass, string Name, string Description, int Cost, int Weight)
            : base(EEquipmentType.Armor, SubType, Rarity, Name, Description, Cost, Weight, ModifierType)
        {
            this.ArmorClass = ArmorClass;
        }
    }
}
