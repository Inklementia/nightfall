using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerSpawnState SpawnState { get; private set; }

    [SerializeField] private PlayerData _playerData;
    #endregion

    #region Components

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; } // script that handles Input System
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public GameManager GM { get; private set; }
    public AudioManager AM { get; private set; }

    private HealthBar _healthBar;

    #endregion

    #region Check Transforms

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _attackHitbox;

    #endregion

    #region Other Variables

    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 _workSpace; // whenever we will set new Velocity, this variable will hold its value

    private float _currentHealth;

    public bool Knockback { get; private set; }

    public float KnockbackStartTime { get; private set; }

    #endregion

    #region Unity Callback functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        SpawnState = new PlayerSpawnState(this, StateMachine, _playerData, "spawn");
        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, _playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, _playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, _playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, _playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, _playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, _playerData, "inAir");
        AttackState = new PlayerAttackState(this, StateMachine, _playerData, "attackState");
        DeadState = new PlayerDeadState(this, StateMachine, _playerData, "dead");

    }
    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
 
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        _currentHealth = _playerData.InitialHealth;
        _healthBar.SetMaxHealth(_playerData.MaxHealth);

        FacingDirection = 1; // at the start of the game we facing RIGHT (right 1, left -1)
        StateMachine.Initialize(IdleState); // when game starts Player goes into idle state :D
    
    }
    private void Update()
    {
 
        CurrentVelocity = RB.velocity; // to get current velocity at the start of every frame
       
        StateMachine.CurrentState.LogicUpdate();
        _healthBar.SetHealth(_currentHealth);
       
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions

    //set velocity to 0
    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    // for wall jump and mb smth else
    public void SetVelocity(float velocity, Vector2 angle, int direction) // 1 is right, -1 is left
    {
        angle.Normalize();
        _workSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    public void SetVelocity(float velocity, Vector2 direction) // for dash
    {
        _workSpace = direction * velocity;
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    // for any movement over X
    public void SetVelocityX(float velocity)
    {
        _workSpace.Set(velocity, CurrentVelocity.y);
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }
    // for any movement over Y
    public void SetVelocityY(float velocity)
    {
        _workSpace.Set(CurrentVelocity.x, velocity);
        RB.velocity = _workSpace;
        CurrentVelocity = _workSpace;
    }

    #endregion

    #region Check Functions

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _playerData.GroundCheckRadius, _playerData.WhatIsGround);
    }

    //to check the wall in front of the player
    public bool CheckIfTouchingWall() {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    //to check the wall behind the player
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * -FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingMovingPlatform()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance * 2, _playerData.WhatIsGround);

        if (hitInfo)
        {
            if (hitInfo.collider.tag == "MovingPlatform")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }           
    } 

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection && !Knockback)
        {
            Flip();
        }
    }

    public Collider2D[] CheckDetectableObjects()
    {
        return Physics2D.OverlapCircleAll(_attackHitbox.position, _playerData.AttackHitboxRadius, _playerData.WhatIsDamageable);
    }

    public void CheckKnockback()
    {
        if (Time.time >= KnockbackStartTime + _playerData.KnockbackDuration)
        {
            Knockback = false;
            SetVelocityZero();

        }
    }


  
    #endregion

    #region Other Functions
    private void Damage(AttackDetails attackDetails)
    {
    
         int direction;
        AM.Play("Hurt");
         DeacreaseHealth(attackDetails.DamageAmount);
         Instantiate(_playerData.HitParticles, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (attackDetails.Position.x < transform.position.x)
         {
             direction = 1; //right 
         }
         else
         {
             direction = -1; //left
         }
  
         Knockback = true;
         KnockbackStartTime = Time.time;
         SetVelocity(attackDetails.KnockbackVeclocity, attackDetails.KnockbackAngle, direction);
        
    }



    public Vector2 DetermineCornerPosition()
    {
        //horizontal ray
        RaycastHit2D xHit = Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);

        float xDistance = xHit.distance; // distance from Raycast origin to the object we detected (ledge/platform)
        _workSpace.Set(xDistance * FacingDirection, 0f);

        RaycastHit2D yHit = Physics2D.Raycast(_ledgeCheck.position + (Vector3)(_workSpace), Vector2.down, _ledgeCheck.position.y - _wallCheck.position.y, _playerData.WhatIsGround);
        float yDistance = yHit.distance;

        _workSpace.Set(_wallCheck.position.x + (xDistance * FacingDirection), _ledgeCheck.position.y - yDistance);

        return _workSpace;
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }


    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180f, 0.0f);
    }

    public void DeacreaseHealth(float amount)
    {
        _currentHealth -= amount;
        //_healthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0.0f)
        {
            StateMachine.ChangeState(DeadState);
        }
    }
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
    public void SetCurrentHealth(float health)
    {
        _currentHealth = health;
    }

    public void AddToCurrentHealth(float addhealth)
    {
       
        _currentHealth += addhealth;
        if (_currentHealth > 100)
        {
            _currentHealth = 100;
        }
    }

    public HealthBar GetHealthBar()
    {
        return _healthBar;
    }

    //to call from animator
    public void KillAndRespawn()
    {
        GM.Respawn();
        _healthBar.SetHealth(0f);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public Transform ReturnGroundCheck()
    {
        return _groundCheck;
    }

    public void SetRightFacingDirectionAfterCutscene()
    {
        FacingDirection = 1;
    }
    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        // ground check radius
        Gizmos.DrawWireSphere(_groundCheck.position, _playerData.GroundCheckRadius);
        // wall check line
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _playerData.WallCheckDistance, _wallCheck.position.y, _wallCheck.position.z));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x - _playerData.WallCheckDistance, _wallCheck.position.y, _wallCheck.position.z));

        // ledge check line
        Gizmos.DrawLine(_ledgeCheck.position, new Vector3(_ledgeCheck.position.x + _playerData.WallCheckDistance, _ledgeCheck.position.y, _ledgeCheck.position.z));

        //attack hitbox
        Gizmos.DrawWireSphere(_attackHitbox.position, _playerData.AttackHitboxRadius);

    }

    #endregion

}
