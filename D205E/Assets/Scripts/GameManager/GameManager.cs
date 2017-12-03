using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Burton.Lib.StateMachine;

public class GameManager : MonoBehaviour
{
    public StateMachine<GameManager> StateMachine;

    void Start ()
    {
        StateMachine = new StateMachine<GameManager>(this);
        StateMachine.ChangeState(new GameRunningState());
	}
	
	void Update ()
    {
        StateMachine.Update();	
	}
}
