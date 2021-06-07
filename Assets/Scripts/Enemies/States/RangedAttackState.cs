using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected D_RangedAttackState _stateData;

    protected GameObject _projectile;
    protected Projectile _projectileScript;

    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : 
        base(entity, stateMachine, animBoolName, attackPosition)
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

        _projectile = GameObject.Instantiate(_stateData._projectile, _attackPosition.position, _attackPosition.rotation);
        _projectileScript = _projectile.GetComponent<Projectile>();
        _projectileScript.FireProjectile(_stateData._projectileSpeed, _stateData._projectileTravelDistance, _stateData._projectileDamage, _stateData._knockbackAngle, _stateData._knockbackVeclocity);
    }
}
