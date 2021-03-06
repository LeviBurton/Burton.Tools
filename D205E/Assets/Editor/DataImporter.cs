﻿using Assets.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using Burton.Lib.Characters;
using Burton.Lib;

public interface IDataImporter
{
    void Import();
    void Import(string FileName);
    void ReImport();
    void ReImport(string FileName);
}

public class DataImporter : IDataImporter
{
    public string ImporterName;
    public int ItemCount;

    public virtual void Import()
    {
        throw new NotImplementedException();
    }

    public virtual void Import(string FileName)
    {
        throw new NotImplementedException();
    }

    public virtual void ReImport()
    {
        throw new NotImplementedException();
    }

    public virtual void ReImport(string FileName)
    {
        throw new NotImplementedException();
    }
}

public class SpellImporter : DataImporter
{
    public SpellImporter()
    {
        ImporterName = "Spells";

        SpellManager.Instance.RefreshAssets();
        ItemCount = SpellManager.Instance.Spells.Count();
    }

    public override void Import(string FileName)
    {
        throw new NotImplementedException();
    }

    public override void Import()
    {
        Debug.Log(string.Format("{0} Import", ImporterName));
    }

    public override void ReImport()
    {
        throw new NotImplementedException();
    }

    public override void ReImport(string FileName)
    {
        throw new NotImplementedException();
    }
}

public class WeaponImporter : DataImporter, IDataImporter
{
    public string PrefabPath = @"";

    public WeaponImporter()
    {
        ImporterName = "Weapons";
        ItemManager.Instance.RefreshAssets();
        ItemCount = ItemManager.Instance.Items.Where(x => x.Type == EItemType.Weapon).Count();
    }

    public override void Import(string FileName)
    {
        WeaponManager.Instance.Import(FileName);
    }

    public override void Import()
    {
        throw new NotImplementedException();
    }

    public override void ReImport()
    {
        throw new NotImplementedException();
    }

    public override void ReImport(string FileName)
    {
        throw new NotImplementedException();
    }
}


public class DataImporterMainWindow : EditorWindow
{
    private Vector2 ScrollVector;
    private List<DataImporter> Importers = new List<DataImporter>();

    [MenuItem("D20/Data/Importer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DataImporterMainWindow));
    }

    public void OnEnable()
    {
        if (!Importers.Any())
        {
            Importers.Add(new WeaponImporter());
            Importers.Add(new SpellImporter());
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Data Type", EditorStyles.boldLabel, GUILayout.Width(UI_Constants.DefaultWidth));
        GUILayout.Label("Item Count", EditorStyles.boldLabel, GUILayout.Width(UI_Constants.DefaultWidth));
        GUILayout.Space(UI_Constants.DefaultWidth);
        GUILayout.Space(UI_Constants.DefaultWidth);
        GUILayout.EndHorizontal();

        ScrollVector = GUILayout.BeginScrollView(ScrollVector);

        foreach (var Importer in Importers)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Importer.ImporterName, GUILayout.Width(UI_Constants.DefaultWidth));
            GUILayout.Label(Importer.ItemCount.ToString(), GUILayout.Width(UI_Constants.DefaultWidth));
            GUILayout.Space(25);

            if (GUILayout.Button("Import", GUILayout.Width(UI_Constants.DefaultWidth + 10)))
            {
                string ImportFile = EditorUtility.OpenFilePanel("Choose data file...", "", "*.*");
                if (ImportFile.Length != 0)
                {
                    Importer.Import(ImportFile);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
