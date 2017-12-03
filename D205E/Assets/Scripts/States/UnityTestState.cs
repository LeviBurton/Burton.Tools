using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Burton.Lib.StateMachine;

public class UnityTestState : State<UnityTestEntity>
{
    public override void OnEnter(UnityTestEntity Entity)
    {
        Debug.Log("OnEnter");
    }

    public override void OnExecute(UnityTestEntity Entity)
    {
        Debug.LogFormat("OnExecute {0}", Entity.gameObject.name);
    }

    public override void OnExit(UnityTestEntity Entity)
    {
        Debug.Log("OnExit");
    }
}
