using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Encounter : IState<PlayerController>
{
    public void OnEnter(PlayerController PlayerController)
    {
    
    }

    public void OnExecute(PlayerController PlayerController)
    {
    
    }

    public void OnExit(PlayerController PlayerController)
    {
    }
}

public class State_Normal : IState<PlayerController>
{
    public void OnEnter(PlayerController PlayerController)
    {
    }

    public void OnExecute(PlayerController PlayerController)
    {
        PlayerController.CastMousePointerIntoWorld();
        PlayerController.HandleSelectionDrag();
        PlayerController.PanCamera();
        PlayerController.ZoomCamera();
    }

    public void OnExit(PlayerController PlayerController)
    {
    }
}
