using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

namespace Burton.Lib
{
    [Serializable]
    public class DbItem : ScriptableObject
    {
        public int ID;
        public string Name;
        public DateTime DateCreated;
        public DateTime DateModified;

        public DbItem(DateTime? DateCreated = null, DateTime? DateModified = null)
        {
            this.DateCreated = DateCreated.GetValueOrDefault();
            this.DateModified = DateModified.GetValueOrDefault();
        }

        // We let any items stored override this to 
        // create a copy of themselves.
        public virtual DbItem Clone()
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
                Items.OfType<T>().ToList().ForEach(x => Result.Add((T)x.Clone()));
            }
            else
            {
                // Predicate
                Items.OfType<T>().Where(Predicate).ToList().ForEach(x => Result.Add((T)x.Clone()));
            }

            return Result.AsEnumerable();
        }

    
        public void Delete(int ID)
        {
            Items[ID - 1] = null;
        }


        public void Load(string FileName)
        {
            using (Stream InStream = File.Open(FileName, FileMode.Open))
            {
                Load(InStream);
            }
        }

        public void Load(Stream InStream)
        {
            var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            NextValidID = (int)BinaryFormatter.Deserialize(InStream);
            Items = (List<DbType>)BinaryFormatter.Deserialize(InStream);
        }

        public void Save(string FileName)
        {
            using (Stream OutStream = File.Open(FileName, FileMode.Create))
            {
                Save(OutStream);
            }
        }

        public void Save(Stream OutStream)
        {
            var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            BinaryFormatter.Serialize(OutStream, NextValidID);
            BinaryFormatter.Serialize(OutStream, Items);
        }
    }
}

