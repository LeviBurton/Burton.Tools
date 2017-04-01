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

        public Armor(Armor Other)
            : base(EItemType.Armor, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.Cost, Other.Weight, Other.AbilityModifierType)
        {
            this.ID = Other.ID;
            this.ArmorClass = Other.ArmorClass;
        }
    }
}
