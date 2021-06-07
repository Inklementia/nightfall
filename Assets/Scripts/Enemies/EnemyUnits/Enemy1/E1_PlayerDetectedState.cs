using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 _enemy;
    public E1_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Enemy1 enemy) : 
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

        if (_performShortRangeAction)
        {
            //transition to attack state
            _stateMachine.ChangeState(_enemy._meleeAttackState);
        }
        else if (_performLongRangeAction)
        {
            _stateMachine.ChangeState(_enemy._chargeState);
        }
        else if (!_isPlayerInMaxAgroRange)
        {
            _stateMachine.ChangeState(_enemy._lookForPlayerState);
        }
        else if (!_isDetectingLedge)
        {
            _stateMachine.ChangeState(_enemy._lookForPlayerState);
        }

       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
