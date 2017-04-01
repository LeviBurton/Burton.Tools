using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    [Serializable]
    public class Armor : Item
    {
        public int ArmorClass;
   
        public Armor(EItemSubType SubType, EItemRarity Rarity, EAbility ModifierType, int ArmorClass, string Name, string Description, int Cost, int Weight)
            : base(EItemType.Armor, SubType, Rarity, Name, Description, Cost, Weight, ModifierType)
        {
            this.ArmorClass = ArmorClass;
        }
    }
}
