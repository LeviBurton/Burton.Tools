using Burton.Lib.Characters;
using Burton.Lib.Dice;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    Character EvilCleric = null;

	void Start ()
    {
        EvilCleric = new Character(new Cleric());

        // Meh.
        EvilCleric.PropertyChanged += OnCharacterPropertyChanged;
        EvilCleric.Level = 1;

        //// Lawful Evil cleric.
        //EvilCleric.AlignmentAttitude.Type = EAlignmentAttitude.Lawful;
        //EvilCleric.AlignmentMorality.Type = EAlignmentMorality.Evil;
     
        // listen for update to our abilities.
        EvilCleric.Abilities.ForEach(x => { x.PropertyChanged += OnAbilityScoreChanged; });

        // Roll the characters abilities
        EvilCleric.RollAbilities();

        //Debug.LogFormat("Alignment: {0}{1} ({2} {3})", EvilCleric.AlignmentAttitude.ShortName, EvilCleric.AlignmentMorality.ShortName, EvilCleric.AlignmentAttitude.Name, EvilCleric.AlignmentMorality.Name);

        EvilCleric.Equipment.Add(ItemManager.Instance.Find<Weapon>(x => x.Name == "Warhammer").SingleOrDefault());

        foreach (var Item in EvilCleric.Equipment)
        {
            Debug.LogFormat("{0} {1} {2}", Item.ID, Item.Name, Item.Type.ToString());
        }
    }

    public void OnCharacterPropertyChanged(object Sender, PropertyChangedEventArgs e)
    {
        
    }

    // There will be a lot of this type of thing going on so that we can handle changes
    // done to our underlying data.
    public void OnAbilityScoreChanged(object Sender, PropertyChangedEventArgs e)
    {
        var Ability = (Ability)Sender;
        Debug.LogFormat("OnAbilityScoreChanged {0}: {1} ({2})", Ability.ShortName, Ability.CurrentValue, Ability.GetModifier());
    }

    void Update ()
    {
	}
}
