using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputHandler.UseJumpInput();
        _player.JumpState.ResetAmountOfJumpsLeft();
        _player.SetVelocity(_playerData.WallJumpVelocity, _playerData.WallJumpAngle, _wallJumpDirection);

        //if we jumped and dont face the wall, flip
        _player.CheckIfShouldFlip(_wallJumpDirection);

        _player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _player.Anim.SetFloat("yVelocity", _player.CurrentVelocity.y);
        _player.Anim.SetFloat("xVelocity", Mathf.Abs(_player.CurrentVelocity.x));

        if(Time.time >= _startTime + _playerData.WallJumpTime)
        {
            //ability is done
            _isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            _wallJumpDirection = -_player.FacingDirection;
        }
        else
        {
            _wallJumpDirection = _player.FacingDirection;
        }
    }
}
