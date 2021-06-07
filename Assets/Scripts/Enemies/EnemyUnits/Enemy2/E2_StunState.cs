using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_StunState : StunState
{
    private Enemy2 _enemy;
    public E2_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy2 enemy) : 
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

        if (_isStunTimeOver)
        {
            if (_isPlayerInMinAgroRange)
            {
                _stateMachine.ChangeState(_enemy._playerDetectedState);
            }
            else
            {
                _stateMachine.ChangeState(_enemy._lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
