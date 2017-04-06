﻿using System;
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
                    List<Ability> AbilityRequirements,
                    DateTime? DateCreated = null,
                    DateTime? DateModified = null)
            : base(DateCreated, DateModified)
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
            ItemDB = ItemDB.Instance;

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

            NewItem.DateCreated = DateTime.Now;
            NewItem.DateModified = NewItem.DateCreated;

            return ItemDB.Add((Item)NewItem);
        }

        public void UpdateItem<T>(T Item) where T : DbItem
        {
            var Copy = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));

            Copy.DateModified = DateTime.Now;

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
                // There has to be a generic alternative to doing this.
                // We basically want to call the class constructor for typeof(Item), passing in the Item so
                // that it calls the copy constructor.

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

    public class ItemDB : SimpleDB<Item>
    {
        private static ItemDB _Instance;

        public static ItemDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ItemDB();

                return _Instance;
            }
        }

        public ItemDB()
        {
            InitBase();
        }

        public void InitBase()
        {
            Items.Clear();
        }
    }
}
