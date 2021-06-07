using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState _idleState { get; private set; }
    public E1_MoveState _moveState { get; private set; }
    public E1_PlayerDetectedState _playerDetectedState { get; private set; }
    public E1_ChargeState _chargeState { get; private set; }
    public E1_LookForPlayerState _lookForPlayerState { get; private set; }
    public E1_MeleeAttackState _meleeAttackState { get; private set; }
    public E1_StunState _stunState { get; private set; }
    public E1_DeadState _deadState { get; private set; }

    [SerializeField] private D_IdleState _idleStateData;
    [SerializeField] private D_MoveState _moveStateData;
    [SerializeField] private D_PlayerDetectedState _playerDetectedStateData;
    [SerializeField] private D_ChargeState _chargeStateData;
    [SerializeField] private D_LookForPlayerState _lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState _meleeAttackStateData;
    [SerializeField] private D_StunState _stunStateData;
    [SerializeField] private D_DeadState _deadStateData;

    [SerializeField] private Transform _meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        _moveState = new E1_MoveState(this, _stateMachine, "move", _moveStateData, this);
        _idleState = new E1_IdleState(this, _stateMachine, "idle", _idleStateData, this);
        _playerDetectedState = new E1_PlayerDetectedState(this, _stateMachine, "playerDetected", _playerDetectedStateData, this);
        _chargeState = new E1_ChargeState(this, _stateMachine, "charge", _chargeStateData, this);
        _lookForPlayerState = new E1_LookForPlayerState(this, _stateMachine, "lookForPlayer", _lookForPlayerStateData, this);
        _meleeAttackState = new E1_MeleeAttackState(this, _stateMachine, "meleeAttack", _meleeAttackPosition, _meleeAttackStateData, this);
        _stunState = new E1_StunState(this, _stateMachine, "stun", _stunStateData, this);
        _deadState = new E1_DeadState(this, _stateMachine, "dead", _deadStateData, this);

        _stateMachine.Initialize(_moveState);
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(_meleeAttackPosition.position, _meleeAttackStateData._attackRadius);
    }

    public override void Damage(AttackDetails attackdetails)
    {
        base.Damage(attackdetails);
        _stateMachine.ChangeState(_lookForPlayerState);

        if (_isDead)
        {
            _stateMachine.ChangeState(_deadState);
        }//if we are stunned and we are not yet in StunState then -> stunState
        else if (_isStunned && _stateMachine._currentState != _stunState)
        {
            _stateMachine.ChangeState(_stunState);
        }
        //if dead -> its dead
       
    }
}
