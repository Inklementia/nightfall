using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState _moveState { get; private set; }
    public E2_IdleState _idleState { get; private set; }
    public E2_PlayerDetectedState _playerDetectedState { get; private set; }
    public E2_MeleeAttackState _meleeAttackState { get; private set; }
    public E2_LookForPlayer _lookForPlayerState { get; private set; }
    public E2_StunState _stunState { get; private set; }
    public E2_DeadState _deadState { get; private set; }
    public E2_DodgeState _dodgeState { get; private set; }
    public E2_RangedAttackState _rangedAttackState { get; private set; }


    [SerializeField] private D_MoveState _moveStateData;
    [SerializeField] private D_IdleState _idleStateData;
    [SerializeField] private D_PlayerDetectedState _playerDetectedStateData;
    [SerializeField] private D_MeleeAttackState _meleeAttackStateData;
    [SerializeField] private D_LookForPlayerState _lookForPlayerStateData;
    [SerializeField] private D_StunState _stunStateData;
    [SerializeField] private D_DeadState _deadStateData;

    public D_DodgeState _dodgeStateData;

    [SerializeField] private D_RangedAttackState _rangedAttackStateData;

    [SerializeField] private Transform _meleeAttackPosition;
    [SerializeField] private Transform _rangedAttackPosition;

    public override void Start()
    {
        base.Start();

        _moveState = new E2_MoveState(this, _stateMachine, "move", _moveStateData, this);
        _idleState = new E2_IdleState(this, _stateMachine, "idle", _idleStateData, this);
        _playerDetectedState = new E2_PlayerDetectedState(this, _stateMachine, "playerDetected", _playerDetectedStateData, this);
        _meleeAttackState = new E2_MeleeAttackState(this, _stateMachine, "meleeAttack", _meleeAttackPosition, _meleeAttackStateData, this);
        _lookForPlayerState = new E2_LookForPlayer(this, _stateMachine, "lookForPlayer", _lookForPlayerStateData, this);
        _stunState = new E2_StunState(this, _stateMachine, "stun", _stunStateData, this);
        _deadState = new E2_DeadState(this, _stateMachine, "dead", _deadStateData, this);
        _dodgeState = new E2_DodgeState(this, _stateMachine, "dodge", _dodgeStateData, this);
        _rangedAttackState = new E2_RangedAttackState(this, _stateMachine, "rangedAttack", _rangedAttackPosition, _rangedAttackStateData, this);


        _stateMachine.Initialize(_moveState);
    }

    public override void Damage(AttackDetails attackdetails)
    {
        base.Damage(attackdetails);


        if (_isDead)
        {
            _stateMachine.ChangeState(_deadState);
        }
        else if (_isStunned && _stateMachine._currentState != _stunState)
        {
            _stateMachine.ChangeState(_stunState);
        }
        else if (CheckPlayerInMinAgroRange())
        {
            _stateMachine.ChangeState(_rangedAttackState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            _lookForPlayerState.TurnImmediately(true);
            _stateMachine.ChangeState(_lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(_meleeAttackPosition.position, _meleeAttackStateData._attackRadius);
    }
}
