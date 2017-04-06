using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Burton.Lib
{
    [Serializable]
    public class DbItem
    {
        public int ID { get; set;  }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public DbItem(DateTime? DateCreated = null, DateTime? DateModified = null)
        {
            this.DateCreated = DateCreated.GetValueOrDefault();
            this.DateModified = DateModified.GetValueOrDefault();
        }

        public DbItem DeepCopy()
        {
            DbItem Other = (DbItem)this.MemberwiseClone();
            return Other;
        }

        public DbItem ShallowCopy()
        {
            DbItem Other = (DbItem)this.MemberwiseClone();
            return Other;
        }

    }

    [Serializable]
    public class SimpleDB<DbType> where DbType: DbItem
    {
        [NonSerialized]
        public  int InvalidItemID = -1;

        private  int NextValidID = 0;
        public  int GetNextValidID()
        {
            return ++NextValidID;
        }

        public void ResetID()
        {
            NextValidID = 0;
        }

        public List<DbType> Items = null;

        public SimpleDB()
        {
            Items = new List<DbType>();
        }

        public int Add(DbType Item)
        {
            Item.ID = GetNextValidID();
            Items.Add(Item);
            return Item.ID;
        }

        public IEnumerable<T> Find<T>(Func<T, bool> Predicate = null) where T : DbItem
        {
            var Result = new List<T>();

            if (Predicate == null)
            {
                // All
                Items.OfType<T>().ToList().ForEach(x => Result.Add((T)x.DeepCopy()));
            }
            else
            {
                // Predicate
                Items.OfType<T>().Where(Predicate).ToList().ForEach(x => Result.Add((T)x.DeepCopy()));
            }

            return Result.AsEnumerable();
        }

        public DbType Get(string Name)
        {
            return (DbType)Items.Where(x => x != null).Where(x => x.Name == Name).SingleOrDefault();
        }

      
        public DbType Get(int ID)
        {
            if (ID < 0)
            {
                throw new ArgumentException("ID < 0");
            }

            return Items.ElementAt(ID - 1);
        }

        public void Delete(int ID)
        {
            Items[ID - 1] = null;
        }

        public void Load(string FileName)
        {
            using (Stream InStream = File.Open(FileName, FileMode.Open))
            {
                var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                NextValidID = (int)BinaryFormatter.Deserialize(InStream);
                Items = (List<DbType>)BinaryFormatter.Deserialize(InStream);
            }
        }

        public void Save(string FileName)
        {
            using (Stream OutStream = File.Open(FileName, FileMode.Create))
            {
                var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                BinaryFormatter.Serialize(OutStream, NextValidID);
                BinaryFormatter.Serialize(OutStream, Items);
            }
        }
    }
}

