using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager
{
    public StateMachine<RoundManager> StateMachine;

    public RoundManager()
    {
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
