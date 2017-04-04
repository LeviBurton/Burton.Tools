using System.Collections.Generic;

using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;
using System;
using System.ComponentModel;
using Burton.Lib.Dice;

namespace Burton.Lib.Characters
{
    public enum EAlignmentMorality
    {
        Good,
        Neutral,
        Evil
    }
    public class AlignmentMorality
    {
        public EAlignmentMorality Type;
        public string Name
        {
            get
            {
                return Type.ToString();
            }
        }

        public string ShortName
        {
            get
            {
                return Type.ToString().Substring(0, 1).ToUpper();
            }
        }
    }

    public enum EAlignmentAttitude
    {
        Lawful,
        Neutral,
        Chaotic
    }
    public class AlignmentAttitude
    {
        public EAlignmentAttitude Type;
        public string Name
        {
            get
            {
                return Type.ToString();
            }
        }

        public string ShortName
        {
            get
            {
                return Type.ToString().Substring(0, 1).ToUpper();
            }
        }
    }

    [Serializable]
    public class Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _Level;
        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                _Level = value;
                OnPropertyChanged("Level");
            }
        }

        public Class Class { get; set; }
        public List<Ability> Abilities;
        public AlignmentMorality AlignmentMorality;
        public AlignmentAttitude AlignmentAttitude;

        // Their current equipment (weapons, armor, etc -- everything)
        public List<Item> Equipment;

        // Weight in pounds they can carry.
        public int CarryingCapacity
        {
            get
            {
                return Abilities[(int)EAbility.Strength].CurrentValue * 15;
            }
        }

        public Armor EquippedArmor;

        public Character(Class CharacterClass)
        {
            Abilities = new List<Ability>(6);
            Equipment = new List<Item>();

            Class = CharacterClass;
            Level = 1;
            AlignmentAttitude = new AlignmentAttitude();
            AlignmentMorality = new AlignmentMorality();

            var AbilityTypes = Enum.GetValues(typeof(EAbility));
            foreach (int AbilityType in AbilityTypes)
            {
                var ToAdd = new Ability((EAbility)AbilityType, 1, 30, 0);
                Abilities.Insert(AbilityType, ToAdd);
            }
        }

        public bool CanEquip(Item E)
        {
            bool bCanEquip = true;
            foreach (var Requirement in E.Require_Abilities)
            {
                if (GetAbility(Requirement.ID).CurrentValue < Requirement.CurrentValue)
                {
                    bCanEquip = false;
                }
            }
            return bCanEquip;
        }

        public void EquipArmor(Armor ArmorToEquip)
        {
            EquippedArmor = ArmorToEquip;
        }

        public void RollAbilities()
        {
            foreach (var Ability in Abilities)
            {
                // Roll 4D6 and remove the lowest die.
                var Roll = DiceRoller.Instance.Roll(4, 6);
                Roll.Sort();
                Roll.RemoveAt(0);

                int Sum = 0;
                foreach (var r in Roll) Sum += r;
                Ability.CurrentValue = Sum;
            }
        }

        public Ability GetAbility(EAbility AbilityType)
        {
            return Abilities[(int)AbilityType];
        }

        // Just an example event handler for an ability score change.
        public void OnAbilityScoreChanged(object Sender, PropertyChangedEventArgs e)
        {
            var AbilityThatChanged = (Ability)Sender;
        }
    }
}
