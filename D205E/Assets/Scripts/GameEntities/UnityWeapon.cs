using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityWeapon : MonoBehaviour
{
    private Weapon ItemInstance;
    public Weapon ItemDefinition;

    // Use this for initialization
    void Start()
    {
        Debug.Log(string.Format("Name: {0}, Type: {1}, SubType: {2}", ItemDefinition.Name, ItemDefinition.Type.ToString(), ItemDefinition.SubType.ToString()));

        // Must clone otherwise we will edit the underlying item asset.
        ItemInstance = (Weapon)ItemDefinition.Clone();
    }

    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(string.Format("Name: {0}, Type: {1}, SubType: {2}", ItemDefinition.Name, ItemDefinition.Type.ToString(), ItemDefinition.SubType.ToString()));
    }
}
