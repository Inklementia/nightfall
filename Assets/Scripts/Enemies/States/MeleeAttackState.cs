using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState _stateData;

    protected AttackDetails _attackDetails;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        _stateData = stateData;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();
    }

    public override void Enter()
    {
        base.Enter();

        _attackDetails.DamageAmount = _stateData._attackDamage;
        _attackDetails.Position = _entity._aliveGo.transform.position;
        _attackDetails.KnockbackVeclocity = _stateData._knockbackVeclocity;
        _attackDetails.KnockbackAngle = _stateData._knockbackAngle;
      
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackPosition.position, _stateData._attackRadius, _stateData._whatIsPlayer);

        foreach (Collider2D colider in detectedObjects)
        {
            colider.transform.SendMessage("Damage", _attackDetails);
        }
    }
}
