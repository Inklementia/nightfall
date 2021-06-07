using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState _stateData;

    protected bool _isPlayerInMinAgroRange;
    protected bool _isDetectingWall;
    protected bool _isDetectingLedge;
    protected bool _isChargeTimeOver;
    protected bool _performShortRangeAction;

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _isPlayerInMinAgroRange = _entity.CheckPlayerInMinAgroRange();
        _isDetectingLedge = _entity.CheckLedge();
        _isDetectingWall = _entity.CheckWall();

        _performShortRangeAction = _entity.CheckPlayerInShortRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        _isChargeTimeOver = false;
        _entity.SetVelocity(_stateData._chargeSpeed); //increase speed for some time 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= _startTime + _stateData._chargeTime){
            _isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
