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

        // we pretty much just wrap access to the ItemDB
        private ItemDB ItemDB;
        private bool bDoBootstrap = false;

        public ItemManager()
        {
            ItemDB = new ItemDB();

            if (bDoBootstrap)
            {
                Bootstrap();
                SaveChanges();
                return;
            }

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
        public int AddItem<T>(T Item)
        {
            var NewItem = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
            return ItemDB.Add((Item)NewItem);
        }

        public void UpdateItem<T>(T Item) where T : DbItem
        {
            var Copy = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
            ItemDB.Items[Copy.ID - 1] = Copy;
        }

        public void DeleteItem(int ID)
        {
            ItemDB.Items[ID - 1] = null;
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

            foreach (var Item in ItemDB.Items.Where(x => x != null))
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
            List<DamageType> DamageTypes = new List<DamageType>();

            var Damage = new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 });
            DamageTypes.Add(Damage);

            var Weapon = new Weapon(EItemSubType.Martial_Melee,
                              EItemRarity.Common,
                              DamageTypes,
                              new int[] { 0, 0 },
                              "Longsword",
                              "Longsword weapon",
                              15,
                              3,
                              new List<Ability>());
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            AddItem<Weapon>(Weapon);

            DamageTypes.Clear();
            DamageTypes.Add(new DamageType(EDamageType.Bludgeoning, new int[] { 1, 8, 0 }));
            Weapon = new Weapon(EItemSubType.Martial_Melee,
                                EItemRarity.Common,
                                DamageTypes,
                               new int[] { 0, 0 },
                                "Warhammer",
                                "Warhammer Weapon",
                                15,
                                2,
                                new List<Ability>());
            Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
            Weapon.VersatileDamage = new int[2] { 1, 10 };
            AddItem<Weapon>(Weapon);

            DamageTypes.Clear();

            DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 10, 0 }));
            Weapon = new Weapon(EItemSubType.Martial_Melee,
                                EItemRarity.Rare,
                                DamageTypes,
                                  new int[] { 0, 0 },
                                "Halberd",
                                "Halberd Weapon",
                                20,
                                6,
                                new List<Ability>());

            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Reach);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            AddItem<Weapon>(Weapon);

            DamageTypes.Clear();

            DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 8, 0 }));
            Weapon = new Weapon(EItemSubType.Martial_Ranged,
                                EItemRarity.Common,
                                DamageTypes,
                                new int[2] { 150, 600 },
                                "Longbow",
                                "Longbow",
                                50,
                                2,
                               new List<Ability>());

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            AddItem<Weapon>(Weapon);

            DamageTypes.Clear();

            DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 10, 0 }));

            Weapon = new Weapon(EItemSubType.Martial_Ranged,
                                EItemRarity.Common,
                                DamageTypes,
                                new int[2] { 100, 400 },
                                "Crossbow, Heavy",
                                "Heavy Crossbow",
                                50,
                                18,
                                new List<Ability>());

            Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            Weapon.WeaponProperties.Add(EWeaponProperty.Loading);
            Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
      
            AddItem<Weapon>(Weapon);
        }

        public void AddBaseArmors()
        {
            //// Just create some base game armor types that will always be around.
            var Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Armor", 5, 8, new List<Ability>());
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10, new List<Ability>());
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13, new List<Ability>());
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12, new List<Ability>());
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20, new List<Ability>());
            AddItem<Armor>(Armor);

            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40, new List<Ability>());
            AddItem<Armor>(Armor);

            List<Ability> Requirements = new List<Ability>();
            Requirements.Add(new Ability(EAbility.Strength, 0, 0, 13));
            Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55, Requirements);
  
            AddItem<Armor>(Armor);
        }
    }
}
