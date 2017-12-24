using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    bool IsSelected();
    void OnToggleSelected();
    void OnSelect();
    void OnDeselect();
}
