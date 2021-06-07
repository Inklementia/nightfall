using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine 
{ 
    public State _currentState { get; private set; }

    public void Initialize(State startingState)
    {
        _currentState = startingState;
        _currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
