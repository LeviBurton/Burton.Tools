using Burton.Lib.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitySpellMethods
{
    public UnitySpellMethods()
    {
    }

    [SpellMethod]
    public static void Mage_Armor(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Mage_Armor via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Resistance(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Resistance via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Bless(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Bless via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Burning_Hands(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Burning_Hands via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Charm_Person(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Charm_Person via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }
    
    [SpellMethod]
    public static void Cure_Wounds(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Cure_Wounds via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Dispel_Magic(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Dispel_Magic via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

    [SpellMethod]
    public static void Fireball(Spell Spell, object Caster)
    {
        var Character = Caster as UnityCharacter;
        Debug.LogFormat("{0} called Fireball via Spell {1}, {2}", Character.Character.Name, Spell.ID, Spell.Name);
    }

}
