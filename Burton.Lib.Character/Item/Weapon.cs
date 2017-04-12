using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EDamageType
    {
        Acid,
        Bludgeoning,
        Cold,
        Fire,
        Force,
        Lightning,
        Necrotic,
        Piercing,
        Poison,
        Pyschic,
        Radiant,
        Slashing,
        Thunder
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
    public class DamageType
    {
        public EDamageType Type;
        public int[] Damage;

        public DamageType(EDamageType Type, int[] Damage)
        {
            this.Type = Type;
            this.Damage = Damage;
        }

        public DamageType(DamageType Other)
        {
            this.Type = Other.Type;
            this.Damage = Other.Damage;
        }
    }

    [Serializable]
    public class Weapon : Item
    {
        public List<DamageType> DamageTypes;
        public List<EWeaponProperty> WeaponProperties;

        public int[] Range = new int[2];
        public int[] VersatileDamage = new int[2];

        public Weapon(EItemSubType SubType, EItemRarity Rarity, List<DamageType> DamageTypes, int[] Range, string Name, string Description, int Cost, int Weight, List<Ability> Requirements)
            : base(EItemType.Weapon, SubType, Rarity, Name, Description, Cost, Weight, Requirements, null, null)
        {
            this.DamageTypes = new List<DamageType>();
            this.Range = Range;

            foreach (var Type in DamageTypes)
            {
                this.DamageTypes.Add(new DamageType(Type));
            }

            this.WeaponProperties = new List<EWeaponProperty>();
        }

        // Make a "deep" copy.
        // Probably could be better, but what do I know.
        public Weapon(Weapon Other)
            : base(EItemType.Weapon, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.Cost, Other.Weight, Other.Require_Abilities, Other.DateCreated, Other.DateModified)
        {
            this.ID = Other.ID;
            this.Range = Other.Range;
            this.DamageTypes = new List<DamageType>();
       
            foreach (var Type in Other.DamageTypes)
            {
                this.DamageTypes.Add(new DamageType(Type));
            }

            this.WeaponProperties = new List<EWeaponProperty>(Other.WeaponProperties);
        }

        public override DbItem Clone()
        {
            Weapon Other = (Weapon)this.MemberwiseClone();
            Other.DamageTypes = new List<DamageType>();

            foreach (var Type in this.DamageTypes)
            {
                Other.DamageTypes.Add(new DamageType(Type));
            }

            Other.WeaponProperties = new List<EWeaponProperty>(this.WeaponProperties);

            return (DbItem) Other;
        }
    }
}
