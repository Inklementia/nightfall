using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;
    private bool _jumpInput;
    private bool _grabInput;
    private bool _dashInput;
    private bool _attackInput;
    private bool _selfDesctuct;
    private bool _selfDesctuctInput;

    private bool _IsGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingLedge;

    public bool CanDie { get; private set; }

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _IsGrounded = _player.CheckIfGrounded();
        _isTouchingWall = _player.CheckIfTouchingWall();
        _isTouchingLedge = _player.CheckIfTouchingLedge();
    }

    public override void Enter()
    {
        base.Enter();

        //whenever we touch the ground we reset the ability to dash and jumps
        _player.JumpState.ResetAmountOfJumpsLeft();
        _player.DashState.ResetCanDash();
        _player.AttackState.ResetCanAttack();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _player.CheckKnockback();

        _xInput = _player.InputHandler.NormInputX;
        _jumpInput = _player.InputHandler.JumpInput;
        _grabInput = _player.InputHandler.GrabInput;
        _dashInput = _player.InputHandler.DashInput;
        _attackInput = _player.InputHandler.AttackInput;
        _selfDesctuct = _player.InputHandler.SelfDestruct;
        _selfDesctuctInput = _player.InputHandler.SelfDestructInput;

        if (_jumpInput && _player.JumpState.CanJump())
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
        else if (!_IsGrounded)
        {
            _player.InAirState.StartCoyoteTime();
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if (_isTouchingWall && _grabInput && _isTouchingLedge)
        {
            _stateMachine.ChangeState(_player.WallGrabState);
        }
        else if (_dashInput && _player.DashState.CheckIfCanDash())
        {
            _stateMachine.ChangeState(_player.DashState);
        }
        else if (_attackInput && _player.AttackState.CombatEnabled && _player.AttackState.CheckIfCanAttack())
        {
            _stateMachine.ChangeState(_player.AttackState);
        }
        else if (_selfDesctuct && !_selfDesctuctInput && CanDie)
        {
            CanDie = false;
            _player.InputHandler.UseSelfDesctructInput();
            _stateMachine.ChangeState(_player.DeadState);
            DeathTimesTracker.DeathsFromSelfDesctruct += 1;
           
        }

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void SetCanDie(bool canDie)
    {
        CanDie = canDie;
    }
}
