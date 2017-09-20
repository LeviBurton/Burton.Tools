using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnityItem : MonoBehaviour
{
    public Item ItemDefinition;

	// Use this for initialization
	void Start () {
        Debug.Log(string.Format("Name: {0}, Type: {1}, SubType: {2}", ItemDefinition.Name, ItemDefinition.Type.ToString(), ItemDefinition.SubType.ToString()));
	}
	
	// Update is called once per frame
	void Update () {
        if (ItemDefinition.Type == EItemType.Weapon)
        {
            var Weapon = ItemDefinition as Weapon;
            if (Weapon != null)
            {
                Debug.Log("I am a Weapon!");
            }
        }
	}
}
