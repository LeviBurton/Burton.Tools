using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {
    public EventSystem EventSystem;
    public GameObject SelectedObject;

    private bool bButtonSelected = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && bButtonSelected == false)
        {
            EventSystem.SetSelectedGameObject(SelectedObject);
            bButtonSelected = true;
        }
	}

    private void OnDisable()
    {
        bButtonSelected = false;
    }
}
