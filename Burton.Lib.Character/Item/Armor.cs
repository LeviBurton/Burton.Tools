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
   
        public Armor(EItemSubType SubType, 
                     EItemRarity Rarity, 
                     EAbility ModifierType, 
                     int ArmorClass, 
                     string Name, 
                     string Description, 
                     int TL,
                     int Cost, 
                     int Weight,
                     List<Ability> AbilityRequirements)
            : base(EItemType.Armor, SubType, Rarity, Name, Description, TL, Cost, Weight, AbilityRequirements, null, null)
        {
            this.ArmorClass = ArmorClass;
        }

        public Armor(Armor Other)
            : base(EItemType.Armor, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.TL, Other.Cost, Other.Weight, Other.Require_Abilities, Other.DateCreated, Other.DateModified)
        {
            this.ID = Other.ID;
            this.ArmorClass = Other.ArmorClass;
        }
    }
}
