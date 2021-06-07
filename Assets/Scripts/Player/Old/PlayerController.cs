using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _movementInputDirection;
    private float _dashTimeLeft;
    private float _lastImageXPos;
    private float _lastDash =-100f;
    private float _knockbackStartTime;

    private int _amountOfJumpsLeft;
    private int _facingDirection = 1; // -1 = left, 1 = right

    private bool _isFacingRight = true;
    private bool _isGrounded;
    private bool _canJump;
    private bool _canMove;
    private bool _canFlip;
    private bool _isTouchingWall;
    private bool _isWallSliding;
   // private bool _isTouchingLedge;
    private bool _canClimbLedge = false;
   // private bool _ledgeDetected;
    private bool _isDashing;
    private bool _knockback;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _anim;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;

    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private int _amountOfJumps = 1;

    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _wallSlidingSpeed = 1.5f;
    [SerializeField] private float _jumpForce = 16;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _movementForceInAir;
    [SerializeField] private float _airDragMultiplier = 0.95f; // no input - slow down
    [SerializeField] private float _variableJumpHeightMultiplier = 0.5f; //no holding jump button - no high jump
    [SerializeField] private float _wallHopForce;
    [SerializeField] private float _wallJumpForce;
    [SerializeField] private float _knockbackDuration;

    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _distanceBetweenImages;
    [SerializeField] private float _dashCooldown;

    [SerializeField] private Vector2 _wallHopDirection;
    [SerializeField] private Vector2 _wallJumpDirection;
    [SerializeField] private Vector2 _knockbackSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        _amountOfJumpsLeft = _amountOfJumps;
        _wallHopDirection.Normalize();
        _wallJumpDirection.Normalize(); // vector = 1
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckDash();
        CheckKnockback();
        //CheckIfLedgeClimb();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void CheckIfWallSliding()
    {
        if (_isTouchingWall && !_isGrounded && _rb.velocity.y < 0 && !_canClimbLedge)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    //wether or not Player is currently dashing 
    public bool GetDashStatus()
    {
        return _isDashing;
    }

    public void KnockBack(int direction)
    {
        _knockback = true;
        _knockbackStartTime = Time.time;
        _rb.velocity = new Vector2(_knockbackSpeed.x * direction, _knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= _knockbackStartTime + _knockbackDuration && _knockback)
        {
            _knockback = false;
            _rb.velocity = new Vector2(0.0f, _rb.velocity.y);
        }
    }

    /*
    private void CheckIfLedgeClimb()
    {
        if (_ledgeDetected && !_canClimbLedge)
        {
            _canClimbLedge = true;
            if (_isFacingRight)
            {
                _ledgePos1 = new Vector2(Mathf.Floor(_ledgePosBot.x + _wallCheckDistance) - _ledgeClimbXOffset1, Mathf.Floor(_ledgePosBot.y) + _ledgeClimbYOffset1);
                _ledgePos2 = new Vector2(Mathf.Floor(_ledgePosBot.x + _wallCheckDistance) + _ledgeClimbXOffset2, Mathf.Floor(_ledgePosBot.y) + _ledgeClimbYOffset2);
            }
            else
            {
                _ledgePos1 = new Vector2(Mathf.Ceil(_ledgePosBot.x - _wallCheckDistance) + _ledgeClimbXOffset1, Mathf.Floor(_ledgePosBot.y) + _ledgeClimbYOffset1);
                _ledgePos2 = new Vector2(Mathf.Ceil(_ledgePosBot.x - _wallCheckDistance) - _ledgeClimbXOffset2, Mathf.Floor(_ledgePosBot.y) + _ledgeClimbYOffset2);
            }

            _canMove = false;
            _canFlip = false;

            _anim.SetBool("canClimbLedge", _canClimbLedge);
            if (_canClimbLedge)
            {
                transform.position = _ledgePos1;
            }
        }
    }

    public void FinishLedgeClimb()
    {
        _canClimbLedge = false;
        transform.position = _ledgePos2;
        _canMove = true;
        _canFlip = true;
        _ledgeDetected = false;
        _anim.SetBool("canClimbLedge", _canClimbLedge);
    }
    */
    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);
        _isTouchingWall = Physics2D.Raycast(_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
        //_isTouchingLedge = Physics2D.Raycast(_ledgeCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
        /*
        if (_isTouchingWall && !_isTouchingLedge && !_ledgeDetected)
        {
            _ledgeDetected = true;
            _ledgePosBot = _wallCheck.position;
        }*/
    }

    private void CheckIfCanJump()
    {
        if ((_isGrounded && _rb.velocity.y <= 0) || _isWallSliding)
        {
            _amountOfJumpsLeft = _amountOfJumps;
            _canJump = true;
        }

        if (_amountOfJumpsLeft <= 0)
        {
            _canJump = false;
        }
        else
        {
            _canJump = true;
        }
    }

    private void CheckMovementDirection()
    {
        if (_isFacingRight && _movementInputDirection < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && _movementInputDirection > 0)
        {
            Flip();
        }
    }

    private void UpdateAnimations()
    {
        _anim.SetInteger("isWalking", (int)_movementInputDirection);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetFloat("yVelocity", _rb.velocity.y);
        _anim.SetBool("isWallSliding", _isWallSliding);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (!_canMove)
        {
            _canMove = true;
            _canFlip = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _variableJumpHeightMultiplier);
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (_lastDash + _dashCooldown))
            {
                AttemptToDash();
            }
        }
    }
    private void AttemptToDash()
    {
        _isDashing = true;
        _dashTimeLeft = _dashTime;
        _lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        _lastImageXPos = transform.position.x;
    }

    //for other scripts
    public int GetFacingDirection()
    {
        return _facingDirection;
    }
    private void CheckDash()
    {
        if (_isDashing)
        {
            if(_dashTimeLeft > 0)
            {
                _canMove = false;
                _canFlip = false;
                _rb.velocity = new Vector2(_dashSpeed * _facingDirection, 0);
                _dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - _lastImageXPos) > _distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    _lastImageXPos = transform.position.x;
                }
            }
            if(_dashTimeLeft <= 0 || _isTouchingWall)
            {
                _isDashing = false;
                _canMove = true;
                _canFlip = true;
            }
        }
    }
    private void Jump()
    {
        if (_canJump && !_isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _amountOfJumpsLeft--;
        }
        else if (_isWallSliding && _movementInputDirection == 0 && _canJump) // wall hop
        {
            _isWallSliding = false;
            _amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(_wallHopForce * _wallHopDirection.x * -_facingDirection, _wallHopForce * _wallHopDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);

        }
        else if ((_isWallSliding || _isTouchingWall) && _movementInputDirection != 0 && _canJump) // wall jump
        {
            _isWallSliding = false;
            _amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(_wallJumpForce * _wallJumpDirection.x * _movementInputDirection, _wallJumpForce * _wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }
    private void ApplyMovement()
    {
        if (_isGrounded && _canMove && !_knockback)
        {
            _rb.velocity = new Vector2(_movementSpeed * _movementInputDirection, _rb.velocity.y);
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection != 0 && !_knockback)
        {
            Vector2 forceToAdd = new Vector2(_movementForceInAir * _movementInputDirection, 0);
            _rb.AddForce(forceToAdd);

            if (Mathf.Abs(_rb.velocity.x) > _movementSpeed && _canMove)
            {
                _rb.velocity = new Vector2(_movementSpeed * _movementInputDirection, _rb.velocity.y);
            }
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0 && !_knockback)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * _airDragMultiplier, _rb.velocity.y);
        }

        if (_isWallSliding)
        {
            if (_rb.velocity.y < -_wallSlidingSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlidingSpeed);
            }
        }
    }

    public void DisableFlip()
    {
        _canFlip = false;
    }
    public void EnableFlip()
    {
        _canFlip = true;
    }

    private void Flip()
    {
        if (!_isWallSliding && _canFlip && !_knockback)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }

    //gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y, _wallCheck.position.z));
    }   

}
