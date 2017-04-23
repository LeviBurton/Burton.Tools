using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class UnityCharacter : MonoBehaviour
{
    public Character Character = null;
    public GameObject Target = null;

    public List<Spell> Spells;

    // Use this for initialization
    void Start()
    {
        SpellManager.Instance.Load(Application.streamingAssetsPath + "/Data/Spells.bytes");

        Character = new Character(new Wizard());

        Character.RollAbilities();
        Spells = new List<Spell>();
        Character.Name = "Test";

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Bless").SingleOrDefault());
        Spells.ElementAt(0).SetSpellMethod<UnitySpellMethods>("Bless");

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Burning Hands").SingleOrDefault());
        Spells.ElementAt(1).SetSpellMethod<UnitySpellMethods>("Burning_Hands");

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Charm Person").SingleOrDefault());
        Spells.ElementAt(2).SetSpellMethod<UnitySpellMethods>("Charm_Person");

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Cure Wounds").SingleOrDefault());
        Spells.ElementAt(3).SetSpellMethod<UnitySpellMethods>("Cure_Wounds");

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Dispel Magic").SingleOrDefault());
        Spells.ElementAt(4).SetSpellMethod<UnitySpellMethods>("Dispel_Magic");

        Spells.Add(SpellManager.Instance.Find<Spell>(x => x.Name == "Fireball").SingleOrDefault());
        Spells.ElementAt(5).SetSpellMethod<UnitySpellMethods>("Fireball");

        Spells.ElementAt(0).Cast(this);
        Spells.ElementAt(1).Cast(this);
        Spells.ElementAt(2).Cast(this);
        Spells.ElementAt(3).Cast(this);
        Spells.ElementAt(4).Cast(this);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
