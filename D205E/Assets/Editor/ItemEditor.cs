using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;

public class ItemEditorWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Label("Item Editor", EditorStyles.boldLabel);
    }
}

public class ItemListWindow : EditorWindow
{
    private Vector2 ScrollVector;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;

    [MenuItem("D20/Items")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ItemListWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Equipment", EditorStyles.boldLabel);

        GUILayout.BeginVertical(GUI.skin.box);

        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        var Items = ItemManager.Instance
            .Find<Item>()
            .ToList();

        // Column Headers
        GUILayout.BeginHorizontal();
        GUILayout.Space(IconWidth + 7);

        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("SubType", EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Description", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Cost", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Weight", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));

        GUILayout.EndHorizontal();
        for (int i = 0; i < Items.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("E" ,GUILayout.Width(IconWidth)))
            {
                EditorWindow.GetWindow(typeof(ItemEditorWindow));
            }

            GUILayout.Label(Items[i].ID.ToString(), GUILayout.Width(SmallWidth));
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
