using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EDamageType
    {
        Slashing,
        Bludgeoning,
        Piercing
    }

    public enum EWeaponProperty
    {
        Ammunition,
        Finesse,
        Heavy,
        Light,
        Loading,
        Range,
        Reach,
        Special,
        Thrown,
        Two_Handed,
        Versatile
    }

    [Serializable]
    public class Weapon : Item
    {
        public EDamageType DamageType;
        public string DamageTypeName
        {
            get { return DamageType.ToString().Replace("_", " "); }
        }

        public List<EWeaponProperty> WeaponProperties;
        public int[] Damage = new int[2];
        public int[] Range = new int[2];
        public int[] VersatileDamage = new int[2];

        public Weapon(EItemSubType SubType, EItemRarity Rarity, EDamageType DamageType, EAbility ModifierType, int[] Damage, string Name, string Description, int Cost, int Weight)
            : base(EItemType.Weapon, SubType, Rarity, Name, Description, Cost, Weight, ModifierType)
        {
            this.DamageType = DamageType;
            this.Damage = Damage;
            this.WeaponProperties = new List<EWeaponProperty>();
        }

        // Make a "deep" copy.
        // Probably could be better, but what do I know.
        public Weapon(Weapon Other)
            : base(EItemType.Weapon, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.Cost, Other.Weight, Other.AbilityModifierType)
        {
            this.ID = Other.ID;
            this.DamageType = Other.DamageType;
            this.Damage = Other.Damage;
            this.Name = Other.Name;
            this.Description = Other.Description;
            this.WeaponProperties = new List<EWeaponProperty>(Other.WeaponProperties);
        }
    }
}
