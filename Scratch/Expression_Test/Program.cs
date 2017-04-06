using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expression_Test
{
    public enum EItemType
    {
        Type1,
        Type2,
        Type3
    }

    public enum EItemSuperType
    {
        SuperType1,
        SuperType2,
        SuperType3
    }

    public class TestType
    {
        public int ID;
        public string Name;

        public TestType(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        public TestType(TestType Other)
        {
            this.ID = Other.ID;
            this.Name = Other.Name;
        }

    }
    public class SuperItem : BaseItem
    {
        public EItemSuperType ItemSuperType;

        public SuperItem(EItemType Type, EItemSuperType ItemSuperType, string Name)
            :base(Type, Name)
        {
            this.ItemSuperType = ItemSuperType;
            this.RefType = new TestType(ID, Name);
        }

        public SuperItem(SuperItem Other)
            : base(Other.ItemType, Other.Name, Other.ID)
        {
            this.ItemSuperType = Other.ItemSuperType;
        }

        public SuperItem ShallowCopy()
        {
            SuperItem Other = (SuperItem)MemberwiseClone();
            return Other;
        }
    }

    public class BaseItem 
    {
        public EItemType ItemType;
        public int ID;
        public string Name;
        public TestType RefType;

        public BaseItem(EItemType ItemType, string Name, int ID = 0)
        {
            this.ID = ID;
            this.Name = Name;
            this.ItemType = ItemType;
        }

        public BaseItem(BaseItem Other)
        {
            this.ID = Other.ID;
            this.ItemType = Other.ItemType;
            this.Name = Other.Name;
        }

        public BaseItem DeepCopy()
        {
            BaseItem Other = (BaseItem)this.MemberwiseClone();

            if (RefType != null)
            {
                Other.RefType = new TestType(RefType);
            }
      
            return Other;
        }

        public BaseItem ShallowCopy() 
        {
            BaseItem Other = (BaseItem)this.MemberwiseClone();
            return Other;
        }
    }

    public class Database<ItemType> where ItemType : BaseItem
    {
        List<ItemType> Items;
        private int NextValidID = 0;

        public Database()
        {
            Items = new List<ItemType>();
        }

        public int Add(ItemType Item)
        {
            Item.ID = ++NextValidID;

            Items.Add(Item);

            return Item.ID;
        }

        // Returns copies of objects of type T that match the Predicate.
        public IEnumerable<T> Find<T>(Func<T, bool> Predicate = null) where T : BaseItem
        {
            var Result = new List<T>();

            if (Predicate == null)
            {
                Items.OfType<T>().ToList().ForEach(x => Result.Add((T)x.DeepCopy()));

                return Result.AsEnumerable();
            }

            // Copy the items.
            Items.OfType<T>().Where(Predicate).ToList().ForEach(x => Result.Add((T)x.DeepCopy()));
        
            return Result.AsEnumerable();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = new Database<BaseItem>();

            db.Add(new SuperItem(EItemType.Type1, EItemSuperType.SuperType1, "Type 1 - 1"));
            db.Add(new SuperItem(EItemType.Type1, EItemSuperType.SuperType2, "Type 1 - 2"));
            db.Add(new SuperItem(EItemType.Type1, EItemSuperType.SuperType3, "Type 1 - 3"));
            db.Add(new BaseItem(EItemType.Type1, "BaseItem - Type 1"));
            db.Add(new BaseItem(EItemType.Type2, "BaseItem - Type 2"));

            // Find all items that are at least BaseItem (but if it's a subclass, that is preserved)
            var AllItems = db.Find<BaseItem>();

            AllItems.ElementAt(0).Name = "Foobar";
            AllItems.ElementAt(0).RefType.Name = "NO";

            var SuperItems = db.Find<SuperItem>(x => x.ItemSuperType == EItemSuperType.SuperType1).SingleOrDefault();
            var BaseItem = db.Find<BaseItem>(x => x.ID == 1).SingleOrDefault();

            var AllSuperItems = db.Find<SuperItem>().ToList();
            var AllBaseItems = db.Find<BaseItem>().ToList();
        }
    }
}
