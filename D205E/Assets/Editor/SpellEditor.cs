using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;

//EditorGUILayout.Toggle("Mute All Sounds", false);
//EditorGUILayout.IntField("Player Lifes", 3);

public class SpellEditorWindow : EditorWindow
{
    public Spell OriginalSpell;
    public Spell CurrentSpell;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;
    bool bIsChecked = false;

    public void NewSpell()
    {
        var DamageTypes = new List<DamageType>();
        DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 }));

        //var Spell = new Spell(EMagicSchoolType.Abjuration, new List<EClassType>(), "New Spell", 0, new SpellRange(), ""); 

        //CurrentSpell = Spell;
        //OriginalSpell = Spell;
    }

    public void SetWeapom(Spell Spell)
    {
        this.OriginalSpell = Spell;
        this.CurrentSpell = Spell;
        this.Repaint();
    }

    void OnGUI()
    {
        int MaxLabelWidth = 150;
        int SingleDigitWidth = 25;

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            if (CurrentSpell.ID <= 0)
            {
                CurrentSpell.ID = SpellManager.Instance.AddItem<Spell>(CurrentSpell);
            }
            else
            {
                SpellManager.Instance.UpdateItem<Spell>(this.CurrentSpell);
            }

            SpellManager.Instance.SaveChanges();

            EditorWindow.GetWindow(typeof(SpellListWindow)).Repaint();
        }

        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.Name = GUILayout.TextField(CurrentSpell.Name);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}

public class SpellListWindow : EditorWindow
{
    private Vector2 ScrollVector;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;
    SpellEditorWindow SpellEditor;

    [MenuItem("D20/Spells")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpellListWindow));
    }

    //[MenuItem("D20/Import Spells")]
    //public static void ShowWindow()
    //{
       
    //        //SpellManager.Instance.Import("Spells.tsv.txt");
    //        //SpellManager.Instance.SaveChanges();

     
    //}
    void OnGUI()
    {

      
        var Items = SpellManager.Instance.Find<Spell>().OrderBy(x => x.MagicSchool).ToList();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Spell", GUILayout.Width(DefaultWidth)))
        {
            if (SpellEditor == null)
            {
                SpellEditor = (SpellEditorWindow)EditorWindow.GetWindow(typeof(SpellEditorWindow));
            }

            SpellEditor.titleContent.text = "New Spell";
            SpellEditor.NewSpell();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical(GUI.skin.box);
        
        // Column Headers
        ScrollVector = GUILayout.BeginScrollView(ScrollVector);
        GUILayout.BeginHorizontal();
        GUILayout.Space(IconWidth + 15);
        GUILayout.Space(IconWidth + 15);
        GUILayout.Label("Level", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("School", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
     
        GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.EndHorizontal();

        foreach (var Spell in Items)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E", GUILayout.Width(IconWidth + 10)))
            {
                if (SpellEditor == null)
                {
                    SpellEditor = (SpellEditorWindow)EditorWindow.GetWindow(typeof(SpellEditorWindow));
                }

                SpellEditor.titleContent.text = string.Format("Editing: {0}", Spell.Name);
                SpellEditor.SetWeapom(Spell);
            }

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Spell.Name, "OK", "Cancel"))
                {
                    SpellManager.Instance.DeleteItem(Spell.ID);
                    SpellManager.Instance.SaveChanges();
                }
            }

            GUILayout.Label(Spell.Level.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.Label(Spell.Name, GUILayout.Width(LargeWidth));
            GUILayout.Label(Spell.MagicSchool.ToString(), GUILayout.Width(LargeWidth));
         
            GUILayout.Label(Spell.ID.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
