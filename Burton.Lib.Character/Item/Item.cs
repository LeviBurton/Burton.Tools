using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

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

    public enum EItemType
    {
        Armor,
        Weapon,
        Gear,
        Tool,
        Mount_And_Vehicle,
        Trade_Good,
        Poison,
        Special_Material
    }

    public enum EItemSubType
    {
        None,
        Light,
        Medium,
        Heavy,
        Shield,
        Simple_Melee,
        Simple_Ranged,
        Martial_Melee,
        Martial_Ranged,
        Spell_Material,
        One_Handed_Small_Arms,
        Two_Handed_Small_Arms,
        Super_Heavy,
        Combat_Accessories,
        Detonators,
        Tool_Kits,
        Utilities
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
        public int TL;
        public Texture2D Icon;

        public EItemType Type;
        public string TypeName
        {
            get { return Type.ToString().Replace("_", " "); }
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
                    int TL,
                    int Cost,
                    int Weight,
                    List<Ability> AbilityRequirements,
                    DateTime? DateCreated = null,
                    DateTime? DateModified = null)
            : base(DateCreated, DateModified)
        {
            Init(Type, SubType, Rarity, Name, Description, TL, Cost, Weight, AbilityRequirements);
        }

        public void Init(Item Other)
        {
            Init(Other.Type, Other.SubType, Other.Rarity, Other.Name, Other.Description, Other.TL, Other.Cost, Other.Weight, Other.Require_Abilities);
        }

        public void Init(EItemType Type,
                    EItemSubType SubType,
                    EItemRarity Rarity,
                    string Name,
                    string Description,
                    int TL,
                    int Cost,
                    int Weight,
                    List<Ability> AbilityRequirements,
                    DateTime? DateCreated = null,
                    DateTime? DateModified = null)
        {
            this.Type = Type;
            this.SubType = SubType;
            this.Rarity = Rarity;
            this.Name = Name;
            this.Description = Description;
            this.TL = TL;
            this.Weight = Weight;
            this.Cost = Cost;
            this.Require_Abilities = new List<Ability>();

            foreach (var Req in this.Require_Abilities)
            {
                this.Require_Abilities.Add(new Ability(Req));
            }
        }
    }

   

    //// Manager that wraps the ItemDB.  Knows about all the different Item Types
    //// and can get/insert/update items in the ItemDB.
    //public class ItemManager
    //{
    //    #region Singleton
    //    private static ItemManager _Instance;
    //    public static ItemManager Instance
    //    {
    //        get
    //        {
    //            if (_Instance == null)
    //                _Instance = new ItemManager();

    //            return _Instance;
    //        }
    //    }
    //    #endregion

    //    public string FileName = "Items.bytes";

    //    // we pretty much just wrap access to the ItemDB
    //    private ItemDB DB;
    //    private bool bDoBootstrap = false;

    //    public ItemManager()
    //    {
    //        DB = ItemDB.Instance;

    //        if (bDoBootstrap)
    //        {
    //            Bootstrap();
    //            return;
    //        }
    //    }

    //    public void SaveChanges()
    //    {
    //        DB.Save(FileName);
    //    }

    //    public void SaveChanges(string FilePath)
    //    {
    //        DB.Save(FilePath);
    //    }

    //    public void SaveChanges(Stream OutStream)
    //    {
    //        DB.Save(OutStream);
    //    }

    //    public void Load(string FilePath)
    //    {
    //        DB.Load(FilePath);
    //    }

    //    public void Refresh(string FilePath)
    //    {
    //        DB.Load(FilePath);
    //    }

    //    public void Refresh(Stream InStream)
    //    {
    //        DB.Load(InStream);
    //    }

    //    public void Load()
    //    {
    //        DB.Load(FileName);
    //    }

    //    public void Refresh()
    //    {
    //        DB.Load(FileName);
    //    }

    //    public IEnumerable<T> Find<T>(Func<T, bool> Predicate = null) where T : DbItem
    //    {
    //        return DB.Find(Predicate);
    //    }

    //    // Create a copy of Item and a add it to the ItemDB
    //    public int AddItem<T>(T Item) where T : DbItem
    //    {
    //        // var NewItem = (Item)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
    //        var NewItem = (Item)Item.Clone();

    //        NewItem.DateCreated = DateTime.Now;
    //        NewItem.DateModified = NewItem.DateCreated;

    //        return DB.Add((Item)NewItem);
    //    }

    //    public void UpdateItem<T>(T Item) where T : DbItem
    //    {
    //        var Copy = (Item)Item.Clone();

    //        Copy.DateModified = DateTime.Now;

    //        DB.Items[Copy.ID - 1] = Copy;
    //    }

    //    public void DeleteItem(int ID)
    //    {
    //        DB.Items[ID - 1] = null;
    //    }

    //    // Some defaults to play with
    //    public void Bootstrap()
    //    {
    //        AddBaseArmors();
    //        AddBaseWeapons();
    //    }

    //    public void ImportSpellComponents(string FileName)
    //    {
    //        var Data = File.ReadAllLines(FileName);
    //        Data[0] = null;

    //        foreach (var Line in Data)
    //        {
    //            if (string.IsNullOrEmpty(Line))
    //                continue;

    //            var Fields = Line.Split(new char[] { '\t' });
    //            var Data_Name = Fields[0];

    //            var Data_Value = Fields[1];

    //            Data_Value = Data_Value.Replace("-", "").Replace(",", "").Replace("gp", "").Trim();
    //            int Cost = 0;

    //            if (!string.IsNullOrEmpty(Data_Value) && !Int32.TryParse(Data_Value, out Cost))
    //            {
    //                continue;
    //            }

    //            var Data_Spells = Fields[2].Split(new char[] { ',' });

    //            var Data_Consumed = Fields[3] == "N" ? false : true;
    //            var Data_Notes = Fields[4];

    //            var SpellMaterial = new SpellMaterial(Data_Name, Data_Notes, Cost, Data_Consumed);

    //            SpellMaterial.ID = AddItem<SpellMaterial>(SpellMaterial);

    //            foreach (var Name in Data_Spells)
    //            {
    //         //       var Spell = SpellManager.Instance.Find<Spell>(x => x.Name == Name).SingleOrDefault();

    //                //if (Spell == null)
    //                //    continue;

    //            //    Spell.SpellMaterials.Add(SpellMaterial);
    //            //    SpellManager.Instance.UpdateItem<Spell>(Spell);
    //            }

    //       //     SpellManager.Instance.SaveChanges();
    //        }
    //    }

    //    public void AddBaseWeapons()
    //    {
    //        List<DamageType> DamageTypes = new List<DamageType>();

    //        var Damage = new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 });
    //        DamageTypes.Add(Damage);

    //        var Weapon = new Weapon(EItemSubType.Martial_Melee,
    //                          EItemRarity.Common,
    //                          DamageTypes,
    //                          new int[] { 0, 0 },
    //                          "Longsword",
    //                          "Longsword weapon",
    //                          15,
    //                          3,
    //                          new List<Ability>());
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
    //        Weapon.VersatileDamage = new int[2] { 1, 10 };
    //        AddItem<Weapon>(Weapon);

    //        DamageTypes.Clear();
    //        DamageTypes.Add(new DamageType(EDamageType.Bludgeoning, new int[] { 1, 8, 0 }));
    //        Weapon = new Weapon(EItemSubType.Martial_Melee,
    //                            EItemRarity.Common,
    //                            DamageTypes,
    //                           new int[] { 0, 0 },
    //                            "Warhammer",
    //                            "Warhammer Weapon",
    //                            15,
    //                            2,
    //                            new List<Ability>());
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Versatile);
    //        Weapon.VersatileDamage = new int[2] { 1, 10 };
    //        AddItem<Weapon>(Weapon);

    //        DamageTypes.Clear();

    //        DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 10, 0 }));
    //        Weapon = new Weapon(EItemSubType.Martial_Melee,
    //                            EItemRarity.Rare,
    //                            DamageTypes,
    //                              new int[] { 0, 0 },
    //                            "Halberd",
    //                            "Halberd Weapon",
    //                            20,
    //                            6,
    //                            new List<Ability>());

    //        Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Reach);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
    //        AddItem<Weapon>(Weapon);

    //        DamageTypes.Clear();

    //        DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 8, 0 }));
    //        Weapon = new Weapon(EItemSubType.Martial_Ranged,
    //                            EItemRarity.Common,
    //                            DamageTypes,
    //                            new int[2] { 150, 600 },
    //                            "Longbow",
    //                            "Longbow",
    //                            50,
    //                            2,
    //                           new List<Ability>());

    //        Weapon.WeaponProperties.Add(EWeaponProperty.Range);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
    //        AddItem<Weapon>(Weapon);

    //        DamageTypes.Clear();

    //        DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 10, 0 }));

    //        Weapon = new Weapon(EItemSubType.Martial_Ranged,
    //                            EItemRarity.Common,
    //                            DamageTypes,
    //                            new int[2] { 100, 400 },
    //                            "Crossbow, Heavy",
    //                            "Heavy Crossbow",
    //                            50,
    //                            18,
    //                            new List<Ability>());

    //        Weapon.WeaponProperties.Add(EWeaponProperty.Range);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Loading);
    //        Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);

    //        AddItem<Weapon>(Weapon);
    //    }

    //    public void AddBaseArmors()
    //    {
    //        //// Just create some base game armor types that will always be around.
    //        var Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Armor", 5, 8, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40, new List<Ability>());
    //        AddItem<Armor>(Armor);

    //        List<Ability> Requirements = new List<Ability>();
    //        Requirements.Add(new Ability(EAbility.Strength, 0, 0, 13));
    //        Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55, Requirements);

    //        AddItem<Armor>(Armor);
    //    }
    //}

    //public class ItemDB : SimpleDB<Item>
    //{
    //    private static ItemDB _Instance;

    //    public static ItemDB Instance
    //    {
    //        get
    //        {
    //            if (_Instance == null)
    //                _Instance = new ItemDB();

    //            return _Instance;
    //        }
    //    }

    //    public ItemDB()
    //    {
    //        InitBase();
    //    }

    //    public void InitBase()
    //    {
    //        Items.Clear();
    //    }
    //}
}
