using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Burton.Lib
{
    [Serializable]
    public class DbItem
    {
        public int ID { get; set;  }
    }

    [Serializable]
    public class SimpleDB<Type> where Type: DbItem
    {
        [NonSerialized]
        public static int InvalidItemID = -1;

        private static int NextValidID = 0;
        public static int GetNextValidID()
        {
            return NextValidID++;
        }

        public List<Type> Items = null;

        public SimpleDB()
        {
            Items = new List<Type>(1024);
        }

        public int Add(Type Item)
        {
            Item.ID = GetNextValidID();
            Items.Add(Item);
            return Item.ID;
        }

        public Type Get(int ID)
        {
            return Items[ID];
        }

        public void Delete(int ID)
        {
            Items[ID] = null;
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
