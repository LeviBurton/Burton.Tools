using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameMode is recreated on every level load.
// GameInstance will own the game mode
// GameMode will have all the game parameters, etc.
// This could be different per level.
public class GameMode : MonoBehaviour
{
    public float RoundStartWaitTime = 30;
    public float GameRunTime = 60;
}
