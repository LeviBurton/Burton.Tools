using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Burton.Lib.StateMachine;

public class GameStartState : IState<GameInstance>
{
    static float RoundStartWaitTime;

    public void OnEnter(GameInstance GameManager)
    {
        Debug.Log("GameStartState::OnEnter");
        RoundStartWaitTime = GameManager.GameMode.RoundStartWaitTime;
    }

    public void OnExecute(GameInstance GameManager)
    {
        RoundStartWaitTime -= Time.deltaTime;
        Debug.LogFormat("RoundStartWaitTime: {0}", RoundStartWaitTime);

        if (RoundStartWaitTime <= 0)
        {
            GameManager.StateMachine.ChangeState(GameManager.State_GameRunning);
        }
    }

    public void OnExit(GameInstance GameManager)
    {
        Debug.Log("GameStartState::OnExit");
    }
}

public class GameRunningState : IState<GameInstance>
{
    public void OnEnter(GameInstance GameManager)
    {
        GameManager.RoundManager.StateMachine.ChangeState(GameManager.RoundManager.State_RoundBegin);
    }

    public void OnExecute(GameInstance GameManager)
    {

    }

    public void OnExit(GameInstance GameManager)
    {
    }
}


public class GameEndState : IState<GameInstance>
{
    public void OnEnter(GameInstance GameManager)
    {

    }

    public void OnExecute(GameInstance GameManager)
    {

    }

    public void OnExit(GameInstance GameManager)
    {
    }
}


public class MainMenuGameState : IState<GameInstance>
{
    public void OnEnter(GameInstance GameManager)
    {
    }

    public void OnExecute(GameInstance GameManager)
    {
    }

    public void OnExit(GameInstance GameManager)
    {
    }
}
