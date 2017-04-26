using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;
using System.IO;

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
                        new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 })
                    },
                    new List<EWeaponProperty>()
                    {
                        EWeaponProperty.Versatile
                    },
                    new int[] { 0, 0 },
                    DefaultWeaponName,
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
        GUILayout.Label("Range:", GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Range[0] = EditorGUILayout.IntField(CurrentWeapon.Range[0], GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.Label("/", GUILayout.Width(MaxLabelWidth), GUILayout.Width(SingleDigitWidth));
        CurrentWeapon.Range[1] = EditorGUILayout.IntField(CurrentWeapon.Range[1], GUILayout.Width(SingleDigitWidth * 4));
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
        GUILayout.Label("Rarity: ",GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Rarity = (EItemRarity)EditorGUILayout.Popup((int)CurrentWeapon.Rarity, Enum.GetNames(typeof(EItemRarity)));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weight: ",  GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Weight = EditorGUILayout.IntField(CurrentWeapon.Weight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cost: ",GUILayout.Width(MaxLabelWidth));
        CurrentWeapon.Cost = EditorGUILayout.IntField(CurrentWeapon.Cost);
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

