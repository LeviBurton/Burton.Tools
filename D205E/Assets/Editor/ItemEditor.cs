using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Burton.Lib.Characters;
using System.Linq;
using System.IO;
using Burton.Lib;
using System;

public class ItemManager
{
    #region Singleton
    private static ItemManager _Instance;
    public static ItemManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new ItemManager();

            return _Instance;
        }
    }
    #endregion

    static string AssetsBasePath = @"Assets/Resources/Data/Items";


    public List<Item> Items;

    public ItemManager()
    {
        Items = new List<Item>();

    }

    public IEnumerable<T> Find<T>(Func<T, bool> Predicate = null) where T : DbItem
    {
        if (Predicate == null)
        {
            return Items.OfType<T>().AsEnumerable();
        }
        else
        {
            return Items.OfType<T>().Where(Predicate).AsEnumerable();
        }
    }

    public void RefreshAssets()
    {
        Items = new List<Item>();

        var ItemGuids = AssetDatabase.FindAssets("t:Item", new string[] { AssetsBasePath });

        foreach (string ItemGuid in ItemGuids)
        {
            var AssetPath = AssetDatabase.GUIDToAssetPath(ItemGuid);
            var Asset = AssetDatabase.LoadAssetAtPath<Item>(AssetPath);
            Items.Add(Asset);
        }
    }

    public void Delete(string ItemType)
    {
        var ItemGuids = AssetDatabase.FindAssets(string.Format("t:{0}", ItemType), new string[] { AssetsBasePath });
        foreach (string ItemGuid in ItemGuids)
        {
            var AssetPath = AssetDatabase.GUIDToAssetPath(ItemGuid);
            AssetDatabase.DeleteAsset(AssetPath);
        }
    }

    public void DeleteAll()
    {
        // Delete all assets
        var ItemGuids = AssetDatabase.FindAssets("t:Item", new string[] { AssetsBasePath });
        foreach (string ItemGuid in ItemGuids)
        {
            var AssetPath = AssetDatabase.GUIDToAssetPath(ItemGuid);
            AssetDatabase.DeleteAsset(AssetPath);
        }
    }

    public void SaveAsset<T>(T Asset) where T : Item
    {
        var AssetPath = AssetDatabase.GetAssetPath(Asset);
        if (string.IsNullOrEmpty(AssetPath))
        {
            AssetPath = AssetsBasePath + string.Format(@"/{0}.asset", Asset.Name.Replace(" ", "_"));
            AssetDatabase.CreateAsset(Asset, AssetPath);
        }

        var AssetFileName = Path.GetFileNameWithoutExtension(AssetPath);

        if (Asset.Name != AssetFileName.Replace("_", " "))
        {
            var NewFileName = "/" + Asset.Name.Replace(" ", "_");
            AssetDatabase.RenameAsset(AssetPath, NewFileName);
        }

        AssetDatabase.SaveAssets();
    }

    // Should handle all item types
    public T CreateAsset<T>(string Name, bool bOnlyCreateInstance = false) where T : Item
    {
        var AssetPath = AssetsBasePath + string.Format(@"/{0}.asset", Name.Replace(" ", "_"));
        T ItemAsset = ScriptableObject.CreateInstance<T>();
        if (!bOnlyCreateInstance)
        {
            AssetDatabase.CreateAsset(ItemAsset, AssetPath);
        }
        return ItemAsset;
    }

    public void Import(string FileName)
    {

    }


    public void AddBaseArmors()
    {
        //// Just create some base game armor types that will always be around.
        //var Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Padded", "Padded Armor", 5, 8, new List<Ability>());
        //AddItem<Armor>(Armor);

        //Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 11, "Leather", "Leather Armor", 11, 10, new List<Ability>());
        //AddItem<Armor>(Armor);

        //Armor = new Armor(EItemSubType.Light, EItemRarity.Common, EAbility.Dexterity, 12, "Studded Leather", "Studded Leather Armor", 45, 13, new List<Ability>());
        //AddItem<Armor>(Armor);

        //Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 12, "Hide", "Hide Armor", 10, 12, new List<Ability>());
        //AddItem<Armor>(Armor);

        //Armor = new Armor(EItemSubType.Medium, EItemRarity.Uncommon, EAbility.Dexterity, 13, "Chain Shirt", "Chain Shirt Armor", 50, 20, new List<Ability>());
        //AddItem<Armor>(Armor);

        //Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 14, "Ring Mail", "Ring Mail Armor", 14, 40, new List<Ability>());
        //AddItem<Armor>(Armor);

        //List<Ability> Requirements = new List<Ability>();
        //Requirements.Add(new Ability(EAbility.Strength, 0, 0, 13));
        //Armor = new Armor(EItemSubType.Heavy, EItemRarity.Rare, 0, 16, "Chain Mail", "Chain Mail Armor", 75, 55, Requirements);

        //AddItem<Armor>(Armor);
    }
}
public class ItemEditorWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Label("Item Editor", EditorStyles.boldLabel);
    }
}

