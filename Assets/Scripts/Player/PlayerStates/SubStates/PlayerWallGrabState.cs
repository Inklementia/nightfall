using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition; // to make player not moving wwhen he grabs a wall
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : 
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //when we enter the state we will stay on the same spot until we release the hold key (ctrl)
        _holdPosition = _player.transform.position;

        HoldPosition();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!_isExitingState)
        {

            HoldPosition();

            if (_yInput > 0)
            {
                //if we press UP we will wall climb
                _stateMachine.ChangeState(_player.WallClimbState);
            }
            else if (_yInput < 0 || !_grabInput)// if we hold DOWN button and NOT ctrl we will wall slide
            {
                _stateMachine.ChangeState(_player.WallSlideState);
            }
        }
    }

    private void HoldPosition()
    {
        _player.transform.position = _holdPosition;
        
        //to keep camera centered on our player
        _player.SetVelocityX(0);
        _player.SetVelocityY(0);
    }
}
