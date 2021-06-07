using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_RangedAttackState : RangedAttackState
{
    private Enemy2 _enemy;
    public E2_RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedAttackState stateData, Enemy2 enemy) : 
        base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
        _entity.AM.Play("RangedEnemyProjectile");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAnimationFinished)
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
