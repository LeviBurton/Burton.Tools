using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager 
{
    public StateMachine<TurnManager> StateMachine;

    public TurnManager()
    {
        StateMachine = new StateMachine<TurnManager>(this);
    }

    public void Update()
    {
        Debug.Log("TurnManager::Update");
        StateMachine.Update();
    }
}
