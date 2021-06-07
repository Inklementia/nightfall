using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{ 
    public PlayerState CurrentState { get; private set; }

    // with which state to start
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // to change states from outside
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
