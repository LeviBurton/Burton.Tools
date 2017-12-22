using UnityEngine;
using Burton.Lib.StateMachine;

public class State_GlobalGameState : IState<GameInstance>
{
    public void OnExecute(GameInstance GameInstance)
    {
        // Tick the game elapsed time.
        GameInstance.ElapsedGameTime += Time.deltaTime;
    }

    #region Unused.  Here to make compiler happy.
    public void OnEnter(GameInstance GameInstance)
    {
    }
    public void OnExit(GameInstance GameInstance)
    {
    }
    #endregion
}

public class State_GameStart : IState<GameInstance>
{
    public void OnEnter(GameInstance GameInstance)
    {

        GameInstance.ElapsedGameTime = 0;
    }

    public void OnExecute(GameInstance GameInstance)
    {
        GameInstance.StateMachine.ChangeState(GameInstance.State_GameRunning);
    }

    public void OnExit(GameInstance GameManager)
    {

    }
}

public class State_GameRunning : IState<GameInstance>
{
    public void OnEnter(GameInstance GameInstance)
    {
      
      //  GameInstance.RoundManager.StateMachine.ChangeState(GameInstance.RoundManager.State_RoundBegin);
    }

    public void OnExecute(GameInstance GameInstance)
    {

        if (GameInstance.ElapsedGameTime >= GameInstance.GameMode.GameRunTime)
        {
            //GameInstance.StateMachine.ChangeState(GameInstance.State_GameEnd);
        }
    }

    public void OnExit(GameInstance GameInstance)
    {

    }
}


public class State_GameEnd : IState<GameInstance>
{
    public void OnEnter(GameInstance GameInstance)
    {

    }

    public void OnExecute(GameInstance GameInstance)
    {
        Debug.Log("Exiting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnExit(GameInstance GameInstance)
    {
    }
}
