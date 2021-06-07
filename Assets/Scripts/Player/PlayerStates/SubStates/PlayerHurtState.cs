using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerState
{
    public bool Knockback { get; private set; }

    public float KnockbackStartTime { get; private set; }

    public PlayerHurtState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }
    public void CheckKnockback()
    {
        if (Time.time >= KnockbackStartTime + _playerData.KnockbackDuration)
        {
            Knockback = false;
            _player.SetVelocityX(0.0f);
        }
    }

    public override void Enter()
    {
        base.Enter();

        Knockback = true;
        KnockbackStartTime = Time.time;
        //_player.SetVelocity(_playerData.KnockbackVeclocity, _playerData.KnockbackAngle, direction);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
