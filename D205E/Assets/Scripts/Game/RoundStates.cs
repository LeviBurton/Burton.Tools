using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBeginState : IState<RoundManager>
{
    public static float Seconds = 0;

    public void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundBeginState::OnEnter");
    }

    public void OnExecute(RoundManager RoundManager)
    {
        RoundManager.StateMachine.ChangeState(RoundManager.State_RoundRun);
    }

    public void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundBeginState::OnExit");
    }
}

public class RoundRunState : IState<RoundManager>
{
    public float RoundRunTime;

    public void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundRunState::OnEnter");
        RoundRunTime = RoundManager.GameInstance.GameMode.GameRunTime;
    }

    public void OnExecute(RoundManager RoundManager)
    {
        RoundRunTime -= Time.deltaTime;
        Debug.LogFormat("RoundRunTime: {0}", RoundRunTime);
        if (RoundRunTime <= 0)
        {
            RoundManager.StateMachine.ChangeState(RoundManager.State_RoundEnd);
        }
    }

    public void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundRunState::OnExit");
    }
}

public class RoundEndState : IState<RoundManager>
{
    public  void OnEnter(RoundManager RoundManager)
    {
        Debug.Log("RoundEndState::OnEnter");
    }

    public void OnExecute(RoundManager RoundManager)
    {
        Debug.Log("Exiting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnExit(RoundManager RoundManager)
    {
        Debug.Log("RoundEndState::OnExit");
    }
}



