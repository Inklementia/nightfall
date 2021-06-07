using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_StunState : StunState
{
    private Enemy1 _enemy;

    public E1_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy1 enemy) : 
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
            if (_performShortRangeAction)
            {
                _stateMachine.ChangeState(_enemy._meleeAttackState);
            }
            else if (_isPlayerInMinAgroRange)
            {
                _stateMachine.ChangeState(_enemy._chargeState);
            }
            else
            {
                _enemy._lookForPlayerState.TurnImmediately(true);
                _stateMachine.ChangeState(_enemy._lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
