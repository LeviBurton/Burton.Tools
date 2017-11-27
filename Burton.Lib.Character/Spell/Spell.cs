using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Burton.Lib.Characters
{
    public enum ESpellSchoolType
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    }

    [Serializable]
    public class SpellMaterial : Item
    {
        bool bConsumeOnUse = false;

        public SpellMaterial(string Name, string Description, int Cost, bool ConsumeOnUse)
            :base(EItemType.Special_Material, EItemSubType.Spell_Material, EItemRarity.Common, Name, Description, 0, Cost, 0, null)
        {
            this.bConsumeOnUse = ConsumeOnUse;
        }

        public override DbItem Clone()
        {
            return (DbItem)this.MemberwiseClone();
        }
    }

    public enum ECastingComponentType
    {
        None,
        Verbal,
        Somatic,
        Material
    }
  

    public enum ESpellRangeType
    {
        None,
        Touch,
        Self,
        Distance,
        Sight
    }

    public enum ESpellSelfRangeType
    {
        None,
        Radius,
        Line,
        Sphere,
        Cone,
        Cube
    }

    [Serializable]
    public class SpellRange
    {
        public ESpellRangeType RangeType;
        public ESpellSelfRangeType SelfRangeType;
        public int Range;

        public SpellRange() {}

        public SpellRange(ESpellRangeType RangeType, ESpellSelfRangeType SelfRangeType, int Range)
        {
            this.RangeType = RangeType;
            this.SelfRangeType = SelfRangeType;
            this.Range = Range;
        }

        public SpellRange(SpellRange Other)
        {
            this.Range = Other.Range;
            this.SelfRangeType = Other.SelfRangeType;
            this.RangeType = Other.RangeType;
        }

        public SpellRange Clone()
        {
            return (SpellRange)this.MemberwiseClone();
        }
    }


    [CreateAssetMenu(fileName = "Spell", menuName = "Spells", order = 1)]
    public class Spell : DbItem
    {
        private Action<Spell, object> CastDelegate = null;

        public ESpellSchoolType MagicSchool;
        public int Level;
        public int CastingTime;
        public bool bConcentration;
        public string Description;

        public List<EClassType> Classes;
        public List<ECastingComponentType> CastingComponentTypes;
        public List<SpellMaterial> SpellMaterials;

        public SpellRange SpellRange;
        public string SpellMethodName = string.Empty;
        public MethodInfo SpellMethodInfo = null;

 
        public Spell(ESpellSchoolType MagicSchool, 
                     List<EClassType> ClassTypes, 
                     string Name, 
                     int Level, 
                     SpellRange Range, 
                     string Description, 
                     List<ECastingComponentType> SpellCastingComponentType,
                     List<SpellMaterial> SpellMaterials,
                     bool bConcentration,
                     string Duration)
        {
            this.MagicSchool = MagicSchool;
            this.Classes = ClassTypes;
            this.Name = Name;
            this.Level = Level;
            this.SpellRange = Range;
            this.Description = Description;
            this.CastingComponentTypes = SpellCastingComponentType;
            this.bConcentration = bConcentration;
            this.SpellMaterials = SpellMaterials;
        }

        public Spell(Spell Other)
        {
            this.bConcentration = Other.bConcentration;
            this.CastDelegate = Other.CastDelegate;
            this.CastingComponentTypes = Other.CastingComponentTypes;
            this.CastingTime = Other.CastingTime;
            this.Classes = Other.Classes;
            this.DateCreated = Other.DateCreated;
            this.DateModified = Other.DateModified;
            this.ID = Other.ID;
            this.Level = Other.Level;
            this.MagicSchool = Other.MagicSchool;
            this.Name = Other.Name;
            this.SpellMaterials = Other.SpellMaterials;
            this.SpellMethodInfo = Other.SpellMethodInfo;
            this.SpellMethodName = Other.SpellMethodName;
            this.SpellRange = Other.SpellRange;
        }

        #region Unity ScriptableObject
        public void OnEnable()
        {
        }

        public void OnDestroy()
        {

        }

        public void OnDisable()
        {

        }
        #endregion region

        public override DbItem Clone()
        {
            var Other = (Spell)this.MemberwiseClone();
            Other.Classes = new List<EClassType>(this.Classes);
            Other.SpellRange = new SpellRange(this.SpellRange);
            Other.CastingComponentTypes = new List<ECastingComponentType>();
            Other.SpellMaterials = new List<SpellMaterial>();
            
            foreach (var m in this.SpellMaterials)
            {
                Other.SpellMaterials.Add(m);
            }

            foreach (var c in this.CastingComponentTypes)
            {
                Other.CastingComponentTypes.Add(c);
            }

            return (DbItem) Other;
        }

        // Unity scriptable object seems to take care of 
        // serializing our delegate!  This means 
        // we can bind, then save.
        public void BindSpellMethod<T>(string MethodName)
        {
            SpellMethodName = MethodName;

            SpellMethodInfo = typeof(T).Assembly
                .GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttributes(true).OfType<SpellMethodAttribute>().Any())
                .Where(x => x.Name == MethodName).SingleOrDefault();

            if (SpellMethodInfo == null)
                return;

            CastDelegate = (Action<Spell,object>)Delegate.CreateDelegate(typeof(Action<Spell, object>), SpellMethodInfo);
        }

        public void Cast(object Caster)
        {
            if (CastDelegate == null)
                return;

            CastDelegate(this, Caster);
        }
    }

  
}
