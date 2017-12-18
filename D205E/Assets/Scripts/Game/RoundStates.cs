using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoundBeginState : BaseState<RoundManager>
{
    public static float Seconds = 0;

    public override void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundBeginState::OnEnter");
    }

    public override void OnExecute(RoundManager RoundManager)
    {
        // Test.
        Seconds += Time.deltaTime;
    
        if (Seconds >= 10)
        {
            RoundManager.StateMachine.ChangeState(new RoundRunState());
        }
    }

    public override void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundBeginState::OnExit");
    }
}

public class RoundEndState : BaseState<RoundManager>
{
    public override void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundEndState::OnEnter");
    }

    public override void OnExecute(RoundManager RoundManager)
    {
  
    }

    public override void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundEndState::OnExit");
    }
}


public class RoundRunState : BaseState<RoundManager>
{
    public override void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundRunState::OnEnter");
    }

    public override void OnExecute(RoundManager RoundManager)
    {

    }

    public override void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundRunState::OnExecute");
    }
}

