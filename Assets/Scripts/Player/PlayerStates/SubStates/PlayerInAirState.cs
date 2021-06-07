using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private int _xInput;

    private bool _dashInput;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _grabInput;

    //Checks

    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _isTouchingLedge;
    private bool _isMovingPlatform;

    private bool _coyoteTime; // time, when you can jump eventhough you are not on the ground
    private bool _wallJumpCoyoteTime;
    private bool _isJumping;

    private float _startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        _isGrounded = _player.CheckIfGrounded();
        _isTouchingWall = _player.CheckIfTouchingWall();
        _isTouchingWallBack = _player.CheckIfTouchingWallBack();
        _isTouchingLedge = _player.CheckIfTouchingLedge();
        _isMovingPlatform = _player.CheckIfTouchingMovingPlatform();

        //if Player detects wall but not detects ledge -> there is a ledge to grab
        if (_isTouchingWall && !_isTouchingLedge)
        {
            //when player detects a ledge -> save players position
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }

        if(!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWall = false;
        _isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        _xInput = _player.InputHandler.NormInputX;
        _jumpInput = _player.InputHandler.JumpInput;
        _jumpInputStop = _player.InputHandler.JumpInputStop;
        _grabInput = _player.InputHandler.GrabInput;
        _dashInput = _player.InputHandler.DashInput;

        CheckJumpMultiplier();

       if (_isGrounded && _player.CurrentVelocity.y < 0.01f) // if player landed -> do landstate (animation)
        {
            _stateMachine.ChangeState(_player.LandState);
        }
        else if(_isTouchingWall && !_isTouchingLedge && !_isGrounded && !_isMovingPlatform) //if Player detects wall but not detects ledge -> there is a ledge to grab
        {
            _stateMachine.ChangeState(_player.LedgeClimbState);
           
        }
        else if(_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            _isTouchingWall = _player.CheckIfTouchingWall();

            //to determine which direction we should jump 
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            //WALL JUMP
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        else if (_jumpInput && _player.JumpState.CanJump())
        {
            _coyoteTime = false;
            _stateMachine.ChangeState(_player.JumpState);
        }
        else if (_isTouchingWall && _grabInput && _isTouchingLedge)
        {
            //if player presses ctrl and near the wall
            _stateMachine.ChangeState(_player.WallGrabState);
        }
        else if (_isTouchingWall && _xInput == _player.FacingDirection && _player.CurrentVelocity.y <= 0) 
        {
            // if player touches the wall and presses keys towards the wall he will slide
            _stateMachine.ChangeState(_player.WallSlideState);
        }
        else if (_dashInput && _player.DashState.CheckIfCanDash())
        {
            _stateMachine.ChangeState(_player.DashState);
        }
        else
        {
            _player.CheckIfShouldFlip(_xInput);
            //if we are IN the air we still wanna move in the direction of player input
            _player.SetVelocityX(_playerData.MovementVelocity * _xInput);


            //for blend tree
            _player.Anim.SetFloat("yVelocity", _player.CurrentVelocity.y);
            _player.Anim.SetFloat("xVelocity", Mathf.Abs(_player.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    // to slow down player when he stoped input
    private void CheckJumpMultiplier()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                _player.SetVelocityY(_player.CurrentVelocity.y * _playerData.variableJumpHeightMultiplier);
                _isJumping = false;
            }
            else if (_player.CurrentVelocity.y <= 0f)
            {
                _isJumping = false;
            }
        }
    }

    //when player leave the ledge, he still can jump for some time
    private void CheckCoyoteTime()
    {
        if(_coyoteTime && Time.time > _startTime + _playerData.coyoteTime)
        {
            _coyoteTime = false;
            _player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + _playerData.coyoteTime)
        {
            _wallJumpCoyoteTime = false;
        }
    }

    // this starts this timer
    public void StartCoyoteTime() => _coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;

    public void SetIsJumping()
    {
        _isJumping = true;
    }
}
