using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{

    private AttackDetails _attackDetails; // struct with attack details

    public bool CombatEnabled{ get; private set; }
    public bool CanAttack { get; private set; }

    private bool _isAttacking;
    private bool _isFirstAttack;
    private bool _attackInput;
 

    private float _lastAttackTime;


    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        CanAttack = false;
        _startTime = Time.time; // to keep track of normal time;
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAnimationFinished)
        {
            //we are at the top of the ledge
            _stateMachine.ChangeState(_player.IdleState);
        }
        else
        {
            _attackInput = _player.InputHandler.AttackInput;

            if (_attackInput && !_isAttacking)
            {
                _isAttacking = true;
                _isFirstAttack = !_isFirstAttack;
                _player.RB.drag = _playerData.AttackDrag; //to slow down player when he attack
                _player.Anim.SetBool("firstAttack", _isFirstAttack);
                

            }

        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        _isAttacking = false;
        _lastAttackTime = Time.time; // for cooldown   
        _player.RB.drag = 0f;


    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        if (_isFirstAttack)
        {
            _player.AM.Play("PlayerAttack1");
        }
        else
        {
            _player.AM.Play("PlayerAttack2");
        }
       

        Collider2D[] detectedObjects = _player.CheckDetectableObjects();

        _attackDetails.DamageAmount = _playerData.DamageAmount;
        _attackDetails.Position = _player.transform.position;
        _attackDetails.StunDamageAmount = _playerData.StunDamageAmount;
        _attackDetails.KnockbackVeclocity = _playerData.KnockbackVelocity;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", _attackDetails);
            //Instanciate hit particles
            //will instanciate in different script so enemies will have different particles
        }
    }


    public bool CheckIfCanAttack()
    {
        return CanAttack && Time.time >= _lastAttackTime + _playerData.AttackCooldown;
    }
    public void ResetCanAttack() => CanAttack = true;
    public void EnableCombat() => CombatEnabled = true;
}
