using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System.Reflection;

public class ItemListWindow : EditorWindow
{
    static string SpellAssetsBasePath = @"Assets/Resources/Data/Items";
    private Assembly SpellMethodsAssembly = typeof(UnitySpellMethods).Assembly;
    private Vector2 ScrollVector;
    private List<Item> Items;
    private List<Item> FilteredItems;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;

    public string SearchString = string.Empty;

    [MenuItem("D20/Items")]
    public static void CreateWindow()
    {
        EditorWindow.GetWindow<ItemListWindow>();
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        ItemManager.Instance.RefreshAssets();
        Items = ItemManager.Instance.Find<Item>().ToList();
        FilteredItems = Items;
    }

    void OnGUI()
    {
        DrawItemList();
    }

    void DrawItemList()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Item"))
        {
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Base Items"))
        {
            ItemManager.Instance.DeleteAll();
            //ItemManager2.Instance.AddBaseArmors();
          //  ItemManager.Instance.AddBaseWeapons();
            RefreshList();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        SearchString = GUILayout.TextField(SearchString);
        if (!string.IsNullOrEmpty(SearchString))
        {
            FilteredItems = FilteredItems.Where(x => x.Name.ToLower().Contains(SearchString.ToLower())).ToList();
        }
        else
        {
            FilteredItems = Items;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Space(IconWidth + 15);
        GUILayout.Space(IconWidth + 15);

        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("SubType", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Description", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Cost", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Weight", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.EndHorizontal();

        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        for (int i = 0; i < Items.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("E", GUILayout.Width(IconWidth + 10)))
            {
                EditorWindow.GetWindow(typeof(ItemEditorWindow));
            }

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                //if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Spell.Name, "OK", "Cancel"))
                //{
                //    var SpellAssetPath = AssetDatabase.GetAssetPath(Spell);
                //    AssetDatabase.DeleteAsset(SpellAssetPath);
                //    RefreshList();
                //}
            }

            GUILayout.Label(Items[i].TypeName, GUILayout.Width(DefaultWidth));
            GUILayout.Label(Items[i].SubTypeName, GUILayout.Width(DefaultWidth));
            GUILayout.Label(Items[i].Name, EditorStyles.wordWrappedLabel, GUILayout.Width(LargeWidth));
            GUILayout.Label(Items[i].Description, EditorStyles.wordWrappedLabel, GUILayout.Width(LargeWidth));
            GUILayout.Label(Items[i].Cost.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.Label(Items[i].Weight.ToString(), GUILayout.Width(SmallWidth));

            GUILayout.EndHorizontal();

        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
