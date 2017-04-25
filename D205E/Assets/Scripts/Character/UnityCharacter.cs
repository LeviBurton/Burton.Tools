using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Burton.Lib.Characters.Combat;

public class UnityCharacter : MonoBehaviour
{
    public Character Character = null;
    public GameObject Target = null;

    public List<Spell> Spells;

    public List<CombatAction> DefaultCombatActions = new List<CombatAction>();

    // Use this for initialization
    void Start()
    {
        Character = new Character(new Wizard());
        Character.RollAbilities();
        Character.Name = "Wizard of Oz";

        // Set default actionss
        foreach (var Action in CombatActionManager.Instance.Actions)
        {
            var Instance = Instantiate(Action);
            Instance.BindCombatActionMethod<CombatActionDelegates>(Instance.ExecuteDelegateName);
            DefaultCombatActions.Add(Instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
