using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectableGameObject : MonoBehaviour
{
    protected bool Selected;

    public Action OnSelectEvent;
    public Action OnDeselectEvent;
    public Action OnToggleSelectedEvent;
    public Func<bool> OnIsSelectedEvent;

    public virtual void OnSelect()
    {
        if (OnSelectEvent != null)
        {
            OnSelectEvent();
        }
    }

    public virtual void OnDeselect()
    {
        if (OnDeselectEvent != null)
        {
            OnDeselectEvent();
        }
    }

    public virtual void OnToggleSelected()
    {
        if (OnToggleSelectedEvent != null)
        {
            OnToggleSelectedEvent();
        }
        else
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
    }

    public virtual bool IsSelected()
    {
        if (OnIsSelectedEvent != null)
        {
            return OnIsSelectedEvent();
        }

        return Selected;
    }
}
