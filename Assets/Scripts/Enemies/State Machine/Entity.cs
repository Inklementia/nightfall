using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine _stateMachine;

    public D_Entity _entityData;
    public int _facingDirection { get; private set; }
    public int _lastDamageDirection { get; private set; }

    public Rigidbody2D _rb { get; private set; }
    public Animator _anim { get; private set; }
    public GameObject _aliveGo { get; private set; }
    public AnimationToStateMachine _ATSM { get; private set; }

    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _playerCheck;
    [SerializeField] private Transform _groundCheck;
    // as we dont have any friction anywhere, when enemy hits the groudn we shall set it velocity to 0 - to avoid sliding

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StunResistanceBar _stunResistanceBar;
    [SerializeField] private GameObject _statsUI;
    [SerializeField] private EnemyKillScoreManager _killScore;

    private float _currentHealth;
    private float _currentStunResistance;
    private float _lastDamageTime;

    // if we will need diff velocity variables, instead of creating new ones, we will just change the value of this one
    private Vector2 _velocityWorkSpace;

    protected bool _isStunned;
    protected bool _isDead;

    public AudioManager AM { get; private set; }

    public virtual void Start()
    {
        _facingDirection = 1;
        _currentHealth = _entityData._maxHealth;
        _currentStunResistance = _entityData._stunResistance;

        _statsUI.SetActive(false);
        _healthBar.SetMaxHealth(_entityData._maxHealth);
        _stunResistanceBar.SetMaxStunResistance(_entityData._stunResistance);

        _aliveGo = transform.Find("Alive").gameObject;
        _rb = _aliveGo.GetComponent<Rigidbody2D>();
        _anim = _aliveGo.GetComponent<Animator>();
        _ATSM = _aliveGo.GetComponent<AnimationToStateMachine>();

        AM = FindObjectOfType<AudioManager>();

        _stateMachine = new FiniteStateMachine();
    }
    public virtual void Update()
    {
        _stateMachine._currentState.LogicUpdate();

        _anim.SetFloat("yVelocity", _rb.velocity.y);

        if(Time.time > _lastDamageTime + _entityData._stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        _stateMachine._currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        _velocityWorkSpace.Set(_facingDirection * velocity, _rb.velocity.y);
        _rb.velocity = _velocityWorkSpace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();

        _velocityWorkSpace.Set(angle.x * velocity * direction, angle.y * velocity);
        _rb.velocity = _velocityWorkSpace;
    }

    public virtual bool CheckWall() // | - wall
    {
        return Physics2D.Raycast(_wallCheck.position, _aliveGo.transform.right, _entityData._wallCheckDistance, _entityData._whatIsGround);
    }
    public virtual bool CheckLedge() // г - ledge
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.down, _entityData._ledgeCheckDistance, _entityData._whatIsGround);
    }
    public virtual bool CheckGround() // ___ - ground
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _entityData._groundCheckRadius, _entityData._whatIsGround);
    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, _aliveGo.transform.right, _entityData._minAgroDistance, _entityData._whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, _aliveGo.transform.right, _entityData._maxAgroDistance, _entityData._whatIsPlayer);
    }

    public virtual bool CheckPlayerInShortRangeAction()
    {
        return Physics2D.Raycast(_playerCheck.position, _aliveGo.transform.right, _entityData._closeRangeActionDistance, _entityData._whatIsPlayer);
    }

    public virtual void DamageHop(float yVelocity)
    {
        _velocityWorkSpace.Set(_rb.velocity.x, yVelocity);
        _rb.velocity = _velocityWorkSpace;
    }

    public virtual void ResetStunResistance()
    {
        _isStunned = false;
        _currentStunResistance = _entityData._stunResistance;

        _stunResistanceBar.SetMaxStunResistance(_entityData._stunResistance);
    }

    public virtual void SetStunResistanceZero()
    {
        _isStunned = true;

        _stunResistanceBar.SetStunResistance(0);
    }
    public virtual void Damage(AttackDetails attackdetails)
    {
        _lastDamageTime = Time.time;

        _currentHealth -= attackdetails.DamageAmount;
        _currentStunResistance -= attackdetails.StunDamageAmount;

        _statsUI.SetActive(true);

        _healthBar.SetHealth(_currentHealth);
        _stunResistanceBar.SetStunResistance(_currentStunResistance);

        DamageHop(attackdetails.KnockbackVeclocity);

        Instantiate(_entityData._hitParticle, _aliveGo.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));


        if(attackdetails.Position.x > _aliveGo.transform.position.x)
        {
            _lastDamageDirection = -1;//came from the right
        }
        else
        {
            _lastDamageDirection = 1;
        }

        if(_currentStunResistance <= 0)
        {
            _isStunned = true;
        }

        if(_currentHealth <= 0)
        {
            _isDead = true;
            _killScore.AddKill();
        }
    }

    public Transform ReturnGroundCheck()
    {
        return _groundCheck;
    }

    public virtual void Flip()
    {
        _facingDirection *= -1;
        _aliveGo.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position + (Vector3)(Vector2.right * _facingDirection * _entityData._wallCheckDistance));
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position + (Vector3)(Vector2.down * _entityData._ledgeCheckDistance));

        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.right * _entityData._closeRangeActionDistance), 0.2f);

        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.right * _entityData._minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(_playerCheck.position + (Vector3)(Vector2.right * _entityData._maxAgroDistance), 0.2f);
    }
}
