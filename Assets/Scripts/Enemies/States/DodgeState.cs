using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{

    protected D_DodgeState _stateData;

    protected bool _performShortRangeAction;
    protected bool _isPlayerInMaxArgoRange;
    protected bool _isGrounded;
    protected bool _isDodgeOver;

    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : 
        base(entity, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _performShortRangeAction = _entity.CheckPlayerInShortRangeAction();
        _isPlayerInMaxArgoRange = _entity.CheckPlayerInMaxAgroRange();
        _isGrounded = _entity.CheckGround();
    }

    public override void Enter()
    {
        base.Enter();

        _isDodgeOver = false;

        _entity.SetVelocity(_stateData._dodgeSpeed, _stateData._dodgeAngle, -_entity._facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= _startTime + _stateData._dodgeTime && _isGrounded)
        {
            _isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
