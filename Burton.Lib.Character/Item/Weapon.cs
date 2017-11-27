using System;
using System.Collections.Generic;

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
        Thunder,
        Versatile
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

    public enum EReloadType
    {
        None,
        Battery,
        Ammunition
    }

    public enum EBatteryType
    {
        None,
        Basic,
        Medium,
        High
    }

    [Serializable]
    public class WeaponProperty 
    {
        public string Name;
        public string ShortName;

        public string Description;
        public DamageType[] DamageTypes;

        public EReloadType ReloadType;
        public EBatteryType BatteryType;
        public int ReloadCharges;

        public WeaponProperty(string Name, string ShortName = null, string Description = null, DamageType[] DamageTypes = null, EReloadType ReloadType = EReloadType.None, EBatteryType BatteryType = EBatteryType.None, int ReloadCharges = 0)
        {
            this.Name = Name;
            this.ShortName = ShortName;
            this.Description = Description;
            this.DamageTypes = DamageTypes;
            this.ReloadType = ReloadType;
            this.ReloadCharges = ReloadCharges;
        }
    }

    [Serializable]
    public class DamageType
    {
        public EDamageType Type;
        public int Modifier;

        public int[] Damage;

        public DamageType(EDamageType Type, int[] Damage, int Modifier = 0)
        {
            this.Type = Type;
            this.Damage = Damage;
            this.Modifier = Modifier;
        }

        public DamageType(DamageType Other)
        {
            this.Type = Other.Type;
            this.Damage = Other.Damage;
            this.Modifier = Other.Modifier;
        }
    }

    [Serializable]
    public class Weapon : Item
    {
        public List<DamageType> DamageTypes;
        public List<EWeaponProperty> WeaponProperties;
        public List<WeaponProperty> WeaponProps;

        public int[] Range = new int[2];
        public int[] VersatileDamage = new int[2];

        public string BookSet = string.Empty;

        public Weapon(EItemSubType SubType, EItemRarity Rarity, List<DamageType> DamageTypes, int[] Range, string Name, string Description, int TL, int Cost, int Weight, List<Ability> Requirements)
            : base(EItemType.Weapon, SubType, Rarity, Name, Description, TL, Cost, Weight, Requirements, null, null)
        {
            this.DamageTypes = new List<DamageType>();
            this.Range = Range;

            foreach (var Type in DamageTypes)
            {
                this.DamageTypes.Add(new DamageType(Type));
            }

            this.WeaponProperties = new List<EWeaponProperty>();
        }

        public void Init(EItemSubType SubType, EItemRarity Rarity, List<DamageType> DamageTypes, List<EWeaponProperty> Properties, int[] Range, string Name, string Description, int Cost, int Weight, List<Ability> Requirements, List<WeaponProperty> WeaponProps = null)
        {
            this.Type = EItemType.Weapon;
            this.SubType = SubType;
            this.Rarity = Rarity;
            this.Name = Name;
            this.Description = Description;
            this.Cost = Cost;
            this.Weight = Weight;
            this.Require_Abilities = Requirements;
            this.DamageTypes = DamageTypes;
            this.Range = Range;
            this.WeaponProperties = Properties;
            this.WeaponProps = WeaponProps;
        }

        // Make a "deep" copy.
        // Probably could be better, but what do I know.
        public Weapon(Weapon Other)
            : base(EItemType.Weapon, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.TL, Other.Cost, Other.Weight, Other.Require_Abilities, Other.DateCreated, Other.DateModified)
        {
            this.ID = Other.ID;
            this.Range = Other.Range;
            this.DamageTypes = new List<DamageType>();

            foreach (var Type in Other.DamageTypes)
            {
                this.DamageTypes.Add(new DamageType(Type));
            }

            this.WeaponProps = new List<WeaponProperty>(Other.WeaponProps);
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
            Other.WeaponProps = new List<WeaponProperty>(this.WeaponProps);

            return (DbItem)Other;
        }
    }

}
