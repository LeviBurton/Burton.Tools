using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

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

    // Move to a UnityEditor specific file since we won't be dealing with the asset database directly in game, i dont thnk.
    public class WeaponManager
    {
        static string AssetsBasePath = @"Assets/Resources/Data/Items/Weapons";
        public List<Weapon> Items;
        public List<WeaponProperty> WeaponProperties;

        #region Singleton
        private static WeaponManager _Instance;
        public static WeaponManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new WeaponManager();

                return _Instance;
            }
        }
        #endregion

        public WeaponManager()
        {
            Items = new List<Weapon>();
            WeaponProperties = new List<WeaponProperty>();
        }

        public void RefreshAssets()
        {
            Items = new List<Weapon>();

            var ItemGuids = AssetDatabase.FindAssets("t:Weapon", new string[] { AssetsBasePath });

            foreach (string ItemGuid in ItemGuids)
            {
                var AssetPath = AssetDatabase.GUIDToAssetPath(ItemGuid);
                var Asset = AssetDatabase.LoadAssetAtPath<Weapon>(AssetPath);
                Items.Add(Asset);
            }
        }

        public void DeleteAll()
        {
            // Delete all assets
            var ItemGuids = AssetDatabase.FindAssets("t:Weapon", new string[] { AssetsBasePath });
            foreach (string ItemGuid in ItemGuids)
            {
                var AssetPath = AssetDatabase.GUIDToAssetPath(ItemGuid);
                AssetDatabase.DeleteAsset(AssetPath);
            }
        }

        public void SaveAsset<T>(T Asset) where T : Item
        {
            var AssetPath = AssetDatabase.GetAssetPath(Asset);
            if (string.IsNullOrEmpty(AssetPath))
            {
                AssetPath = AssetsBasePath + string.Format(@"/{0}.asset", Asset.Name.Replace(" ", "_"));
                AssetDatabase.CreateAsset(Asset, AssetPath);
            }

            var AssetFileName = Path.GetFileNameWithoutExtension(AssetPath);

            if (Asset.Name != AssetFileName.Replace("_", " "))
            {
                var NewFileName = "/" + Asset.Name.Replace(" ", "_");
                AssetDatabase.RenameAsset(AssetPath, NewFileName);
            }

            AssetDatabase.SaveAssets();
        }

        // Should handle all item types
        public T CreateAsset<T>(string Name, bool bOnlyCreateInstance = false) where T : Item
        {
            var AssetPath = AssetsBasePath + string.Format(@"/{0}.asset", Name.Replace(" ", "_"));
            T ItemAsset = ScriptableObject.CreateInstance<T>();
            if (!bOnlyCreateInstance)
            {
                AssetDatabase.CreateAsset(ItemAsset, AssetPath);
            }
            return ItemAsset;
        }

        public void CreateWeaponProperties()
        {
            WeaponProperties = new List<WeaponProperty>();

            WeaponProperties.Add(new WeaponProperty("AP", "ap", "Armor Piercing"));
            WeaponProperties.Add(new WeaponProperty("Auto", "auto"));
            WeaponProperties.Add(new WeaponProperty("Auto Heavy", "auto-heavy"));
            WeaponProperties.Add(new WeaponProperty("Burst Fire", "burst fire"));
            WeaponProperties.Add(new WeaponProperty("Direct", "direct"));
            WeaponProperties.Add(new WeaponProperty("ESP", "ESP", "Electronis Stacked Projectiles"));
            WeaponProperties.Add(new WeaponProperty("EXP", "exp"));
            WeaponProperties.Add(new WeaponProperty("Feed", "feed"));
            WeaponProperties.Add(new WeaponProperty("Grenade", "grenade"));
            WeaponProperties.Add(new WeaponProperty("Guided", "guided"));
            WeaponProperties.Add(new WeaponProperty("Laser", "laser"));
            WeaponProperties.Add(new WeaponProperty("Magnetic", "magnetic"));
            WeaponProperties.Add(new WeaponProperty("Mastercraft", "mastercraft"));
            WeaponProperties.Add(new WeaponProperty("Nuclear", "nuclear"));
            WeaponProperties.Add(new WeaponProperty("Pincher", "pincher"));
            WeaponProperties.Add(new WeaponProperty("Plasma", "plasma"));
            WeaponProperties.Add(new WeaponProperty("Reload", "reload"));
            WeaponProperties.Add(new WeaponProperty("Shotgun", "shotgun"));
            WeaponProperties.Add(new WeaponProperty("Sniper", "sniper"));
            WeaponProperties.Add(new WeaponProperty("Sonic", "sonic"));
            WeaponProperties.Add(new WeaponProperty("Undermount", "undermount"));
        }

        public void Import(string FileName)
        {
            DeleteAll();

            CreateWeaponProperties();
            AddBaseWeapons();

            var Data = File.ReadAllLines(FileName);

            //UnityEngine.Object prefab = EditorUtility.CreateEmptyPrefab(PrefabPath + ".prefab");
            //EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

            var ColumnMapping = new Dictionary<string, int>();
            var Columns = Data[0].Split(new char[] { '\t' });

            for (int i = 0; i < Columns.Count(); i++)
            {
                ColumnMapping[Columns[i]] = i;
            }

            for (int i = 1; i < Data.Count(); i++)
            {
                var Line = Data[i].Split(new char[] { '\t' });
                if (string.IsNullOrEmpty(Line[0]))
                    continue;

                EItemSubType WeaponSubType = EItemSubType.None;
                EItemRarity WeaponRarity = EItemRarity.Common;
                List<DamageType> WeaponDamageTypes = new List<DamageType>();
                int[] WeaponRange = new int[2];

                string WeaponName = string.Empty;
                string WeaponDescription = string.Empty;
                int WeaponTL = 0;
                int WeaponCost = 0;
                int WeaponWeight = 0;
                List<Ability> AbilityRequirements = new List<Ability>();

                string tmpDamage = string.Empty;

                for (int Col = 0; Col < Line.Count(); Col++)
                {
                    if (Col == ColumnMapping["Name"])
                    {
                        WeaponName = Line[Col];
                    }
                    else if (Col == ColumnMapping["SubType"])
                    {
                        var tmpSubType = Line[Col];
                        tmpSubType = tmpSubType.Replace(" ", "_");
                        WeaponSubType = (EItemSubType) Enum.Parse(typeof(EItemSubType), tmpSubType, true);
                    }
                    else if (Col == ColumnMapping["Damage"])
                    {
                        tmpDamage = Line[Col];
                        var PropName = tmpDamage.Trim();
                        var Match = Regex.Match(PropName, @"(?<=\().+?(?=\))");
                        string PropValue = string.Empty;

                        if (Match.Success)
                        {
                            PropValue = Match.Value.Trim();
                            PropName = PropName.Substring(0, PropName.IndexOf("("));

                            var ValuesAndModifiers = PropValue.Split(new char[] { '+' });
                            int Modifier = 0;

                            if (ValuesAndModifiers.Count() > 1)
                            {
                                Modifier = Convert.ToInt32(ValuesAndModifiers[1]);
                            }

                            string[] tmpValue = ValuesAndModifiers[0].Split(new char[] { 'd' });
                            int[] DamageDice = new int[3] { Convert.ToInt32(tmpValue[0]), Convert.ToInt32(tmpValue[1]), Modifier};
                            WeaponDamageTypes.Add(new DamageType((EDamageType)Enum.Parse(typeof(EDamageType), PropName, true), DamageDice));
                        }
                    }
                    else if (Col == ColumnMapping["Weight"])
                    {
                        WeaponWeight = Convert.ToInt32(Line[Col]);
                    }
                    else if (Col == ColumnMapping["Cost"])
                    {
                        WeaponCost = Convert.ToInt32(Line[Col]);
                    }
                    else if (Col == ColumnMapping["TL"])
                    {
                        WeaponTL = Convert.ToInt32(Line[Col]);
                    }
                    else if (Col == ColumnMapping["Range"])
                    {
                        var Range1 = Convert.ToInt32(Line[Col].Split(new char[] { '/' })[0]);
                        var Range2 = Convert.ToInt32(Line[Col].Split(new char[] { '/' })[1]);
                        WeaponRange = new int[2] { Range1, Range2 };
                    }
                    else if (Col == ColumnMapping["Properties"])
                    {
                        var PropertyString = Line[Col].Split(new char[] { ',' });

                        foreach (string Name in PropertyString)
                        {
                            var PropName = Name.Trim();
                            var Match = Regex.Match(PropName, @"(?<=\().+?(?=\))");
                            string PropValue = string.Empty;

                            if (Match.Success)
                            {
                                PropValue = Match.Value;
                                PropName = PropName.Substring(0, PropName.IndexOf("("));
                            }

                            var WeaponProperty = WeaponProperties.Find(x => x.ShortName == PropName);
                            if (WeaponProperty != null)
                            {
                                if (WeaponProperty.ShortName == "reload")
                                {
                                    WeaponProperty.ReloadCharges = Convert.ToInt32(PropValue);
                                }
                            }
                        }
                    }

                    var Weapon = CreateAsset<Weapon>(WeaponName);
                    Weapon.Init(WeaponSubType, WeaponRarity, WeaponDamageTypes, new List<EWeaponProperty>(), WeaponRange, WeaponName, WeaponDescription, WeaponCost, WeaponWeight, new List<Ability>(), WeaponProperties);

                    SaveAsset<Weapon>(Weapon);
                }
            }
        }

        public void AddBaseWeapons()
        {
            var Weapon = CreateAsset<Weapon>("Longsword");

            Weapon.Init(EItemSubType.Martial_Melee,
                        EItemRarity.Common,
                        new List<DamageType>()
                        {
                            new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 })
                        },
                        new List<EWeaponProperty>()
                        {
                            EWeaponProperty.Versatile
                        },
                        new int[] { 0, 0 },
                        "Longsword",
                        "Longsword weapon",
                        15,
                        3,
                        new List<Ability>());
            Weapon.VersatileDamage = new int[2] { 1, 10 };

            Weapon = CreateAsset<Weapon>("Warhammer");
            Weapon.Init(EItemSubType.Martial_Melee,
                        EItemRarity.Common,
                        new List<DamageType>()
                        {
                            new DamageType(EDamageType.Bludgeoning, new int[] { 1, 8, 0 })
                        },
                        new List<EWeaponProperty>()
                        {
                            EWeaponProperty.Versatile
                        },
                        new int[] { 0, 0 },
                        "Warhammer",
                        "Warhammer weapon",
                        15,
                        2,
                        new List<Ability>());
            Weapon.VersatileDamage = new int[2] { 1, 10 };

            Weapon = CreateAsset<Weapon>("Longbow");
            Weapon.Init(EItemSubType.Martial_Ranged,
                        EItemRarity.Common,
                        new List<DamageType>()
                        {
                            new DamageType(EDamageType.Piercing, new int[] { 1, 8, 0 })
                        },
                        new List<EWeaponProperty>()
                        {
                            EWeaponProperty.Range,
                            EWeaponProperty.Heavy,
                            EWeaponProperty.Two_Handed,
                            EWeaponProperty.Ammunition
                        },
                        new int[] { 150, 600 },
                        "Longbow",
                        "Longbow Weapom",
                        11,
                        2,
                        new List<Ability>());
            AssetDatabase.SaveAssets();

            //DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 8, 0 }));
            //Weapon = new Weapon(EItemSubType.Martial_Ranged,
            //                    EItemRarity.Common,
            //                    DamageTypes,
            //                    new int[2] { 150, 600 },
            //                    "Longbow",
            //                    "Longbow",
            //                    50,
            //                    2,
            //                   new List<Ability>());

            //Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);
            //AddItem<Weapon>(Weapon);

            //DamageTypes.Clear();

            //DamageTypes.Add(new DamageType(EDamageType.Piercing, new int[] { 1, 10, 0 }));

            //Weapon = new Weapon(EItemSubType.Martial_Ranged,
            //                    EItemRarity.Common,
            //                    DamageTypes,
            //                    new int[2] { 100, 400 },
            //                    "Crossbow, Heavy",
            //                    "Heavy Crossbow",
            //                    50,
            //                    18,
            //                    new List<Ability>());

            //Weapon.WeaponProperties.Add(EWeaponProperty.Range);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Heavy);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Two_Handed);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Loading);
            //Weapon.WeaponProperties.Add(EWeaponProperty.Ammunition);

            //AddItem<Weapon>(Weapon);
        }

    }
}
