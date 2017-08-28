using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Burton.Lib.Characters
{
    public enum ESpellSchoolType
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    }

    [Serializable]
    public class SpellMaterial : Item
    {
        bool bConsumeOnUse = false;

        public SpellMaterial(string Name, string Description, int Cost, bool ConsumeOnUse)
            :base(EItemType.Special_Material, EItemSubType.Spell_Material, EItemRarity.Common, Name, Description, 0, Cost, 0, null)
        {
            this.bConsumeOnUse = ConsumeOnUse;
        }

        public override DbItem Clone()
        {
            return (DbItem)this.MemberwiseClone();
        }
    }

    public enum ECastingComponentType
    {
        None,
        Verbal,
        Somatic,
        Material
    }
  

    public enum ESpellRangeType
    {
        None,
        Touch,
        Self,
        Distance,
        Sight
    }

    public enum ESpellSelfRangeType
    {
        None,
        Radius,
        Line,
        Sphere,
        Cone,
        Cube
    }

    [Serializable]
    public class SpellRange
    {
        public ESpellRangeType RangeType;
        public ESpellSelfRangeType SelfRangeType;
        public int Range;

        public SpellRange() {}

        public SpellRange(ESpellRangeType RangeType, ESpellSelfRangeType SelfRangeType, int Range)
        {
            this.RangeType = RangeType;
            this.SelfRangeType = SelfRangeType;
            this.Range = Range;
        }

        public SpellRange(SpellRange Other)
        {
            this.Range = Other.Range;
            this.SelfRangeType = Other.SelfRangeType;
            this.RangeType = Other.RangeType;
        }

        public SpellRange Clone()
        {
            return (SpellRange)this.MemberwiseClone();
        }
    }


    [CreateAssetMenu(fileName = "Spell", menuName = "Spells", order = 1)]
    public class Spell : DbItem
    {
        private Action<Spell, object> CastDelegate = null;

        public ESpellSchoolType MagicSchool;
        public int Level;
        public int CastingTime;
        public bool bConcentration;
        public string Description;

        public List<EClassType> Classes;
        public List<ECastingComponentType> CastingComponentTypes;
        public List<SpellMaterial> SpellMaterials;

        public SpellRange SpellRange;
        public string SpellMethodName = string.Empty;
        public MethodInfo SpellMethodInfo = null;

 
        public Spell(ESpellSchoolType MagicSchool, 
                     List<EClassType> ClassTypes, 
                     string Name, 
                     int Level, 
                     SpellRange Range, 
                     string Description, 
                     List<ECastingComponentType> SpellCastingComponentType,
                     List<SpellMaterial> SpellMaterials,
                     bool bConcentration,
                     string Duration)
        {
            this.MagicSchool = MagicSchool;
            this.Classes = ClassTypes;
            this.Name = Name;
            this.Level = Level;
            this.SpellRange = Range;
            this.Description = Description;
            this.CastingComponentTypes = SpellCastingComponentType;
            this.bConcentration = bConcentration;
            this.SpellMaterials = SpellMaterials;
        }

        public Spell(Spell Other)
        {
            this.bConcentration = Other.bConcentration;
            this.CastDelegate = Other.CastDelegate;
            this.CastingComponentTypes = Other.CastingComponentTypes;
            this.CastingTime = Other.CastingTime;
            this.Classes = Other.Classes;
            this.DateCreated = Other.DateCreated;
            this.DateModified = Other.DateModified;
            this.ID = Other.ID;
            this.Level = Other.Level;
            this.MagicSchool = Other.MagicSchool;
            this.Name = Other.Name;
            this.SpellMaterials = Other.SpellMaterials;
            this.SpellMethodInfo = Other.SpellMethodInfo;
            this.SpellMethodName = Other.SpellMethodName;
            this.SpellRange = Other.SpellRange;
        }

        #region Unity ScriptableObject
        public void OnEnable()
        {
        }

        public void OnDestroy()
        {

        }

        public void OnDisable()
        {

        }
        #endregion region

        public override DbItem Clone()
        {
            var Other = (Spell)this.MemberwiseClone();
            Other.Classes = new List<EClassType>(this.Classes);
            Other.SpellRange = new SpellRange(this.SpellRange);
            Other.CastingComponentTypes = new List<ECastingComponentType>();
            Other.SpellMaterials = new List<SpellMaterial>();
            
            foreach (var m in this.SpellMaterials)
            {
                Other.SpellMaterials.Add(m);
            }

            foreach (var c in this.CastingComponentTypes)
            {
                Other.CastingComponentTypes.Add(c);
            }

            return (DbItem) Other;
        }

        // Unity scriptable object seems to take care of 
        // serializing our delegate!  This means 
        // we can bind, then save.
        public void BindSpellMethod<T>(string MethodName)
        {
            SpellMethodName = MethodName;

            SpellMethodInfo = typeof(T).Assembly
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttributes(true).OfType<SpellMethodAttribute>().Any())
                .Where(x => x.Name == MethodName).SingleOrDefault();

            if (SpellMethodInfo == null)
                return;

            CastDelegate = (Action<Spell,object>)Delegate.CreateDelegate(typeof(Action<Spell, object>), SpellMethodInfo);
        }

        public void Cast(object Caster)
        {
            if (CastDelegate == null)
                return;

            CastDelegate(this, Caster);
        }
    }

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

        static string SpellAssetsBasePath = @"Assets/Data/Spells";
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

}
