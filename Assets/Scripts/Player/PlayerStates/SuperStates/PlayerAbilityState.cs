using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone; // to track if any ability (jump, dash etc) is done and we can transition to other state
    private bool _isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoSurroundingChecks()
    {
        base.DoSurroundingChecks();
        _isGrounded = _player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAbilityDone)// if ability is done (jump, dash)
        {
            if (_isGrounded && _player.CurrentVelocity.y < 0.01f)
            {
                // if it is on the ground -> do idle state
                _stateMachine.ChangeState(_player.IdleState);
               
            }
            else
            {
                _stateMachine.ChangeState(_player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
