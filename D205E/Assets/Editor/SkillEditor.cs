using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters.Skills;
using System;
using Burton.Lib.Characters;

public class SkillEditorWindow : EditorWindow
{
    public Skill OriginalSkill;
    public Skill CurrentSkill;

    public void NewSkill()
    {
        var NewSkill = new Skill();
        CurrentSkill = NewSkill;
        OriginalSkill = NewSkill;

    }
    void OnGUI()
    {

    }
}

public class SkillListWindow : EditorWindow
{
    private Vector2 ScrollVector;
    public static SkillListWindow Window;

    int DefaultWidth = 100;
    int SmallWidth = 50;
    int IconWidth = 16;
    int LargeWidth = 200;
    SkillEditorWindow Editor;
    string DatabaseFile;
    private List<Skill> Skills;

    [MenuItem("D20/Skills")]
    public static void ShowWindow()
    {
        Window = (SkillListWindow) EditorWindow.GetWindow(typeof(SkillListWindow));
    }

    private void OnDestroy()
    {

    }

    private void OnEnable()
    {
        DatabaseFile = Application.streamingAssetsPath + "/Data/Skills.bytes";
        SkillManager.Instance.FileName = DatabaseFile;
        SkillManager.Instance.Load();
        RefreshList();
    }

    private void RefreshList()
    {
        Skills = SkillManager.Instance.Find<Skill>().ToList();
    }

    void OnGUI()
    {

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Skill"))
        {
            if (Editor == null)
            {
                Editor = (SkillEditorWindow)EditorWindow.GetWindow(typeof(SkillEditorWindow));
            }

            Editor.titleContent.text = "New Skill";
        }

        if (GUILayout.Button("Reset All"))
        {
            SkillManager.Instance.Bootstrap();
            RefreshList();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical(GUI.skin.box);
        // Column Headers
        ScrollVector = GUILayout.BeginScrollView(ScrollVector);
       
        foreach (var Ability in Enum.GetNames(typeof(EAbility)))
        {
            GUILayout.Label(Ability, EditorStyles.boldLabel, GUILayout.Width(LargeWidth));

            foreach (var Skill in Skills.Where(x => x.Ability == (EAbility)Enum.Parse(typeof(EAbility), Ability)))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(IconWidth);
                if (GUILayout.Button("D", GUILayout.Width(IconWidth + 10)))
                {
                    if (EditorUtility.DisplayDialog("Confirm delete", "Delete " + Skill.Name, "OK", "Cancel"))
                    {
                        SkillManager.Instance.DeleteItem(Skill.ID);
                        SkillManager.Instance.SaveChanges(DatabaseFile);
                        RefreshList();
                    }
                }
                GUILayout.Label(Skill.Name, EditorStyles.label, GUILayout.Width(DefaultWidth));
                GUILayout.Label(Skill.Description, EditorStyles.boldLabel, GUILayout.Width(DefaultWidth));
                GUILayout.EndHorizontal();
            }

        }
        
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
