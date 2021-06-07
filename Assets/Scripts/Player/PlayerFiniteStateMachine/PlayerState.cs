using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class => means all states gonna inherit from here
public class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _stateMachine;
    protected PlayerData _playerData;

    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    protected float _startTime; // to reference for how long we were in specific state

    private string _animBoolName;

    //constructor -> whenever we will create a new state it shall have this parameters
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _playerData = playerData;
        _animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        DoSurroundingChecks();
        _player.Anim.SetBool(_animBoolName, true);
        _startTime = Time.time; // when we enter in state -> start timer;

        //Debug.Log(_animBoolName);

        _isAnimationFinished = false;
        _isExitingState = false;
    }
    public virtual void Exit()
    {
        _player.Anim.SetBool(_animBoolName, false);
        _isExitingState = true;
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoSurroundingChecks();
    }
    //to check for walls, ledges, etc
    public virtual void DoSurroundingChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => _isAnimationFinished = true;
}
