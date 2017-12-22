using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Burton.Lib.StateMachine;

public class GameInstance : MonoBehaviour
{
    public StateMachine<GameInstance> StateMachine;
    public State_GameStart State_GameStart = new State_GameStart();
    public State_GameRunning State_GameRunning = new State_GameRunning();
    public State_GameEnd State_GameEnd = new State_GameEnd();
    public State_GlobalGameState State_GlobalGameState = new State_GlobalGameState();

    public RoundManager RoundManager = null;
    public GameMode GameMode = null;

    public float ElapsedGameTime = 0.0f;

    void Awake ()
    {
        // We need to survive level/map loads, etc.
        DontDestroyOnLoad(gameObject);

        StateMachine = new StateMachine<GameInstance>(this);
        RoundManager = new RoundManager(this);

        StateMachine.ChangeState(State_GameStart);
        StateMachine.GlobalState = State_GlobalGameState;

	}
    public void FixedUpdate()
    {
        StateMachine.Update();
        RoundManager.Update();
    }
    public void Update ()
    {
    
	}
}

/*
 * 
 * Thoughts --
 *  GameModes
 *      Objectives
 *      Win/Lose conditions
 *      Rewards
 *  
 *  GaneStates
 *   CombatEncounter
 *      Round
 *      Rounds are 6 seconds in the game world.  Each participant takes a turn during the round.  Order of
 *      turns is determined at the beginning of the round where everyone rolls initiative.
 *      
 *      A round can be broken down into the following steps:
 *          1.  Determine surprise.  GameManager will determine if anyone in the combat encounter is surprised
 *              1.1 SUrprised combatants can't move or take an action on the FIRST turn of combat.
 *          2.  Determine locations (spawn locations, etc.)  
 *          3.  Roll initiative.  This will determine the order of combatants.
 *          4.  First combatant begins their turn
 *              4.1 Combatants take turns in initiative order (highest to lowest)
 *          5.  Begin the next round.  When everyone has had a turn, the combatant with the highest initiative acts again, and repeat step 4.
 *          
 *      Turn: 
 *  
 *  Load Map, establish game mode of map

 *  
 *  Game is broken down into rounds?  What is D20 term for this?
 *  
 *  From the 5E SRD
 *  
 *  A typical combat encounter is a clash between two sides, a flurry of weapon swings, feints, parries, footwork, and spellcasting. 
 *  The game organizes the chaos of combat into a cycle of rounds and turns. A round represents about 6 seconds in the game world. 
 *  During a round, each participant in a battle takes a turn. The order of turns is determined at the beginning of a combat encounter, when everyone rolls initiative. 
 *  Once everyone has taken a turn, the fight continues to the next round if neither side has defeated the other.

 * Combat Step by Step
 * Determine surprise. The GM determines whether anyone involved in the combat encounter is surprised. 
 * Combatants who are surprised can't move or take an action on the first turn of combat, and can't take a reaction until that turn ends.
 * Establish positions. The GM decides where all the characters and monsters are located. 
 * Given the adventurers' marching order or their stated positions in the room or other location, the GM figures out where the adversaries are--how far away and in what direction.
 * Roll initiative. Everyone in the combat encounter rolls initiative, determining the  order of combatants' turns.  do so. 
 * All combatants are now ready to begin their first regular round of combat.
 * Take turns. Combatants act in initiative order (highest to lowest).
 * Begin the next round. When everyone has had a turn, the combatant with the highest initiative acts again, and steps 4 and 5 repeat until combat ends.
 */
