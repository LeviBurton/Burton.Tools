using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    public enum EAbility
    {
        Strength = 0,
        Dexterity = 1,
        Constitution = 2,
        Intelligence = 3,
        Wisdom = 4,
        Charisma = 5
    }
    
    public enum EDifficultyClass
    {
        VeryEasy = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
        VeryHard = 4,
        NearlyImpossible = 5
    }

    public class DifficultyClass
    {
        public EDifficultyClass ID { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
    }

    public static class DifficultyClassesTable
    {
        private static DataTable Table;
        public static void InitTable()
        {
            Table = new DataTable("DifficultyClasses");

            Table.Columns.Add(new DataColumn("ID", typeof(EDifficultyClass)));
            Table.Columns.Add(new DataColumn("Name", typeof(string)));
            Table.Columns.Add(new DataColumn("Value", typeof(int)));

            // Ability Checks
            // ===============================
            // Task Difficulty      DC
            // ------------------------
            // Very Easy            5
            // Easy                 10
            // Medium               15
            // Hard                 20
            // Very Hard            25
            // Nearly Impossible    30
            Table.Rows.Add(new Object[] { EDifficultyClass.VeryEasy, "Very Easy", 5 });
            Table.Rows.Add(new Object[] { EDifficultyClass.Easy, "Easy", 10 });
            Table.Rows.Add(new Object[] { EDifficultyClass.Medium, "Medium", 15 });
            Table.Rows.Add(new Object[] { EDifficultyClass.Hard, "Hard", 20 });
            Table.Rows.Add(new Object[] { EDifficultyClass.VeryHard, "Very Hard", 25 });
            Table.Rows.Add(new Object[] { EDifficultyClass.NearlyImpossible, "Nearly Impossible", 30 });

            Table.AcceptChanges();
        }

        public static int GetDifficultyByClass(EDifficultyClass Class)
        {
            if (Table == null)
            {
                DifficultyClassesTable.InitTable();
            }

            return (int)DifficultyClassesTable.Table.Rows[(int)Class]["Value"];
        }
    }

    public static class AbilityModifierTable
    {
        private static DataTable Table;

        public static void InitTable()
        {
            Table = new DataTable("AbilityModifiers");
            Table.Columns.Add(new DataColumn("Value", typeof(int)));
            Table.Columns.Add(new DataColumn("Modifier", typeof(int)));

            Table.Rows.Add(new Object[] { 0, 0 });
            Table.Rows.Add(new Object[] { 1, -5 });
            Table.Rows.Add(new Object[] { 2, -4 });
            Table.Rows.Add(new Object[] { 3, -4 });
            Table.Rows.Add(new Object[] { 4, -3 });
            Table.Rows.Add(new Object[] { 5, -3 });
            Table.Rows.Add(new Object[] { 6, -2 });
            Table.Rows.Add(new Object[] { 7, -2 });
            Table.Rows.Add(new Object[] { 8, -1 });
            Table.Rows.Add(new Object[] { 9, -1 });
            Table.Rows.Add(new Object[] { 10, 0 });
            Table.Rows.Add(new Object[] { 11, 0 });
            Table.Rows.Add(new Object[] { 12, 1 });
            Table.Rows.Add(new Object[] { 13, 1 });
            Table.Rows.Add(new Object[] { 14, 2 });
            Table.Rows.Add(new Object[] { 15, 2 });
            Table.Rows.Add(new Object[] { 16, 3 });
            Table.Rows.Add(new Object[] { 17, 3 });
            Table.Rows.Add(new Object[] { 18, 4 });
            Table.Rows.Add(new Object[] { 19, 4 });
            Table.Rows.Add(new Object[] { 20, 5 });
            Table.Rows.Add(new Object[] { 21, 5 });
            Table.Rows.Add(new Object[] { 22, 6 });
            Table.Rows.Add(new Object[] { 23, 6 });
            Table.Rows.Add(new Object[] { 24, 7 });
            Table.Rows.Add(new Object[] { 25, 7 });
            Table.Rows.Add(new Object[] { 26, 8 });
            Table.Rows.Add(new Object[] { 27, 8 });
            Table.Rows.Add(new Object[] { 28, 9 });
            Table.Rows.Add(new Object[] { 29, 9 });
            Table.Rows.Add(new Object[] { 30, 10 });
            Table.AcceptChanges();
        }

        public static int GetModifier(int AbilityValue)
        {
            if (Table == null)
            {
                InitTable();
            }

            return (int)Table.Rows[AbilityValue]["Modifier"];
        }
    }

    [Serializable]
    public class Ability : INotifyPropertyChanged
    {
        public EAbility ID { get; set; }

        public string Name
        {
            get
            {
                return ID.ToString();
            }
        }
        public string ShortName
        {
            get
            {
                return ID.ToString().Substring(0, 3).ToUpper();
            }
        }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        private int _CurrentValue;
        public int CurrentValue
        {
            get
            {
                return _CurrentValue;
            }

            set
            {
                _CurrentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        public int GetModifier()
        {
            return (CurrentValue - 10) / 2;
            //return AbilityModifierTable.GetModifier(CurrentValue);
        }

        public Ability(Ability Other)
        {
            this.ID = Other.ID;
            this.MaxValue = Other.MaxValue;
            this.MinValue = Other.MinValue;
            this.CurrentValue = Other.CurrentValue;
        }

        public Ability(EAbility TypeID, int MinValue, int MaxValue, int CurrentValue)
        {
            this.ID = TypeID;
            this.MaxValue = MaxValue;
            this.MinValue = MinValue;
            this.CurrentValue = CurrentValue;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
