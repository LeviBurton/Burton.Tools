using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

[CustomEditor(typeof(Weapon))]
public class LevelScriptEditor : Editor
{
    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;
    int MaxLabelWidth = 150;
    int SingleDigitWidth = 25;
    bool bIsChecked = false;

    public override void OnInspectorGUI()
    {
        Weapon CurrentWeapon = target as Weapon;

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Name = GUILayout.TextField(CurrentWeapon.Name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Type: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.SubType = (EItemSubType)EditorGUILayout.Popup((int)CurrentWeapon.SubType, Enum.GetNames(typeof(EItemSubType)));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Range:", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Range[0] = EditorGUILayout.IntField(CurrentWeapon.Range[0], GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.Label("/", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth));
        CurrentWeapon.Range[1] = EditorGUILayout.IntField(CurrentWeapon.Range[1], GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Damage Types:", GUILayout.Width(MaxLabelWidth));

        GUILayout.BeginVertical();

        if (GUILayout.Button("Add"))
        {
            CurrentWeapon.DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 }));
            this.Repaint();
        }

        var DeletedDamageType = new List<DamageType>();

        foreach (var Type in CurrentWeapon.DamageTypes)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                DeletedDamageType.Add(Type);
            }

            Type.Type = (EDamageType)EditorGUILayout.Popup((int)Type.Type, Enum.GetNames(typeof(EDamageType)));

            Type.Damage[0] = EditorGUILayout.IntField(Type.Damage[0], GUILayout.Width(SingleDigitWidth));
            GUILayout.Label("d", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth / 2));
            Type.Damage[1] = EditorGUILayout.IntField(Type.Damage[1], GUILayout.Width(SingleDigitWidth));

            GUILayout.Label("+/-", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth + 5));
            Type.Damage[2] = EditorGUILayout.IntField(Type.Damage[2], GUILayout.Width(SingleDigitWidth + 15));

            GUILayout.EndHorizontal();
        }

        foreach (var DamageType in DeletedDamageType)
        {
            CurrentWeapon.DamageTypes.Remove(DamageType);
        }

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Rarity: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Rarity = (EItemRarity)EditorGUILayout.Popup((int)CurrentWeapon.Rarity, Enum.GetNames(typeof(EItemRarity)));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weight: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Weight = EditorGUILayout.IntField(CurrentWeapon.Weight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cost: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Cost = EditorGUILayout.IntField(CurrentWeapon.Cost);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Description = GUILayout.TextArea(CurrentWeapon.Description, GUILayout.Height(80));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Properties: ", GUILayout.Width(MaxLabelWidth));

        GUILayout.BeginVertical();
        foreach (var PropName in Enum.GetNames(typeof(EWeaponProperty)))
        {
            EWeaponProperty Prop = (EWeaponProperty)Enum.Parse(typeof(EWeaponProperty), PropName);
            var bShouldBeChecked = CurrentWeapon.WeaponProperties.Contains(Prop);
            bIsChecked = EditorGUILayout.ToggleLeft(PropName, bShouldBeChecked);

            if (!bIsChecked)
            {
                CurrentWeapon.WeaponProperties.Remove(Prop);
            }
            else if (!CurrentWeapon.WeaponProperties.Contains(Prop))
            {
                CurrentWeapon.WeaponProperties.Add(Prop);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

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
                    WeaponSubType = (EItemSubType)Enum.Parse(typeof(EItemSubType), tmpSubType, true);
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
                        int[] DamageDice = new int[3] { Convert.ToInt32(tmpValue[0]), Convert.ToInt32(tmpValue[1]), Modifier };
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
public class WeaponEditorWindow : EditorWindow
{
    public WeaponListWindow WeaponListWindow;

    public Weapon OriginalWeapon;
    public Weapon CurrentWeapon;
    string DatabaseFile;
    readonly string DefaultWeaponName = "Default Weapon";
    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;
    int MaxLabelWidth = 150;
    int SingleDigitWidth = 25;

    bool bIsChecked = false;

    private void OnEnable()
    {
        WeaponListWindow = EditorWindow.GetWindow<WeaponListWindow>();
    }

    public void NewWeapon()
    { 
       // var Asset = ItemManager.Instance.CreateAsset<Weapon>(DefaultWeaponName, true);

        var Asset = ScriptableObject.CreateInstance<Weapon>();

        Asset.Init(EItemSubType.Martial_Melee,
                    EItemRarity.Common,
                    new List<DamageType>()
                    {
                        new DamageType(EDamageType.Slashing, new int[] { 1, 6, 0 })
                    },
                    new List<EWeaponProperty>()
                    {
                    },
                    new int[] { 0, 0 },
                    "",
                    "",
                    15,
                    3,
                    new List<Ability>());

        Asset.VersatileDamage = new int[2] { 1, 10 };
      
        CurrentWeapon = Asset;
        OriginalWeapon = Asset;
    }

    public void SetWeapom(Weapon Weapon)
    {
        this.OriginalWeapon = Weapon;
        this.CurrentWeapon = Weapon;
        this.Repaint();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            CurrentWeapon.DateModified = DateTime.Now;
            EditorUtility.SetDirty(CurrentWeapon);

            ItemManager.Instance.SaveAsset(CurrentWeapon);

            if (WeaponListWindow != null)
            {
                WeaponListWindow.RefreshList();
                WeaponListWindow.Repaint();
            }
        }

        if (GUILayout.Button("Close"))
        {
            this.Close();
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Name: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Name = GUILayout.TextField(CurrentWeapon.Name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Type: ",  GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.SubType = (EItemSubType)EditorGUILayout.Popup((int)CurrentWeapon.SubType, Enum.GetNames(typeof(EItemSubType)));
        GUILayout.EndHorizontal();


     

        GUILayout.BeginHorizontal();
        GUILayout.Label("Damage Types:",GUILayout.Width(MaxLabelWidth));

        GUILayout.BeginVertical();

        if (GUILayout.Button("Add"))
        {
            CurrentWeapon.DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 }));
            this.Repaint();
        }

        var DeletedDamageType = new List<DamageType>();

        foreach (var Type in CurrentWeapon.DamageTypes)
        {
            GUILayout.BeginHorizontal();
    
            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                DeletedDamageType.Add(Type);
            }

            Type.Type = (EDamageType) EditorGUILayout.Popup((int)Type.Type, Enum.GetNames(typeof(EDamageType)));

            Type.Damage[0] = EditorGUILayout.IntField(Type.Damage[0], GUILayout.Width(SingleDigitWidth));
            GUILayout.Label("d", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth/2));
            Type.Damage[1] = EditorGUILayout.IntField(Type.Damage[1], GUILayout.Width(SingleDigitWidth));

            GUILayout.Label("+/-", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth + 5));
            Type.Damage[2] = EditorGUILayout.IntField(Type.Damage[2], GUILayout.Width(SingleDigitWidth + 15));

            GUILayout.EndHorizontal();
        }

        foreach (var DamageType in DeletedDamageType)
        {
            CurrentWeapon.DamageTypes.Remove(DamageType);
        }

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Range:", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Range[0] = EditorGUILayout.IntField(CurrentWeapon.Range[0], GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.Label("/", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth));
        CurrentWeapon.Range[1] = EditorGUILayout.IntField(CurrentWeapon.Range[1], GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Rarity: ",GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Rarity = (EItemRarity)EditorGUILayout.Popup((int)CurrentWeapon.Rarity, Enum.GetNames(typeof(EItemRarity)));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cost: ",GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Cost = EditorGUILayout.IntField(CurrentWeapon.Cost);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weight: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Weight = EditorGUILayout.IntField(CurrentWeapon.Weight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description: ", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Description = GUILayout.TextArea(CurrentWeapon.Description, GUILayout.Height(80));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Properties: ",  GUILayout.Width(MaxLabelWidth));

        GUILayout.BeginVertical();
        foreach (var PropName in Enum.GetNames(typeof(EWeaponProperty)))
        {
            EWeaponProperty Prop = (EWeaponProperty)Enum.Parse(typeof(EWeaponProperty), PropName);
            var bShouldBeChecked = CurrentWeapon.WeaponProperties.Contains(Prop);
            bIsChecked = EditorGUILayout.ToggleLeft(PropName, bShouldBeChecked);

            if (!bIsChecked)
            {
                CurrentWeapon.WeaponProperties.Remove(Prop);
            }
            else if (!CurrentWeapon.WeaponProperties.Contains(Prop))
            {
                CurrentWeapon.WeaponProperties.Add(Prop);
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}

