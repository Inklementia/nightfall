using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
        _amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputHandler.UseJumpInput(); // set jump input to false
        _player.SetVelocityY(_playerData.JumpVelocity);
        _isAbilityDone = true;
        _amountOfJumpsLeft--;
        _player.InAirState.SetIsJumping();
    }
    public bool CanJump()
    {
        if (_amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft()
    {
        _amountOfJumpsLeft--;
    }
}
