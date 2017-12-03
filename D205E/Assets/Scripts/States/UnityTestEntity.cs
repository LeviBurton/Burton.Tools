﻿using Burton.Lib.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTestEntity : MonoBehaviour
{
    public StateMachine<UnityTestEntity> StateMachine;

    public void Start()
    {
        StateMachine = new StateMachine<UnityTestEntity>(this);
        StateMachine.ChangeState(new UnityTestState());
    }

    void Update()
    {
        StateMachine.Update();
    }
}
