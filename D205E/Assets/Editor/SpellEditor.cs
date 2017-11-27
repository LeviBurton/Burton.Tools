using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System;
using System.IO;
using System.Reflection;
using Burton.Lib;

public class SpellManager
{
    #region Singleton
    private static SpellManager _Instance;
    public static SpellManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new SpellManager();

            return _Instance;
        }
    }
    #endregion

    static string SpellAssetsBasePath = @"Assets/Resources/Data/Spells";
    public List<Spell> Spells;

    public SpellManager()
    {
        Spells = new List<Spell>();
        RefreshAssets();
    }

    public IEnumerable<T> Find<T>(Func<T, bool> Predicate = null) where T : DbItem
    {
        if (Predicate == null)
        {
            return Spells.OfType<T>().AsEnumerable();
        }
        else
        {
            return Spells.OfType<T>().Where(Predicate).AsEnumerable();
        }
    }

    public void RefreshAssets()
    {
        Spells = new List<Spell>();

        var SpellGUIDs = AssetDatabase.FindAssets("t:Spell", new string[] { SpellAssetsBasePath });

        foreach (string SpellGuid in SpellGUIDs)
        {
            var SpellAssetPath = AssetDatabase.GUIDToAssetPath(SpellGuid);
            var Spell = AssetDatabase.LoadAssetAtPath<Spell>(SpellAssetPath);
            Spell.BindSpellMethod<Spell>(Spell.SpellMethodName);

            Spells.Add(Spell);
        }
    }

    public void DeleteAll()
    {
        // Delete all assets
        var SpellGUIDs = AssetDatabase.FindAssets("t:Spell", new string[] { SpellAssetsBasePath });
        foreach (string SpellGuid in SpellGUIDs)
        {
            var SpellAssetPath = AssetDatabase.GUIDToAssetPath(SpellGuid);
            AssetDatabase.DeleteAsset(SpellAssetPath);
        }
    }

    public void Import(string FileName)
    {
        DeleteAll();

        var Data = File.ReadAllLines(FileName);
        Data[0] = null;
        Data[1] = null;
        foreach (var Line in Data)
        {
            if (string.IsNullOrEmpty(Line))
                continue;

            var Fields = Line.Split(new char[] { '\t' });
            //   continue;

            if (Fields[57] == "")
                continue;

            // v 47
            // s 48
            // m 49
            // 51 conc
            // 52 dur

            var Data_V = Fields[47];
            var Data_S = Fields[48];
            var Data_M = Fields[49];

            var Data_Conc = Fields[51];
            var Data_Dur = Fields[52];

            var Data_School = Fields[42];
            var Data_Name = Fields[39];
            var Data_Level = Convert.ToInt32(Fields[41]);
            var Data_Range = Fields[46];
            var Data_Duration = Fields[52];
            var Data_CastingTime = Fields[45];

            var Data_Bard = Fields[0];
            var Data_Cleric = Fields[1];
            var Data_Paladin = Fields[21];
            var Data_Ranger = Fields[27];
            var Data_Sorc = Fields[29];
            var Data_Warlock = Fields[31];
            var Data_Wizard = Fields[38];

            var Range = new SpellRange();
            var SrcRange = Data_Range.ToString();

            Data_Range = Data_Range.Replace("(", "").Replace(")", "").Replace("-foot", "").Replace("-feet", "").Replace("sphere", "").Replace("feet", "").Replace("mile", "").ToLower();

            if (Data_Range.Contains("self"))
            {
                Data_Range = Data_Range.Replace("self", "");

                Range.RangeType = ESpellRangeType.Self;

                foreach (var RangeType in Enum.GetValues(typeof(ESpellSelfRangeType)))
                {
                    if (Data_Range.Contains(RangeType.ToString().ToLower()))
                    {
                        Range.SelfRangeType = (ESpellSelfRangeType)RangeType;
                        Data_Range = Data_Range.Replace(Range.SelfRangeType.ToString().ToLower(), "").Trim();
                        Range.Range = Convert.ToInt32(Data_Range);

                        break;
                    }
                }
            }
            else if (Data_Range.Contains("touch"))
            {
                Range.RangeType = ESpellRangeType.Touch;
                Range.SelfRangeType = ESpellSelfRangeType.None;
            }
            else if (Data_Range.Contains("sight"))
            {
                Range.RangeType = ESpellRangeType.Sight;
                Range.SelfRangeType = ESpellSelfRangeType.None;
            }
            else if (Data_Range.Contains("special"))
            {
                Range.SelfRangeType = ESpellSelfRangeType.None;
                Range.RangeType = ESpellRangeType.None;
            }
            else
            {
                Range.RangeType = ESpellRangeType.Distance;
                Range.Range = Convert.ToInt32(Data_Range);
                Range.SelfRangeType = ESpellSelfRangeType.None;
            }

            List<EClassType> ClassTypes = new List<EClassType>();

            if (Data_Bard != "")
                ClassTypes.Add(EClassType.Bard);

            if (Data_Cleric != "")
                ClassTypes.Add(EClassType.Cleric);

            if (Data_Paladin != "")
                ClassTypes.Add(EClassType.Paladin);

            if (Data_Ranger != "")
                ClassTypes.Add(EClassType.Ranger);

            if (Data_Sorc != "")
                ClassTypes.Add(EClassType.Sorcerer);

            if (Data_Warlock != "")
                ClassTypes.Add(EClassType.Warlock);

            if (Data_Wizard != "")
                ClassTypes.Add(EClassType.Wizard);

            var School = (ESpellSchoolType)Enum.Parse(typeof(ESpellSchoolType), Data_School);

            var Conc = Data_Conc == "1";

            List<ECastingComponentType> CastingComps = new List<ECastingComponentType>();

            if (Data_V == "1")
            {
                CastingComps.Add(ECastingComponentType.Verbal);
            }

            if (Data_S == "1")
            {
                CastingComps.Add(ECastingComponentType.Somatic);
            }

            var SpellMaterials = new List<SpellMaterial>();

            if (Data_M == "1")
            {
                CastingComps.Add(ECastingComponentType.Material);

                //SpellMaterials = ItemManager.Instance.Find<SpellMaterial>(x => x.SubType == EItemSubType.Spell_Material && x.Name == Data_Name).ToList();
            }


            var s = new Spell(School, ClassTypes, Data_Name, Data_Level, Range, "", CastingComps, SpellMaterials, Conc, Data_Duration);

            Add(s);

            Console.WriteLine(string.Format("{0,-30} {1,-5} {2,-20}", Fields[39], Fields[41], Fields[42]));
        }

        AssetDatabase.SaveAssets();
    }

    void Add(Spell Spell)
    {
        var AssetPath = SpellAssetsBasePath + string.Format(@"/{0}.asset", Spell.Name.Replace(" ", "_"));

        Spell SpellAsset = ScriptableObject.CreateInstance<Spell>();
        SpellAsset.Name = Spell.Name;
        SpellAsset.MagicSchool = Spell.MagicSchool;
        SpellAsset.Level = Spell.Level;
        SpellAsset.ID = Spell.ID;
        SpellAsset.Classes = Spell.Classes;
        SpellAsset.CastingTime = Spell.CastingTime;
        SpellAsset.CastingComponentTypes = Spell.CastingComponentTypes;
        SpellAsset.bConcentration = Spell.bConcentration;
        SpellAsset.SpellMaterials = Spell.SpellMaterials;
        SpellAsset.SpellMethodInfo = Spell.SpellMethodInfo;
        SpellAsset.SpellMethodName = Spell.SpellMethodName;
        SpellAsset.SpellRange = Spell.SpellRange;

        AssetDatabase.CreateAsset(SpellAsset, AssetPath);
        AssetDatabase.SaveAssets();
    }
}

public class SpellEditorWindow : EditorWindow
{
    public SpellListWindow SpellListWindow;

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
        SpellListWindow = EditorWindow.GetWindow<SpellListWindow>();
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
            CurrentSpell.DateModified = DateTime.Now;
            EditorUtility.SetDirty(CurrentSpell);

            AssetDatabase.SaveAssets();

            var CurrentPath = AssetDatabase.GetAssetPath(CurrentSpell);
            var CurrentFileName = Path.GetFileNameWithoutExtension(CurrentPath);
    
            if (CurrentSpell.Name != CurrentFileName.Replace("_", " "))
            {
                var NewFileName = "/" + CurrentSpell.Name.Replace(" ", "_") ;
                Debug.Log(AssetDatabase.RenameAsset(CurrentPath, NewFileName));
            }

            if (SpellListWindow != null)
            {
                SpellListWindow.RefreshList();
                SpellListWindow.Repaint();
            }
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
        CurrentSpell.BindSpellMethod<UnitySpellMethods>(CurrentSpell.SpellMethodName);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
public class SpellListWindow : EditorWindow
{
    static string SpellAssetsBasePath = @"Assets/Resources/Data/Spells";
    private Assembly SpellMethodsAssembly = typeof(UnitySpellMethods).Assembly;
    private Vector2 ScrollVector;
    private List<Spell> Spells = new List<Spell>();
    private List<Spell> FilteredSpells = new List<Spell>();
    private string DatabaseFile = string.Empty;
    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;

    public SpellEditorWindow SpellEditor;
    public SpellListWindow ListWindow;

    public string SearchString = string.Empty;

    [MenuItem("D20/Spells")]
    static void CreateWindow()
    {
        EditorWindow.GetWindow<SpellListWindow>();
    }

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        SpellManager.Instance.RefreshAssets();
        Spells = SpellManager.Instance.Find<Spell>().ToList();
        foreach (var Spell in Spells)
        {
            Spell.BindSpellMethod<UnitySpellMethods>(Spell.SpellMethodName);
        }

        FilteredSpells = Spells;

    }

    void Import()
    {
        string path = EditorUtility.OpenFilePanel("Import file", "", "*");

        if (path.Length != 0)
        {
            SpellManager.Instance.Import(path);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("New Spell"))
        {
            if (SpellEditor == null)
            {
                SpellEditor = EditorWindow.GetWindow<SpellEditorWindow>();
            }

            SpellEditor.titleContent.text = "New Spell";
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        SearchString = GUILayout.TextField(SearchString);

        if (!string.IsNullOrEmpty(SearchString))
        {
            FilteredSpells = Spells.Where(x => x.Name.ToLower().Contains(SearchString.ToLower())).ToList();
        }
        else
        {
            FilteredSpells = Spells;
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

        foreach (var Spell in FilteredSpells)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E", GUILayout.Width(IconWidth + 10)))
            {
                if (SpellEditor == null)
                {
                    var WindowTitle = string.Format("Editing: {0}", Spell.Name);

                    SpellEditor = EditorWindow.GetWindow<SpellEditorWindow>();
                }

                SpellEditor.titleContent.text = string.Format("Editing: {0}", Spell.Name);
                SpellEditor.SetActiveSpell(Spell);
            }

            if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
            {
                if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Spell.Name, "OK", "Cancel"))
                {
                    var SpellAssetPath = AssetDatabase.GetAssetPath(Spell);
                    AssetDatabase.DeleteAsset(SpellAssetPath);
                    RefreshList();
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
