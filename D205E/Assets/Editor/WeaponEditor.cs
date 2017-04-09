using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;

//EditorGUILayout.Toggle("Mute All Sounds", false);
//EditorGUILayout.IntField("Player Lifes", 3);

public class WeaponEditorWindow : EditorWindow
{
    public Weapon OriginalWeapon;
    public Weapon CurrentWeapon;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;
    bool bIsChecked = false;

    public void NewWeapon()
    {
        var DamageTypes = new List<DamageType>();
        DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0}));

        var Weapon = new Weapon(EItemSubType.Martial_Melee,
                                EItemRarity.Common,
                                DamageTypes,
                                new int[] { 90, 200 },
                                "New Weapon Name",
                                "New Weapon Description",
                                0,
                                0,
                                new List<Ability>());
        CurrentWeapon = Weapon;
        OriginalWeapon = Weapon;
    }

    public void SetWeapom(Weapon Weapon)
    {
        this.OriginalWeapon = Weapon;
        this.CurrentWeapon = Weapon;
        this.Repaint();
    }

    void OldLayout()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            if (CurrentWeapon.ID <= 0)
            {
                CurrentWeapon.ID = ItemManager.Instance.AddItem<Weapon>(CurrentWeapon);
            }
            else
            {
                ItemManager.Instance.UpdateItem<Weapon>(this.CurrentWeapon);
            }

            ItemManager.Instance.SaveChanges();

            EditorWindow.GetWindow(typeof(WeaponListWindow)).Repaint();
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(100)))
        {
            this.Close();
        }
        GUILayout.EndHorizontal();


        CurrentWeapon.Name = EditorGUILayout.TextField("Name", CurrentWeapon.Name);
        CurrentWeapon.SubType = (EItemSubType)EditorGUILayout.Popup("Type", (int)CurrentWeapon.SubType, Enum.GetNames(typeof(EItemSubType)));

        //CurrentWeapon.DamageType = (EDamageType)EditorGUILayout.Popup("Damage Type", (int)CurrentWeapon.DamageType, Enum.GetNames(typeof(EDamageType)));
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Damage");
        //GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
        //myStyle.alignment = TextAnchor.MiddleCenter;

        //GUILayout.TextField(CurrentWeapon.Damage[0].ToString(), myStyle, GUILayout.Width(25));
        //GUILayout.TextField(CurrentWeapon.Damage[1].ToString(), myStyle, GUILayout.Width(25));
        //GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Damage Types");
        GUILayout.EndHorizontal();

        CurrentWeapon.Rarity = (EItemRarity)EditorGUILayout.Popup("Rarity", (int)CurrentWeapon.Rarity, Enum.GetNames(typeof(EItemRarity)));
        CurrentWeapon.Weight = EditorGUILayout.IntField("Weight", CurrentWeapon.Weight);
        CurrentWeapon.Cost = EditorGUILayout.IntField("Cost", CurrentWeapon.Cost);

        EditorGUILayout.LabelField("Weapon Properties");

        EditorGUILayout.BeginVertical();
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

        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Description");
        CurrentWeapon.Description = EditorGUILayout.TextArea(CurrentWeapon.Description, GUILayout.Height(100));
        //     Debug.LogFormat("OnGUI: {0} '{1}'", CurrentWeapon.Name, CurrentWeapon.Description);
        GUILayout.EndVertical();
    }

    void OnGUI()
    {
        int MaxLabelWidth = 150;
        int SingleDigitWidth = 25;

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            if (CurrentWeapon.ID <= 0)
            {
                CurrentWeapon.ID = ItemManager.Instance.AddItem<Weapon>(CurrentWeapon);
            }
            else
            {
                ItemManager.Instance.UpdateItem<Weapon>(CurrentWeapon);
            }

            ItemManager.Instance.SaveChanges();

            EditorWindow.GetWindow(typeof(WeaponListWindow)).Repaint();
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

public class WeaponListWindow : EditorWindow
{
    private Vector2 ScrollVector;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;
    WeaponEditorWindow WeaponEditor;

    [MenuItem("D20/Weapons")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WeaponListWindow));
    }

    void OnGUI()
    {
        var Items = ItemManager.Instance.Find<Weapon>().ToList();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Weapon", GUILayout.Width(DefaultWidth)))
        {
            if (WeaponEditor == null)
            {
                WeaponEditor = (WeaponEditorWindow)EditorWindow.GetWindow(typeof(WeaponEditorWindow));
            }

            WeaponEditor.titleContent.text = "New Weapon";
            WeaponEditor.NewWeapon();
        }
  
        GUILayout.EndHorizontal();

      

        GUILayout.BeginVertical(GUI.skin.box);
        // Column Headers
        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        GUILayout.BeginHorizontal();
        GUILayout.Space(IconWidth + 15);
        GUILayout.Space(IconWidth + 15);
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("SubType", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Damage Types", EditorStyles.boldLabel, GUILayout.Width(LargeWidth - 30));
        GUILayout.Label("Description", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Rarity", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Cost", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Weight", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        //GUILayout.Label("Created", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        //GUILayout.Label("Modified", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.EndHorizontal();

        foreach (var Weapon in Items)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E", GUILayout.Width(IconWidth + 10)))
            {
                if (WeaponEditor == null)
                {
                    WeaponEditor = (WeaponEditorWindow)EditorWindow.GetWindow(typeof(WeaponEditorWindow));
                }

                WeaponEditor.titleContent.text = string.Format("Editing: {0}", Weapon.Name);
                WeaponEditor.SetWeapom(Weapon);
            }

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Weapon.Name, "OK", "Cancel"))
                {
                    ItemManager.Instance.DeleteItem(Weapon.ID);
                    ItemManager.Instance.SaveChanges();
                }
            }

            GUILayout.Label(Weapon.Name, GUILayout.Width(LargeWidth));
            GUILayout.Label(Weapon.SubTypeName, GUILayout.Width(DefaultWidth));

            GUILayout.BeginVertical(GUILayout.Width(LargeWidth-30));
   
            foreach (var DamageType in Weapon.DamageTypes)
            { 
                if (DamageType.Damage[0] == 0)
                    GUILayout.Label(string.Format("{0} ({1})", DamageType.Type.ToString(), DamageType.Damage[2].ToString("+0;-#")));
                else
                    GUILayout.Label(string.Format("{0} ({1}D{2}{3})", DamageType.Type.ToString(), DamageType.Damage[0], DamageType.Damage[1], DamageType.Damage[2].ToString("+0;-#")));
            }
     
            GUILayout.EndVertical();

            GUILayout.Label(Weapon.Description, EditorStyles.wordWrappedLabel, GUILayout.Width(LargeWidth));
            GUILayout.Label(Weapon.Rarity.ToString().Replace("_", " "), GUILayout.Width(DefaultWidth));
            GUILayout.Label(Weapon.Cost.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.Label(Weapon.Weight.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.Label(Weapon.ID.ToString(), GUILayout.Width(SmallWidth));
            //GUILayout.Label(Weapon.DateCreated.ToString(), GUILayout.Width(LargeWidth));
            //GUILayout.Label(Weapon.DateModified.ToString(), GUILayout.Width(LargeWidth));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
