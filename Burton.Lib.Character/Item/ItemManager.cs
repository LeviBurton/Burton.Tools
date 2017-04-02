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

        public string FileName = "Items.sdb";

        // we pretty much just wrap access to the ItemDB/
        private ItemDB ItemDB;
        private bool bDoBootstrap = false;

        public ItemManager()
        {
            ItemDB = new ItemDB();

            if (bDoBootstrap)
                Bootstrap();

            Refresh();
        }

        public void SaveChanges()
        {
            ItemDB.Save(FileName);
        }

        public void Refresh()
        {
            ItemDB.Load(FileName);
        }

        // Create a copy of Item and a add it to the ItemDB
        public void AddItem<T>(T Item)
        {
            var NewItem = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
            ItemDB.Add((Item)NewItem);
        }

        public void UpdateItem<T>(T Item) where T : DbItem
        {
            var Copy = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
            ItemDB.Items[Copy.ID - 1] = Copy;
        }

        // Get a copy of the Item by ID
        public T GetItemCopy<T>(int ID)
        {
            Item Item = null;

            try
            {
                Item = ItemDB.Get(ID);
            }
            catch (Exception Ex)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
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

        // Some defaults to play with
        public void Bootstrap()
        {
            AddBaseArmors();
            AddBaseWeapons();
            SaveChanges();
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
            AddItem<Weapon>(Weapon);

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
            AddItem<Weapon>(Weapon);

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
            AddItem<Weapon>(Weapon);

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
            AddItem<Weapon>(Weapon);

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
            AddItem<Weapon>(Weapon);
        }

        public void AddBaseArmors()
        {
            //// Just create some base game armor types that will always be around.
            var Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Armor", 5, 8);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40);
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55);
            Armor.AbilityRequirements.Add(new Ability(EAbility.Strength, 1, 0, 13));
            AddItem<Armor>(Armor);
        }
    }
}
