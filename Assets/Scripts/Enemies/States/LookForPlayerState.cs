using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayerState _stateData;

    protected bool _turnImmediately;
    protected bool _isPlayerInMinAgroRange;
    protected bool _isAllTurnsDone;
    protected bool _isAllTurnsTimeDone;

    protected float _lastTurnTime;

    protected int _amountOfTurnsDone;

    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _isPlayerInMinAgroRange = _entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        _isAllTurnsDone = false;
        _isAllTurnsTimeDone = false;

        _lastTurnTime = _startTime;
        _amountOfTurnsDone = 0;

        _entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_turnImmediately)
        {
            _entity.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
            _turnImmediately = false;

        } else if (Time.time >= _lastTurnTime + _stateData._timeBetweenTurns && !_isAllTurnsDone)//if time is up but we still have turns to do
        {
            _entity.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
        }

        if(_amountOfTurnsDone >= _stateData._amountOfTurns)
        {
            _isAllTurnsDone = true;
        }

        if (Time.time >= _lastTurnTime + _stateData._timeBetweenTurns && _isAllTurnsDone) //when everything is done, we wait again 1 turnTime and do whatever 
        {
            _isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void TurnImmediately(bool flip)
    {
        _turnImmediately = flip;
    }
}
