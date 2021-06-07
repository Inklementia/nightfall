using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private bool _isHolding; //user holds dash button 
    private bool _dashInputStop;
 
    private float _lastDashTime;

    private Vector2 _dashDirection;
    private Vector2 _dashDirectionInput;
    private Vector2 _lastAfterImagePosition;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        _player.InputHandler.UseDashInput(); //we already used dashInput;

        _isHolding = true;
        _dashDirection = Vector2.right * _player.FacingDirection; // by default will dash where player looks

        Time.timeScale = _playerData.HoldTimeScale;
        _startTime = Time.unscaledTime; // to keep track of normal time;

        _player.DashDirectionIndicator.gameObject.SetActive(true); // to make dashIndicator visible when we enter dash state
    }

    public override void Exit()
    {
        base.Exit();

        //to slow down when we end dash and fly Up
        if(_player.CurrentVelocity.y > 0)
        {
            _player.SetVelocityY(_player.CurrentVelocity.y * _playerData.DashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState)
        {
            //when we press and hold dash button
            if (_isHolding)
            {
                _dashDirectionInput = _player.InputHandler.DashDirectionInput;
                _dashInputStop = _player.InputHandler.DashInputStop;

                //if there is an input
                if (_dashDirectionInput != Vector2.zero)
                {
                    //set it and normalize the angle
                    _dashDirection = _dashDirectionInput;
                    _dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, _dashDirection); // returns angle for indicator
                _player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f); //substraction -> because sprite is at 45 by default
            
                //if player unpressed the dash button or dash time run out
                if(_dashInputStop || Time.unscaledTime >= _startTime + _playerData.MaxHoldTime)
                {
                    //dash
                    _isHolding = false;
                    Time.timeScale = 1;
                    _startTime = Time.time;
                    _player.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x)); // flip character in the direction of dash
                    _player.RB.drag = _playerData.Drag; // how air density affect object velocity 
                    _player.SetVelocity(_playerData.DashVelocity, _dashDirection); 
                    _player.DashDirectionIndicator.gameObject.SetActive(false); // to hide Indicator
                    PlaceAfterImage();
                }

            }
            else
            {
                _player.SetVelocity(_playerData.DashVelocity, _dashDirection);
                CheckIfShouldPlaceAfterImage();

                if (Time.time >= _startTime + _playerData.DashTime)
                {
                    _player.RB.drag = 0f;
                    _isAbilityDone = true;
                    _lastDashTime = Time.time; // for cooldown
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(_player.transform.position, _lastAfterImagePosition) >= _playerData.DistanceBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        _lastAfterImagePosition = _player.transform.position;
    }

    //other functions
    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= _lastDashTime + _playerData.DashCooldown && !_player.Knockback;
    }
    public void ResetCanDash() => CanDash = true;

    public void SetCanDash(bool canDash)
    {
        CanDash = canDash;
    }

}
