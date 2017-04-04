using System;
using System.Collections.Generic;
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

    [Serializable]
    public class Spell : DbItem
    {
        public EMagicSchoolType MagciSchoolType { get; set; }
        public EClassType ClassType { get; set; }

        public int Level;
        public int CastingTime;
        public int Range;

        public string Description;

        public Spell(EMagicSchoolType MagicSchool, EClassType ClassType, string Name, int Level, string Description)
        {
            this.MagciSchoolType = MagicSchool;
            this.ClassType = ClassType;
            this.Name = Name;
            this.Level = Level;
            this.Description = Description;
        }


        public Spell(Spell Other)
        {
            MagciSchoolType = Other.MagciSchoolType;
            ClassType = Other.ClassType;
            Name = Other.Name;
            Level = Other.Level;
            Description = Other.Description;
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
            DB = new SpellDB();

            if (bDoBootstrap)
            {
                Bootstrap();
                SaveChanges();
                return;
            }

            Refresh();
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
            AddItem<Spell>(new Spell(EMagicSchoolType.Necromancy, EClassType.Cleric, "Raise Dead", 1, "Raising the dead!"));


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
