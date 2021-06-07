using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform _attackPosition;

    protected bool _isAnimationFinished;
    protected bool _isPlayerInMinAgroRange;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition) : 
        base(entity, stateMachine, animBoolName)
    {
        _attackPosition = attackPosition;
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _isPlayerInMinAgroRange = _entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        _entity._ATSM._attackState = this;
        _isAnimationFinished = false;
        _entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    //to call from animations
    public virtual void TriggerAttack()//to do damage/fire projectile
    {

    }

    public virtual void FinishAttack()//when attack is done
    {
        _isAnimationFinished = true;
    }
}
