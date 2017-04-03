using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Very_Rare,
        Legendary
    }

    // We need a SimpleDB and Type for each of these.
    public enum EItemType
    {
        Armor,
        Weapon,
        Adventuring_Gear,
        Tool,
        Mount_And_Vehicle,
        Trade_Good,
        Poison,
        Special_Material
    }

    public enum EItemSubType
    {
        Light,
        Medium,
        Heavy,
        Shield,
        Simple_Melee,
        Simple_Ranged,
        Martial_Melee,
        Martial_Ranged
    }

    public enum EModifierType
    {
        Armor_Class,
        Hit_Points,
        Attribute,
        Attack,
        Damage
    }

    public class Modifier
    {
        public EModifierType Type;
        public int Value;
    }

    /* 
        Base equipment class from which all other types of equipment derive.
        Provides:
            - Name
            - Description
            - Cost
            - Weight
            - Type
            - SubType
            - Rarity
            - Ability Modifier to use
            - Ability requirements to use/equip
      
        Need to figure out what else the base equipment class should provide.  
    */

    [Serializable]
    public class Item : DbItem
    {
        public string Description;
        public int Weight;
        public int Cost;

        public EItemType Type;
        public string TypeName
        {
            get { return Type.ToString().Replace("_", " ");  }
        }

        public EItemSubType SubType;
        public string SubTypeName
        {
            get { return SubType.ToString().Replace("_", " "); }
        }
        public List<Ability> Require_Abilities;
        public List<Modifier> Modifiers;

        public EAbility PrimaryAbility;
        public EItemRarity Rarity;

        public Item(EItemType Type, 
                    EItemSubType SubType, 
                    EItemRarity Rarity, 
                    string Name, 
                    string Description, 
                    int Cost, 
                    int Weight, 
                    List<Ability> AbilityRequirements)
        {
            this.Type = Type;
            this.SubType = SubType;
            this.Rarity = Rarity;
            this.Name = Name;
            this.Description = Description;
            this.Weight = Weight;
            this.Cost = Cost;
            this.Require_Abilities = new List<Ability>();

            foreach (var Req in this.Require_Abilities)
            {
                this.Require_Abilities.Add(new Ability(Req));
            }
        }
    }
}
