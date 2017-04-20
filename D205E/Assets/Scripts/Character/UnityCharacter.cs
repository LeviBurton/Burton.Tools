using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnityCharacter : MonoBehaviour
{
    public Character Character = null;
    public List<Spell> Spells;

	// Use this for initialization
	void Start ()
    {
        Character = new Character(new Class(8, EAbility.Intelligence));
        Character.RollAbilities();

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Bless").SingleOrDefault());
        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Burning Hands").SingleOrDefault());
        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Charm Person").SingleOrDefault());
        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Cure Wounds").SingleOrDefault());
        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Dispel Magic").SingleOrDefault());
        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Fireball").SingleOrDefault());

        Spells.ElementAt(0).Cast(Character);

    }

    // Update is called once per frame
    void Update ()
    {
	}
}
