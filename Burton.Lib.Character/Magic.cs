using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EMagicSchoolType
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

    public class SpellMaterial
    {
        public string Name;
        bool bConsumeOnUse;
    }

    public enum ESpellComponentType
    {
        Verbal,
        Somatic,
        Material
    }

    public class SpellComponent
    {
        public ESpellComponentType SpellComponentType;
        public List<SpellMaterial> Materials;
    }

    public enum ESpellRangeType
    {
        Touch,
        Self,
        Distance,
        Sight
    }

    public enum ESpellSelfRangeType
    {
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

        public SpellRange(ESpellRangeType RangeType, ESpellSelfRangeType SelfRangeType, int Range)
        {
            this.RangeType = RangeType;
            this.SelfRangeType = SelfRangeType;
            this.Range = Range;
        }

        public SpellRange()
        {
        }
    }

    [Serializable]
    public class Spell : DbItem
    {
        public EMagicSchoolType MagicSchool { get; set; }
        public List<EClassType> Classes;
        public SpellRange SpellRange;

        public int Level;
        public int CastingTime;
    
        public bool bConcentration;

        public string Description;

        public Spell(EMagicSchoolType MagicSchool, List<EClassType> ClassTypes, string Name, int Level, SpellRange Range, string Description)
        {
            this.MagicSchool = MagicSchool;
            this.Classes = ClassTypes;
            this.Name = Name;
            this.Level = Level;
            this.SpellRange = Range;
            this.Description = Description;
        }


        public Spell(Spell Other)
        {
            ID = Other.ID;
            MagicSchool = Other.MagicSchool;
            Classes = Other.Classes;
            Name = Other.Name;
            Level = Other.Level;
            Description = Other.Description;
            SpellRange = Other.SpellRange;
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
        public string FileName = "Spells.sdb";
        private SpellDB DB;
        private bool bDoBootstrap = false;

        public SpellManager()
        {
            DB = SpellDB.Instance;

            if (bDoBootstrap)
            {
                Bootstrap();
                SaveChanges();
                return;
            }

            Refresh();
        }

        public void Import(string FileName)
        {
            DB.Items.Clear();
            DB.ResetID();

            var Data = File.ReadAllLines(FileName);
            Data[0] = null;
            Data[1] = null;
            foreach (var Line in Data)
            {
                if (string.IsNullOrEmpty(Line))
                    continue;

                var Fields = Line.Split(new char[] { '\t' });

                if (Fields[57] == "")
                    continue;

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
                    Range.SelfRangeType = 0;
                }
                else if (Data_Range.Contains("sight"))
                {
                    Range.RangeType = ESpellRangeType.Sight;
                    Range.SelfRangeType = 0;
                }
                else if (Data_Range.Contains("special"))
                {
                    Range.SelfRangeType = 0;
                    Range.RangeType = 0;
                }
                else 
                {
                    Range.RangeType = ESpellRangeType.Distance;
                    Range.Range = Convert.ToInt32(Data_Range);
                    Range.SelfRangeType = 0;
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

                var School = (EMagicSchoolType) Enum.Parse(typeof(EMagicSchoolType), Data_School);
              
                var Spell = new Spell(School, ClassTypes, Data_Name, Data_Level, Range, "");
                AddItem<Spell>(Spell);

                Console.WriteLine(string.Format("{0,-30} {1,-5} {2,-20}", Fields[39], Fields[41], Fields[42]));
            }

            SaveChanges();
        }

        public void SaveChanges()
        {
            DB.Save(FileName);
        }

        public void Refresh()
        {
            DB.Load(FileName);
        }

        public int AddItem<T>(T Item) where T : DbItem
        {
            var NewItem = (Spell)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));

            NewItem.DateCreated = DateTime.Now;
            NewItem.DateModified = NewItem.DateCreated;

            return DB.Add((Spell)NewItem);
        }

        public void UpdateItem<T>(T Item) where T : DbItem
        {
            var Copy = (Spell)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));

            Copy.DateModified = DateTime.Now;

            DB.Items[Copy.ID - 1] = Copy;
        }

        public void DeleteItem(int ID)
        {
            DB.Items[ID - 1] = null;
        }

        // Get a copy of the Item by ID
        public T GetItemCopy<T>(int ID)
        {
            Spell Item = null;

            try
            {
                Item = DB.Get(ID);
            }
            catch (Exception Ex)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));
        }

        // Returns a list containing copies of the items in the ItemDB
        public List<Spell> GetItemsCopy()
        {
            List<Spell> Result = new List<Spell>();

            foreach (var Item in DB.Items.Where(x => x != null))
            {
                Result.Add(new Spell(Item));
            }

            return Result;
        }

        // Some defaults to play with
        public void Bootstrap()
        {
            DB.Items.Clear();
            DB.ResetID();
            AddBase();
            SaveChanges();
        }

        public void AddBase()
        {
           // AddItem<Spell>(new Spell(EMagicSchoolType.Necromancy, new List<EClassType>() { EClassType.Cleric, EClassType.Wizard }, "Raise Dead", 1, "Raising the dead!"));
        }
    }

    public class SpellDB : SimpleDB<Spell>
    {
        private static SpellDB _Instance;

        public static SpellDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SpellDB();

                return _Instance;
            }
        }
    }
}
