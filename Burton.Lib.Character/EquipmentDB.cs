using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class EquipmentDB : SimpleDB<Equipment>
    {
        private static EquipmentDB _Instance;

        public static EquipmentDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new EquipmentDB();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public EquipmentDB()
        {
            InitBase();
            Save("Equipment.sdb");
        }

        public void InitBase()
        {
            Items.Clear();
            // Load("Equipment.sdb");
            // return;
            AddBaseArmors();
            AddBaseWeapons();
        }

        public void AddBaseArmors()
        {
            // Just create some base game armor types that will always be around.
            var Armor = new Armor(EEquipmentSubType.Light, EEquipmentRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Leather Armor", 5, 8);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Light, EEquipmentRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Light, EEquipmentRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Medium, EEquipmentRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Medium, EEquipmentRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Heavy, EEquipmentRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40);
            Add(Armor);

            Armor = new Armor(EEquipmentSubType.Heavy, EEquipmentRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55);
            Armor.AbilityRequirements.Add(new Ability(EAbility.Strength, 1, 0, 13));
            Add(Armor);

        }

        public void AddBaseWeapons()
        {
            var Weapon = new Weapon(EEquipmentSubType.Martial_Melee,
                                    EEquipmentRarity.Common,
                                    EDamageType.Slashing,
                                    EAbility.Strength,
                                    new int[] { 1, 8 },
                                    "Longsword",
                                    "Longsword weapon",
                                    15,
                                    3);
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            Add(Weapon);

            Weapon = new Weapon(EEquipmentSubType.Martial_Melee,
                                EEquipmentRarity.Common,
                                EDamageType.Bludgeoning,
                                EAbility.Strength,
                                new int[] { 1, 8 },
                                "Warhammer",
                                "Warhammer Weapon",
                                15,
                                2);
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            Add(Weapon);

            Weapon = new Weapon(EEquipmentSubType.Martial_Melee,
                                EEquipmentRarity.Rare,
                                EDamageType.Slashing,
                                EAbility.Strength,
                                new int[] { 1, 10 },
                                "Halberd",
                                "Halberd Weapon",
                                20,
                                6);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Reach);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);

            Add(Weapon);

            Weapon = new Weapon(EEquipmentSubType.Martial_Ranged,
                                EEquipmentRarity.Common,
                                EDamageType.Piercing,
                                EAbility.Dexterity,
                                new int[] { 1, 8 },
                                "Longbow",
                                "Longbow ranged weapon",
                                50,
                                2);

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);

            // FIXME: need to figure out how to specify what
            // ammo type the weapon requires
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            Weapon.Range = new int[2] { 150, 600 };
            Add(Weapon);

            Weapon = new Weapon(EEquipmentSubType.Martial_Ranged,
                                EEquipmentRarity.Common,
                                EDamageType.Piercing,
                                EAbility.Dexterity,
                                new int[] { 1, 10 },
                                "Crossbow, Heavy",
                                "Classic Heavy Crossbow",
                                50,
                                18);

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            Weapon.WeaponProperties.Add(EWeaponProperty.Loading);
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            Weapon.Range = new int[2] { 100, 400 };
            Add(Weapon);
        }

    }
}
