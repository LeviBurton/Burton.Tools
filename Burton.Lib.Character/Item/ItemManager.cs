using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    // Manager that wraps the ItemDB.  Knows about all the different Item Types
    // and can get/insert/update items in the ItemDB.
    public class ItemManager
    {
        #region Singleton
        private static ItemManager _Instance;
        public static ItemManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ItemManager();

                return _Instance;
            }
        }
        #endregion

        // we pretty much just wrap access to the ItemDB/
        public ItemDB ItemDB;

        public ItemManager()
        {
            ItemDB = new ItemDB();

            AddBaseArmors();
            AddBaseWeapons();
        }

        public void AddArmor(Armor Armor)
        {
            ItemDB.Add(new Armor(Armor));
        }

        public void AddWeapon(Weapon Weapon)
        {
            ItemDB.Add(new Weapon(Weapon));
        }

        // Returns a list containing copies of the items in the ItemDB
        public List<Item> GetItemsCopy()
        {
            List<Item> Result = new List<Item>();

            foreach (var Item in ItemDB.Items)
            {
                if (Item.Type == EItemType.Weapon)
                {
                    Result.Add(new Weapon((Weapon)Item));
                }
                else if (Item.Type == EItemType.Armor)
                {
                    Result.Add(new Armor((Armor)Item));
                }
            }

            return Result;
        }

        public Item GetItemCopy<T>(int ID)
        {
            var Item = ItemDB.Get(ID);
       
            if (typeof(T) == typeof(Armor))
            {
                return new Armor((Armor)Item);
            }
            else if (typeof(T) == typeof(Weapon))
            {
                return new Weapon((Weapon)Item);
            }
         
            return Item;
        }

        public void AddBaseWeapons()
        {
            var Weapon = new Weapon(EItemSubType.Martial_Melee,
                              EItemRarity.Common,
                              EDamageType.Slashing,
                              EAbility.Strength,
                              new int[] { 1, 8 },
                              "Longsword",
                              "Longsword weapon",
                              15,
                              3);
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            AddWeapon(Weapon);

            Weapon = new Weapon(EItemSubType.Martial_Melee,
                                EItemRarity.Common,
                                EDamageType.Bludgeoning,
                                EAbility.Strength,
                                new int[] { 1, 8 },
                                "Warhammer",
                                "Warhammer Weapon",
                                15,
                                2);
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            AddWeapon(Weapon);

            Weapon = new Weapon(EItemSubType.Martial_Melee,
                                EItemRarity.Rare,
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

            AddWeapon(Weapon);

            Weapon = new Weapon(EItemSubType.Martial_Ranged,
                                EItemRarity.Common,
                                EDamageType.Piercing,
                                EAbility.Dexterity,
                                new int[] { 1, 8 },
                                "Longbow",
                                "Longbow",
                                50,
                                2);

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);

            // FIXME: need to figure out how to specify what
            // ammo type the weapon requires
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            Weapon.Range = new int[2] { 150, 600 };
            AddWeapon(Weapon);

            Weapon = new Weapon(EItemSubType.Martial_Ranged,
                                EItemRarity.Common,
                                EDamageType.Piercing,
                                EAbility.Dexterity,
                                new int[] { 1, 10 },
                                "Crossbow, Heavy",
                                "Heavy Crossbow",
                                50,
                                18);

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            Weapon.WeaponProperties.Add(EWeaponProperty.Loading);
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            Weapon.Range = new int[2] { 100, 400 };
            AddWeapon(Weapon);
        }

        public void AddBaseArmors()
        {
            //// Just create some base game armor types that will always be around.
            var Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Armor", 5, 8);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40);
            AddArmor(Armor);

            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55);
            Armor.AbilityRequirements.Add(new Ability(EAbility.Strength, 1, 0, 13));
            AddArmor(Armor);

        }
    }
}
