﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;

public class WeaponListWindow : EditorWindow
{
    static string SpellAssetsBasePath = @"Assets/Resources/Data/Items";
    private Vector2 ScrollVector;
    private string DatabaseFile;
    public List<Weapon> Items;
    public List<Weapon> FilteredItems;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;
    WeaponEditorWindow WeaponEditor;
    private GUIStyle GUIStyle_Heading1 = new GUIStyle(); 

    [MenuItem("D20/Weapons")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WeaponListWindow));
    }

    public void RefreshList()
    {
        ItemManager.Instance.RefreshAssets();
        Items = ItemManager.Instance.Find<Weapon>().ToList();
        FilteredItems = Items;
    }

    private void OnEnable()
    {
        RefreshList();
        GUIStyle_Heading1.fontSize = 14;
        GUIStyle_Heading1.fontStyle = FontStyle.Bold;
        GUIStyle_Heading1.padding = new RectOffset(0, 0, 2, 2);
    }

    void OnGUI()
    {
        Items = ItemManager.Instance.Find<Weapon>().ToList();
        var GroupedItems = ItemManager.Instance.Find<Weapon>().GroupBy(x => x.SubType, (key, g) => new { SubGroupName = key, Items = g.ToList() }).ToList();

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

        GUILayout.BeginHorizontal();
        GUILayout.Space(IconWidth + 15);
        GUILayout.Space(IconWidth + 15);
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
      //  GUILayout.Label("SubType", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Damage Types", EditorStyles.boldLabel, GUILayout.Width(LargeWidth - 30));
        GUILayout.Label("Description", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Rarity", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Cost", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Weight", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
    
        //GUILayout.Label("Created", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        //GUILayout.Label("Modified", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.EndHorizontal();

        // Column Headers
        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        foreach (var SubType in GroupedItems)
        {
            GUILayout.Label(SubType.SubGroupName.ToString().Replace("_", " "), GUIStyle_Heading1);

            foreach (var Weapon in SubType.Items)
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
                        var AssetPath = AssetDatabase.GetAssetPath(Weapon);
                        AssetDatabase.DeleteAsset(AssetPath);
                        RefreshList();
                    }
                }

                GUILayout.Label(Weapon.Name, GUILayout.Width(LargeWidth));
          //      GUILayout.Label(Weapon.SubTypeName, GUILayout.Width(DefaultWidth));

                GUILayout.BeginVertical(GUILayout.Width(LargeWidth - 30));

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

                //GUILayout.Label(Weapon.DateCreated.ToString(), GUILayout.Width(LargeWidth));
                //GUILayout.Label(Weapon.DateModified.ToString(), GUILayout.Width(LargeWidth));
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            GUILayout.Space(15);
        }

  

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
