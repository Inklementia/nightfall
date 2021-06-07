using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnState : PlayerState
{
    private float _orignalGravity;
    public PlayerSpawnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

       // _player.FinishRespawn();
        _stateMachine.ChangeState(_player.InAirState);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        _orignalGravity = _player.RB.gravityScale;
        _player.SetVelocityZero();
        _player.RB.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
      
        _player.RB.gravityScale = _orignalGravity;
        _player.IdleState.SetCanDie(true);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
