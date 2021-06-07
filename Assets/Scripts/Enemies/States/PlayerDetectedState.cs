using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState _stateData;

    protected bool _isPlayerInMinAgroRange; 
    protected bool _isPlayerInMaxAgroRange;
    protected bool _performLongRangeAction;
    protected bool _performShortRangeAction;
    protected bool _isDetectingLedge;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();
        _isPlayerInMinAgroRange = _entity.CheckPlayerInMinAgroRange();
        _isPlayerInMaxAgroRange = _entity.CheckPlayerInMaxAgroRange();

        _isDetectingLedge = _entity.CheckLedge();

        _performShortRangeAction = _entity.CheckPlayerInShortRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        _performLongRangeAction = false;
        _entity.SetVelocity(0f);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= _startTime + _stateData._longRangeActionTime)
        {
            _performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
