using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected bool _grabInput;
    protected bool _jumpInput;
    protected bool _isTouchingLedge;
    protected bool _isMovingPlatform;

    protected int _xInput;
    protected int _yInput;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _isGrounded = _player.CheckIfGrounded();
        _isTouchingWall = _player.CheckIfTouchingWall();
        _isTouchingLedge = _player.CheckIfTouchingLedge();
        _isMovingPlatform = _player.CheckIfTouchingMovingPlatform();

        if (_isTouchingWall && !_isTouchingLedge)
        {
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }
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

        _xInput = _player.InputHandler.NormInputX;
        _yInput = _player.InputHandler.NormInputY;
        _grabInput = _player.InputHandler.GrabInput;
        _jumpInput = _player.InputHandler.JumpInput;

        //
        if (_jumpInput)
        {
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        else if (_isGrounded && !_grabInput)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        else if(!_isTouchingWall || (_xInput != _player.FacingDirection && !_grabInput))//if we are not near wall, not facing wall and doesnt hold ctrl
        {
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if(_isTouchingWall && !_isTouchingLedge && !_isMovingPlatform)
        {
            _stateMachine.ChangeState(_player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
