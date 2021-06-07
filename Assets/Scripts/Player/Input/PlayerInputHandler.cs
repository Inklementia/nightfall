using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _cam; // to get cursor position

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }// to set certain angles player can dash: 90, 45, 180

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public bool SelfDestruct { get; private set; }
    public bool SelfDestructInput { get; private set; }
    public bool SelfDestructInputStop { get; private set; }
    public bool ActionInput { get; private set; }
    public bool TalkInput { get; private set; }

    [SerializeField] private float _inputHoldTime = 0.2f;
    [SerializeField] private float _selfDesctructHoldTime = 0.5f;

    private float _jumpInputStartTime;
    private float _dashInputStartTime;
    private float _attackInputStartTime;
    private float _selfDestructInputStartTime;


    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckSelfDesctructHoldTime();
 
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>(); // with gamepad it gives us values between 0 and 1
        if (Mathf.Abs(RawMovementInput.x) > 0.5f)
        {
            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x; // will give normalized valies x of 0 or 1
        }
        else
        {
            NormInputX = 0;
        }
        if (Mathf.Abs(RawMovementInput.y) > 0.5f)
        { 
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y; // will give normalized valies y of 0 or 1
        }
        else {
            NormInputY = 0;
        }
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        //when space or X (gamepad) is pressed 
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }
        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            _dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(_playerInput.currentControlScheme  == "Keyboard" && _cam)
        {
            //taking current position of mouse from camera - player.position
            RawDashDirectionInput = _cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }
        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized); // if it 2 will do 0, if it 42 will do 45
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }
        if (context.canceled)
        {
            AttackInput = false;
        }
    }
    
    public void OnSelfDestructInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SelfDestruct = false;
            SelfDestructInput = true;
            SelfDestructInputStop = false;
            _selfDestructInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            SelfDestructInputStop = true;
            SelfDestruct = false;
        }
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActionInput = true;
        }
        if (context.canceled)
        {
            ActionInput = false;
        }
    }
    public void OnTalkInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TalkInput = true;
        }
        if (context.canceled)
        {
            TalkInput = false;
        }
    }

    //in methods above we set some bools to true and here we set them to false -> easy peasy
    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseActionInput() => ActionInput = false;
    public void UseTalkInput() => TalkInput = false;
    public void UseSelfDesctructInput() => SelfDestructInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= _jumpInputStartTime + _inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= _dashInputStartTime + _inputHoldTime)
        {
            DashInput = false;
        }
    }
    // you need to press long enough to kill your self
    // this apparently dont work, in Player, in this script increase hold time (now it is 0) aand if you will hold Z for some time, 
    // it will indead destroy player but if after you quickly press Z again he will destroy himself non-stop
    private void CheckSelfDesctructHoldTime()
    {
        if (SelfDestructInput && Time.time >= _selfDestructInputStartTime + _selfDesctructHoldTime)
        {
            SelfDestructInput = false;
            SelfDestruct = true;
           
        }
    }
}
