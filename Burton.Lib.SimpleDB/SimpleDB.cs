﻿using System;
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
        public string Name { get; set; }
    }

    [Serializable]
    public class SimpleDB<Type> where Type: DbItem
    {
        [NonSerialized]
        public static int InvalidItemID = -1;

        private static int NextValidID = 0;
        public static int GetNextValidID()
        {
            return ++NextValidID;
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

            return Items[ID - 1];
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
