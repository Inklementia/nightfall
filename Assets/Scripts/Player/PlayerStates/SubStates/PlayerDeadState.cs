using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{

    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        _player.AM.Play("Splash");

        GameObject.Instantiate(_playerData.BloodSplash, _player.ReturnGroundCheck().position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        GameObject.Instantiate(_playerData.DeathBloodParticles, _player.transform.position, _playerData.DeathBloodParticles.transform.rotation);
       //GameObject.Instantiate(_playerData.DeathChunkParticles, _player.transform.position, _playerData.DeathChunkParticles.transform.rotation);
        _player.KillAndRespawn();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        DeathTimesTracker.Deaths += 1;

        if (_player.transform.parent != null && _player.transform.parent.parent != null)
        {
            _player.transform.SetParent(null);
        }
    }

    public override void Exit()
    {
        base.Exit();
  
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _player.CheckKnockback();
        if (_isAnimationFinished)
        {
            _stateMachine.ChangeState(_player.SpawnState);

        }
    }
  
}
