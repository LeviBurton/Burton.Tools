using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Burton.Lib.Characters.Combat;

public class UnityCharacter : MonoBehaviour
{
    public List<Gear> Gear;
    public List<Spell> Spells;
    public List<Weapon> Weapons;
    public List<Armor> Armor;

    public List<CombatAction> DefaultCombatActions = new List<CombatAction>();

    public void MakeItemInstances()
    {
        var WeaponCopies = new List<Weapon>(Weapons);
        for (int i = 0; i < WeaponCopies.Count; i++)
        {
            Weapons[i] = new Weapon(WeaponCopies[i]);
        }
        WeaponCopies.Clear();

        var ArmorCopies = new List<Armor>(Armor);
        for (int i = 0; i < ArmorCopies.Count; i++)
        {
            Armor[i] = new Armor(ArmorCopies[i]);
        }
        ArmorCopies.Clear();

        var SpellCopies = new List<Spell>(Spells);
        for (int i = 0; i < SpellCopies.Count; i++)
        {
            Spells[i] = (Spell)new Spell(SpellCopies[i]);
        }
        SpellCopies.Clear();

        var GearCopies = new List<Gear>(Gear);
        for (int i = 0; i < GearCopies.Count; i++)
        {
            Gear[i] = (Gear)new Gear(GearCopies[i]);
        }

        GearCopies.Clear();
    }

    void Start()
    {
        MakeItemInstances();

        //Character = ScriptableObject.CreateInstance<Character>();
        //Character.Class = new Wizard();
        ////   Character.RollAbilities();
        //Character.Name = "Wizard of Oz";

        //// Set default actionss
        ////foreach (var Action in CombatActionManager.Instance.Actions)
        ////{
        ////    var Instance = Instantiate(Action);
        ////    Instance.BindCombatActionMethod<CombatActionDelegates>(Instance.ExecuteDelegateName);
        ////    DefaultCombatActions.Add(Instance);
        ////}
    }

    // Update is called once per frame
    void Update()
    {
    }
}
