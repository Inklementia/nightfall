using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 _detectedPos; // for saving initial player position when ledge is detected
    private Vector2 _cornerPos; // г -> corner of the ledge
    private Vector2 _startPos;
    private Vector2 _stopPos;

    private bool _isHanging;
    private bool _isClimbing;
    private bool _jumpInput;


    private int _xInput;
    private int _yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) :
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        _player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        _isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        _player.SetVelocityZero();
        _player.transform.position = _detectedPos;
        _cornerPos = _player.DetermineCornerPosition();

        //basically tracking position of player when he climbs on the ledge
        _startPos.Set(_cornerPos.x - (_player.FacingDirection * _playerData.StartOffset.x), _cornerPos.y - _playerData.StartOffset.y);
        _stopPos.Set(_cornerPos.x + (_player.FacingDirection * _playerData.StopOffset.x), _cornerPos.y + _playerData.StopOffset.y);

        _player.transform.position = _startPos;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if (_isClimbing)
        {
            _player.transform.position = _stopPos;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (_isAnimationFinished)
        {
            //we are at the top of the ledge
            _stateMachine.ChangeState(_player.IdleState);
        }
        else
        {

            _xInput = _player.InputHandler.NormInputX;
            _yInput = _player.InputHandler.NormInputY;
            _jumpInput = _player.InputHandler.JumpInput;

            _player.SetVelocityZero();
            _player.transform.position = _startPos;

            if (_xInput == _player.FacingDirection && _isHanging && !_isClimbing)
            {
                _isClimbing = true;
                _player.Anim.SetBool("climbLedge", true);
            }
            else if (_yInput == -1 && _isHanging && !_isClimbing)
            {
                _stateMachine.ChangeState(_player.InAirState);
            }
            else if (_jumpInput && !_isClimbing)
            {
                _player.WallJumpState.DetermineWallJumpDirection(true); //assuming we know where we are hanging (we know there is a wall in front of us)
                _stateMachine.ChangeState(_player.WallJumpState);
            }

        }

    }

    public void SetDetectedPosition(Vector2 pos)
    {
        _detectedPos = pos;
    }

}
