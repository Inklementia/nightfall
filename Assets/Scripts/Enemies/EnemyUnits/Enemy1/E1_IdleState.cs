using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_IdleState : IdleState
{
    private Enemy1 _enemy;

    public E1_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enemy1 enemy) : 
        base(entity, stateMachine, animBoolName, stateData)
    { 
        _enemy = enemy;
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
        else if (_isIdleTimeOver)
        {
            _stateMachine.ChangeState(_enemy._moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
