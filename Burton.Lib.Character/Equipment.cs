using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EEquipmentRarity
    {
        Common,
        Uncommon,
        Rare,
        Very_Rare,
        Legendary
    }

    public enum EEquipmentType
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

    public enum EEquipmentSubType
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

    [Serializable]
    public class Equipment : DbItem
    {
        public string Description;
        public int Weight;
        public int Cost;

        public EEquipmentType Type;
        public EEquipmentSubType SubType;
        public List<Ability> AbilityRequirements;
        public EAbility AbilityModifierType;
        public EEquipmentRarity Rarity;

        public Equipment(EEquipmentType Type, EEquipmentSubType SubType, EEquipmentRarity Rarity, string Name, string Description, int Cost, int Weight, EAbility AbilityModifierType)
        {
            this.Type = Type;
            this.SubType = SubType;
            this.Rarity = Rarity;
            this.Name = Name;
            this.Description = Description;
            this.Weight = Weight;
            this.Cost = Cost;
            this.AbilityModifierType = AbilityModifierType;
            this.AbilityRequirements = new List<Ability>();
        }
    }
}
