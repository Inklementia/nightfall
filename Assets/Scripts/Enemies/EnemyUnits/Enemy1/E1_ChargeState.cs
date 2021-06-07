using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 _enemy;

    public E1_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Enemy1 enemy) : 
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

        // transisiton to attack state
        if (_performShortRangeAction)
        {
            _stateMachine.ChangeState(_enemy._meleeAttackState);
        }
        else if (!_isDetectingLedge)
        {
            _stateMachine.ChangeState(_enemy._lookForPlayerState);

        }
        /*
        else if (_isDetectingWall)
        {
            _stateMachine.ChangeState(_enemy._stunState);
        }*/
        else if (_isChargeTimeOver)
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
