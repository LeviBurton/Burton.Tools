using System.Collections.Generic;
using System;
using System.ComponentModel;
using Burton.Lib.Characters.Combat;
using UnityEngine;

namespace Burton.Lib.Characters
{
    public enum ECreatureSizeType
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Gargantuan
    }
    public static class CreatureSize
    {
        public static float GetSpace(ECreatureSizeType Type)
        {
            float Size = 0.0f;

            if (Type == ECreatureSizeType.Tiny)
                return 2.5f;
            else if (Type == ECreatureSizeType.Small)
                return 5;
            else if (Type == ECreatureSizeType.Medium)
                return 5;
            else if (Type == ECreatureSizeType.Large)
                return 10;
            else if (Type == ECreatureSizeType.Huge)
                return 15;
            else if (Type == ECreatureSizeType.Gargantuan)
                return 20;

            return Size;
        }
    }

    public enum EAlignmentMoralityType
    {
        Good,
        Neutral,
        Evil
    }
  
    public enum EAlignmentAttitudeType
    {
        Lawful,
        Neutral,
        Chaotic
    }
  
    [Serializable]
    [CreateAssetMenu(fileName = "Character", menuName = "Character", order = 1)]
    public class Character : ScriptableObject
    {
        public string Name;
        public int Level;

        public Class Class { get; set; }
        public List<Ability> Abilities;
        public EAlignmentMoralityType AlignmentMoralityType;
        public EAlignmentAttitudeType AlignmentAttitudeType;

        // Their current equipment (weapons, armor, etc -- everything)
        public List<Item> Equipment;
        public List<CombatAction> CombatActions = new List<CombatAction>();

        // Weight in pounds they can carry.
        public int CarryingCapacity
        {
            get
            {
                return Abilities[(int)EAbility.Strength].CurrentValue * 15;
            }
        }

        public int Speed
        {
            get
            {
                return 0;
            }
        }
        public Armor EquippedArmor;

        public Character(Class CharacterClass)
        {
            Abilities = new List<Ability>(6);
            Equipment = new List<Item>();
 
            Class = CharacterClass;
            Level = 1;
            AlignmentAttitudeType = EAlignmentAttitudeType.Lawful;
            AlignmentMoralityType = EAlignmentMoralityType.Good;

            var AbilityTypes = Enum.GetValues(typeof(EAbility));
            foreach (int AbilityType in AbilityTypes)
            {
                var ToAdd = new Ability((EAbility)AbilityType, 1, 30, 0);
                Abilities.Insert(AbilityType, ToAdd);
            }

           // CombatActions = CombatActionManager.Instance.Actions;
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


        // consider turning this into an interface called IAbility.
        // things that need abilities then implement that interface
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
