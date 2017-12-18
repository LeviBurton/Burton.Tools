using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnBeginState : BaseState<TurnManager>
{
    public override void OnEnter(TurnManager TurnManager)
    {
        Debug.Log("TurnBeginState::OnEnter");
    }

    public override void OnExecute(TurnManager TurnManager)
    {
        Debug.Log("TurnBeginState::OnExecute");
    }

    public override void OnExit(TurnManager TurnManager)
    {
        Debug.Log("TurnBeginState::OnExit");
    }
}

public class TurnEndState : BaseState<TurnManager>
{
    public override void OnEnter(TurnManager TurnManager)
    {
    }

    public override void OnExecute(TurnManager TurnManager)
    {

    }

    public override void OnExit(TurnManager TurnManager)
    {
    }
}


public class TurnRunState : BaseState<TurnManager>
{
    public override void OnEnter(TurnManager TurnManager)
    {
    }

    public override void OnExecute(TurnManager TurnManager)
    {

    }

    public override void OnExit(TurnManager TurnManager)
    {
    }
}
