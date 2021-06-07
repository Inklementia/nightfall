using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MoveState : MoveState
{
    private Enemy2 _enemy;
    public E2_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy2 enemy) : 
        base(entity, stateMachine, animBoolName, stateData)
    {
        _enemy = enemy;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isPlayerInMinAgroRange)
        {
            _stateMachine.ChangeState(_enemy._playerDetectedState);
        }
        else if (_isDetectingWall || !_isDetectingLedge)
        {
            _enemy._idleState.SetFlipAfterIdle(true);
            _stateMachine.ChangeState(_enemy._idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
