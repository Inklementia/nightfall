using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState _stateData;

    protected bool _isStunTimeOver;
    protected bool _isGrounded;
    protected bool _isMovementStopped;
    protected bool _performShortRangeAction;
    protected bool _isPlayerInMinAgroRange;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _isGrounded = _entity.CheckGround();
        _performShortRangeAction = _entity.CheckPlayerInShortRangeAction();
        _isPlayerInMinAgroRange = _entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        _entity.SetStunResistanceZero();
        _isStunTimeOver = false;
        _isMovementStopped = false;
        _entity.SetVelocity(_stateData._stunKnockbackSpeed, _stateData._stunKnockbackAngle, _entity._lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();

        _entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= _startTime + _stateData._stunTime)
        {
            _isStunTimeOver = true;
        }

        if (_isGrounded && Time.time > _startTime + _stateData._stunKnockbackTime && !_isMovementStopped) 
        {
            //to make velocity = 0 only once
            _isMovementStopped = true;
            _entity.SetVelocity(0f);
           
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
