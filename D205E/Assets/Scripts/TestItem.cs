using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public bool Selected = false;
    private Outline Outline;
    SelectableGameObject Selectable = new SelectableGameObject();

    public void OnSelect()
    {
        Selected = true;
        Outline.enabled = Selected;
        Outline.color = 1;
    }

    public void OnDeselect()
    {
        Selected = false;
        Outline.enabled = false;
    }

    public void OnToggleSelected()
    {
        if (IsSelected())
        {
            OnDeselect();
        }
        else
        {
            OnSelect();
        }
    }

    public bool IsSelected()
    {
        return Selected;
    }

    public void OnMouseDown()
    {
        OnToggleSelected();
    }

    public void OnMouseEnter()
    {
        Outline.enabled = true;
        if (!IsSelected())
        {
            Outline.color = 0;
        }
    }

    public void OnMouseExit()
    {
        Debug.Log("OnMouseLeave");
        if (!IsSelected())
        {
            Outline.enabled = false;
        }
    }
   

    // Use this for initialization
    void Start () {
        Outline = GetComponent<Outline>();
        Selectable = GetComponent<SelectableGameObject>();

        // We go through all these delegate hoops so that we can just grab all the SelectableGameObjects later on.
        // That way we don't have to do any nasty inheritence or interfaces -- just tell the component how we want to behave.
        Selectable.OnSelectEvent += OnSelect;
        Selectable.OnToggleSelectedEvent += OnToggleSelected;
        Selectable.OnDeselectEvent += OnDeselect;

        OnDeselect();
	}   
}
