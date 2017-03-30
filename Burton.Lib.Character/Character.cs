using System.Collections.Generic;

using Burton.Lib.Characters.Quirks;
using Burton.Lib.Characters.Skills;
using System;
using System.ComponentModel;

namespace Burton.Lib.Characters
{
    public class Character
    {
        public string Name { get; set; }

        // Ability Scores
        // ===============================
        // When creating a new character, roll 4d6, discard the lowest result,
        // then add the 3 together.
        // Repeat for all 6, then assign them to your abilities
        public List<Ability> Abilities;

        // Weight in pounds they can carry.
        public int CarryingCapacity
        {
            get
            {
                return Abilities[(int)EAbility.Strength].CurrentValue * 15;
            }
        }

        public Class Class { get; set; }

        public int Level { get; set; }

        public Character(Class CharacterClass)
        {
            Abilities = new List<Ability>(6);
            Class = CharacterClass;
            Level = 1;
        
            var AbilityTypes = Enum.GetValues(typeof(EAbility));
            foreach (int AbilityType in AbilityTypes)
            {
                var ToAdd = new Ability((EAbility)AbilityType, 1, 30, 0);
                ToAdd.PropertyChanged += OnAbilityScoreChanged;
                Abilities.Insert(AbilityType, ToAdd);
            }
        }

        public Ability GetAbility(EAbility AbilityType)
        {
            return Abilities[(int)AbilityType];
        }

        public void OnAbilityScoreChanged(object Sender, PropertyChangedEventArgs e)
        {
            var AbilityThatChanged = (Ability)Sender;
        }
    }
}
