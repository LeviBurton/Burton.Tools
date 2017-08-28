using Burton.Lib.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnityGear : MonoBehaviour
{
    private Gear ItemInstance;
    public Gear ItemDefinition;

    // Use this for initialization
    void Start()
    {
        //  Debug.Log(string.Format("Name: {0}, Type: {1}, SubType: {2}", ItemDefinition.Name, ItemDefinition.Type.ToString(), ItemDefinition.SubType.ToString()));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
