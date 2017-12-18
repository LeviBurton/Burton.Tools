using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager
{
    public GameInstance GameInstance;
    public StateMachine<RoundManager> StateMachine;

    public RoundBeginState State_RoundBegin = new RoundBeginState();
    public RoundEndState State_RoundEnd = new RoundEndState();
    public RoundRunState State_RoundRun = new RoundRunState();

    public RoundManager(GameInstance GameInstance)
    {
        this.GameInstance = GameInstance;
        StateMachine = new StateMachine<RoundManager>(this);
    }

   public void Update()
    {
        if (StateMachine != null)
        {
            StateMachine.Update();
        }
    }
}
