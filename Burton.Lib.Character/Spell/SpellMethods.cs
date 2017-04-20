using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burton.Lib.Characters
{
    // Allows us to use reflection to find SpellMethods in an assembly.
    // We do this to populate popup boxes in the Unity Editor.
    public class SpellMethodAttribute : Attribute
    {
    }

    public class SpellMethods
    {
        public SpellMethods()
        {
        }

        [SpellMethod]
        public static void Mage_Armor(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Mage_Armor via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Resistance(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Resistance via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Bless(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Bless via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Burning_Hands(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Burning_Hands via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Charm_Person(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Charm_Person via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }


        [SpellMethod]
        public static void Cure_Wounds(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Cure_Wounds via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Dispel_Magic(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Dispel_Magic via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

        [SpellMethod]
        public static void Fireball(Spell Spell, Character Caster)
        {
            Console.WriteLine("{0} called Fireball via Spell {1}, {2}", Caster.Name, Spell.ID, Spell.Name);
        }

    }
}
