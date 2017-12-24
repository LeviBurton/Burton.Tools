using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour, ISelectable
{
    public bool Selected = false;
    private Outline Outline;

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
        OnDeselect();
	}
}
