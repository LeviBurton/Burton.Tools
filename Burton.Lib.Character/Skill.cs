using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters.Skills
{
    [Serializable]
    public class Skill : DbItem
    {
        public EAbility Ability { get; set; }
        public string Description { get; set; }

        public Skill()
        {     
        }

        public Skill(Skill Other)
        {
            this.ID = Other.ID;
            this.Name = Other.Name;
            this.Ability = Other.Ability;
            this.Description = Other.Description;
            this.DateCreated = Other.DateCreated;
            this.DateModified = Other.DateModified;
        }

        public Skill(EAbility AbilityModifier, string Name, string Description = null)
        {
            this.Name = Name;
            this.Description = Description;
            this.Ability = AbilityModifier;
        }
    }

    public class SkillManager
    {
        #region Singleton
        private static SkillManager _Instance;
        public static SkillManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SkillManager();

                return _Instance;
            }
        }
        #endregion

        public string FileName = "Skills.sdb";
        private SkillsDB DB;
        private bool bDoBootstrap = false;

        public SkillManager()
        {
            DB = SkillsDB.Instance;

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
            var NewItem = (Skill)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));

            NewItem.DateCreated = DateTime.Now;
            NewItem.DateModified = NewItem.DateCreated;

            return DB.Add((Skill)NewItem);
        }

        public void UpdateItem<T>(T Item) where T : DbItem
        {
            var Copy = (Skill)Activator.CreateInstance(typeof(T), Convert.ChangeType(Item, typeof(T)));

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
            Skill Item = null;

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
        public List<Skill> GetItemsCopy()
        {
            List<Skill> Result = new List<Skill>();

            foreach (var Item in DB.Items.Where(x => x != null))
            {
                Result.Add(new Skill(Item));
            }

            return Result;
        }

        // Some defaults to play with
        public void Bootstrap()
        {
            DB.Items.Clear();
            DB.ResetID();
            AddBaseSkills();
            SaveChanges();
        }

        public void AddBaseSkills()
        {
            AddItem<Skill>(new Skill(EAbility.Strength,     "Athletics"));
            AddItem<Skill>(new Skill(EAbility.Dexterity,    "Acrobatics"));
            AddItem<Skill>(new Skill(EAbility.Dexterity,    "Sleight of Hand"));
            AddItem<Skill>(new Skill(EAbility.Dexterity,    "Stealth"));
            AddItem<Skill>(new Skill(EAbility.Intelligence, "Arcana"));
            AddItem<Skill>(new Skill(EAbility.Intelligence, "History"));
            AddItem<Skill>(new Skill(EAbility.Intelligence, "Investigation"));
            AddItem<Skill>(new Skill(EAbility.Intelligence, "Nature"));
            AddItem<Skill>(new Skill(EAbility.Intelligence, "Religion"));
            AddItem<Skill>(new Skill(EAbility.Wisdom,       "Animal Handling"));
            AddItem<Skill>(new Skill(EAbility.Wisdom,       "Insight"));
            AddItem<Skill>(new Skill(EAbility.Wisdom,       "Medicine"));
            AddItem<Skill>(new Skill(EAbility.Wisdom,       "Perception"));
            AddItem<Skill>(new Skill(EAbility.Wisdom,       "Survival"));
            AddItem<Skill>(new Skill(EAbility.Charisma,     "Deception"));
            AddItem<Skill>(new Skill(EAbility.Charisma,     "Intimidation"));
            AddItem<Skill>(new Skill(EAbility.Charisma,     "Performance"));
            AddItem<Skill>(new Skill(EAbility.Charisma,     "Persuasion"));
        }
    }

    public class SkillsDB : SimpleDB<Skill>
    {
        private static SkillsDB _Instance;

        public static SkillsDB Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SkillsDB();

                return _Instance;
            }
        }

        // Skills and Proficiencies are closely related to each other.
        public SkillsDB()
        {
          
        }
    }
}
