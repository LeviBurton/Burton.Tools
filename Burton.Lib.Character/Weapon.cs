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
    public class Weapon : Equipment
    {
        public EDamageType DamageType;
        public List<EWeaponProperty> WeaponProperties;
        public int[] Damage = new int[2];
        public int[] Range = new int[2];
        public int[] VersatileDamage = new int[2];

        public Weapon(EEquipmentSubType SubType, EEquipmentRarity Rarity, EDamageType DamageType, EAbility ModifierType, int[] Damage, string Name, string Description, int Cost, int Weight)
            : base(EEquipmentType.Weapon, SubType, Rarity, Name, Description, Cost, Weight, ModifierType)
        {
            this.DamageType = DamageType;
            this.Damage = Damage;
            this.WeaponProperties = new List<EWeaponProperty>();
        }
    }
}
