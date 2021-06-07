using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_PlayerDetectedState : PlayerDetectedState
{
    private Enemy2 _enemy;

    public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Enemy2 enemy) : 
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

        if (_performShortRangeAction)
        {
            if(Time.time >= _enemy._dodgeState._startTime + _enemy._dodgeStateData._dodgeCooldown)
            {
                _stateMachine.ChangeState(_enemy._dodgeState);
            }
            else
            {
                _stateMachine.ChangeState(_enemy._meleeAttackState);
            }
            
        }
        else if (_performLongRangeAction)
        {
            _stateMachine.ChangeState(_enemy._rangedAttackState);
        }
        else if (!_isPlayerInMaxAgroRange)
        {
            _stateMachine.ChangeState(_enemy._lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
