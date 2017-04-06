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
    }

    [Serializable]
    public class SimpleDB<Type> where Type: DbItem
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

        public List<Type> Items = null;

        public SimpleDB()
        {
            Items = new List<Type>();
        }

        public int Add(Type Item)
        {
            Item.ID = GetNextValidID();
            Items.Add(Item);
            return Item.ID;
        }

        public Type Get(string Name)
        {
            return (Type)Items.Where(x => x != null).Where(x => x.Name == Name).SingleOrDefault();
        }

      
        public Type Get(int ID)
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
                Items = (List<Type>)BinaryFormatter.Deserialize(InStream);
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

