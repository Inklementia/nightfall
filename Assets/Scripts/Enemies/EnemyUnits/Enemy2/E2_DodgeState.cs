using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DodgeState : DodgeState
{
    private Enemy2 _enemy;
    public E2_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData, Enemy2 enemy) : 
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

        if (_isDodgeOver)
        {
            if(_isPlayerInMaxArgoRange && _performShortRangeAction)
            {
                _stateMachine.ChangeState(_enemy._meleeAttackState);
            }
            else if (_isPlayerInMaxArgoRange && !_performShortRangeAction)
            {
                _stateMachine.ChangeState(_enemy._rangedAttackState);
            }
            else if (!_isPlayerInMaxArgoRange)
            {
                _stateMachine.ChangeState(_enemy._lookForPlayerState);
            }

            //ranged attack state
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
