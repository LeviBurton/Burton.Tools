using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public class ItemDB : SimpleDB<Item>
    {
        public ItemDB()
        {
            InitBase();
        }

        public void InitBase()
        {
            Items.Clear();
        }
    }
}
