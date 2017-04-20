using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;
using System.IO;
using System.Reflection;

//EditorGUILayout.Toggle("Mute All Sounds", false);
//EditorGUILayout.IntField("Player Lifes", 3);

public class SpellEditorWindow : EditorWindow
{
    public Spell OriginalSpell;
    public Spell CurrentSpell;
    private Assembly SpellMethodsAssembly = typeof(UnitySpellMethods).Assembly;

    public Dictionary<string, MethodInfo> SpellMethods;
    private int SelectedSpellMethod;
    private List<string> SpellMethodNames;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 150;
    bool bIsChecked = false;

    public void NewSpell()
    {
        var DamageTypes = new List<DamageType>();
        DamageTypes.Add(new DamageType(EDamageType.Slashing, new int[] { 1, 8, 0 }));
    }

    public void SetActiveSpell(Spell Spell)
    {
        this.OriginalSpell = Spell;
        this.CurrentSpell = Spell;
        OnEnable();
        this.Repaint();
    }

    private void SetSpellMethodNames()
    {
        SelectedSpellMethod = 0;
        SpellMethods = SpellMethodsAssembly
                        .GetTypes()
                        .SelectMany(x => x.GetMethods())
                        .Where(y => y.GetCustomAttributes(true).OfType<SpellMethodAttribute>().Any())
                        .ToDictionary(z => z.Name);

        SpellMethodNames = SpellMethods.Keys.ToList<string>();
        SpellMethodNames.Insert(0, "None");

        for (int i = 0; i < SpellMethodNames.Count; i++)
        {
            if (SpellMethodNames[i] == CurrentSpell.SpellMethodName)
            {
                SelectedSpellMethod = i;
                break;
            }
        }
    }

    private void OnEnable()
    {
        SetSpellMethodNames();
    }

    private void OnDisable()
    {
       
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

            ((SpellListWindow)EditorWindow.GetWindow(typeof(SpellListWindow))).RefreshList();
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

        GUILayout.BeginHorizontal();
        GUILayout.Label("Level: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.Level = EditorGUILayout.IntField(CurrentSpell.Level, GUILayout.Width(SingleDigitWidth * 4));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell School: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.MagicSchool = (ESpellSchoolType)EditorGUILayout.Popup((int)CurrentSpell.MagicSchool, Enum.GetNames(typeof(ESpellSchoolType)));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Range Type: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.SpellRange.RangeType = (ESpellRangeType)EditorGUILayout.Popup((int)CurrentSpell.SpellRange.RangeType, Enum.GetNames(typeof(ESpellRangeType)));
        GUILayout.EndHorizontal();
    
        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Self Range Type: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.SpellRange.SelfRangeType = (ESpellSelfRangeType)EditorGUILayout.Popup((int)CurrentSpell.SpellRange.SelfRangeType, Enum.GetNames(typeof(ESpellSelfRangeType)));
        GUILayout.EndHorizontal();
       

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Range: ", GUILayout.Width(MaxLabelWidth));
        CurrentSpell.SpellRange.Range = EditorGUILayout.IntField(CurrentSpell.SpellRange.Range, GUILayout.Width(SingleDigitWidth * 4));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Method: ", GUILayout.Width(MaxLabelWidth));
        SelectedSpellMethod = EditorGUILayout.Popup(SelectedSpellMethod, SpellMethodNames.ToArray());
        CurrentSpell.SpellMethodName = SpellMethodNames[SelectedSpellMethod];
        CurrentSpell.SetSpellMethod<UnitySpellMethods>(CurrentSpell.SpellMethodName);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}

public class SpellListWindow : EditorWindow
{
    private Vector2 ScrollVector;
    private Assembly SpellMethodsAssembly = typeof(UnitySpellMethods).Assembly;

    private List<Spell> Items = new List<Spell>();
    private List<Spell> FilteredItems = new List<Spell>();
    private string DatabaseFile = Application.streamingAssetsPath + "/Data/Spells.bytes";

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;
    static SpellEditorWindow SpellEditor;
    static SpellListWindow ListWindow;

    public string SearchString = string.Empty;
    [MenuItem("D20/Spells")]
    public static void ShowWindow()
    {
        ListWindow = (SpellListWindow)EditorWindow.GetWindow(typeof(SpellListWindow));
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        SpellManager.Instance.FileName = DatabaseFile;
        SpellManager.Instance.Load();
        Items = SpellManager.Instance.Find<Spell>().OrderBy(x => x.Name).ToList();
        FilteredItems = Items;
    }

    void OnGUI()
    {
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

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
      
        SearchString = GUILayout.TextField(SearchString);

        if (!string.IsNullOrEmpty(SearchString))
        {
            FilteredItems = Items.Where(x => x.Name.ToLower().Contains(SearchString.ToLower())).ToList();
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
      
        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("School", EditorStyles.boldLabel, GUILayout.Width(LargeWidth));
        GUILayout.Label("Level", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
      //  GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(SmallWidth));
        GUILayout.EndHorizontal();

        // Column Headers
        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        foreach (var Spell in FilteredItems)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E", GUILayout.Width(IconWidth + 10)))
            {
                if (SpellEditor == null)
                {
                    var WindowTitle = string.Format("Editing: {0}", Spell.Name);

                    SpellEditor = EditorWindow.GetWindow<SpellEditorWindow>();
                    //SpellEditor = EditorWindow.GetWindow<SpellEditorWindow>(WindowTitle, typeof(SpellListWindow));

                }

                SpellEditor.titleContent.text = string.Format("Editing: {0}", Spell.Name);
                SpellEditor.SetActiveSpell(Spell);
            }

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Spell.Name, "OK", "Cancel"))
                {
                    SpellManager.Instance.DeleteItem(Spell.ID);
                    SpellManager.Instance.SaveChanges();
                }
            }

          
            GUILayout.Label(Spell.Name, GUILayout.Width(LargeWidth));
            GUILayout.Label(Spell.MagicSchool.ToString(), GUILayout.Width(LargeWidth));
            GUILayout.Label(Spell.Level.ToString(), GUILayout.Width(SmallWidth));
            //GUILayout.Label(Spell.ID.ToString(), GUILayout.Width(SmallWidth));
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
