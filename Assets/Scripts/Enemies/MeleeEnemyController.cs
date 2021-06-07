using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    private enum State
    {
        Moving,
        Knockback,
        Dead
    }

    private State _currentState;

    private bool _groundDetected;
    private bool _wallDetected;

    private int _facingDirection = 1;
    private int _damageDirection;

    private float _currentHealth;
    private float _knockbackStartTime;
    private float _lastTouchDamageTime; //last time enemy damaged the player
    private float[] _attackDetails = new float[2];

    private Vector2 _movement;
    private Vector2 _touchDamageBotLeft;
    private Vector2 _touchDamageTopRight;

    [SerializeField] private GameObject _alive;
    [SerializeField] private Rigidbody2D _aliveRb;
    [SerializeField] private Animator _aliveAnim;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _touchDamageCheck;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private Vector2 _knockbackSpeed;

    [SerializeField] private GameObject _hitPatricles;
    [SerializeField] private GameObject _deathChunksParticles;
    [SerializeField] private GameObject _deathBloodParticles;

    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _knockbackDuration;
    [SerializeField] private float _touchDamageCooldown;
    [SerializeField] private float _touchDamage, _touchDamageWidth, _touchDamageHeight;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        switch (_currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    // -- walking state ---------------------------------------------------------------

    private void EnterMovingState()
    {

    }

    private void UpdateMovingState()
    {
        _groundDetected = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround);
        _wallDetected = Physics2D.Raycast(_wallCheck.position, transform.right, _wallCheckDistance, _whatIsGround);
        CheckTouchDamage();

        if(!_groundDetected || _wallDetected)
        {
            //flip
            Flip();
        }
        else
        {
            //move
            _movement.Set(_movementSpeed * _facingDirection, _aliveRb.velocity.y);
            _aliveRb.velocity = _movement;
        }
    }

    private void ExitMovingState()
    {

    }

    // -- knockback state -----------------------------------------------------------------

    private void EnterKnockbackState()
    {
        _knockbackStartTime = Time.time; //keep track of exact time when knockback started
        _movement.Set(_knockbackSpeed.x * _damageDirection, _knockbackSpeed.y);
        _aliveRb.velocity = _movement;
        _aliveAnim.SetBool("knockback", true);
    }

    private void UpdateKnockbackState()
    {
        if(Time.time >= _knockbackStartTime + _knockbackDuration)//knockback has been prolonged long enough
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState()
    {
        _aliveAnim.SetBool("knockback", false);
    }

    // -- dead state -------------------------------------------------------------
    private void EnterDeadState()
    {
        //spawn blood particles
        Instantiate(_deathChunksParticles, _alive.transform.position, _deathChunksParticles.transform.rotation);
        Instantiate(_deathBloodParticles, _alive.transform.position, _deathBloodParticles.transform.rotation);
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    // --> OTHER FUNCTIONS <------------
    private void Damage(float[] attackDetails)
    {
        _currentHealth -= attackDetails[0]; // damage = 0 index, from which side player - 1 index

        Instantiate(_hitPatricles, _alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if(attackDetails[1] > _alive.transform.position.x)
        {
            _damageDirection = -1;
        }
        else{
            _damageDirection = 1;
        }

        //hit particles

        if( _currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }else if(_currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }
    private void CheckTouchDamage()
    {
        if(Time.time >= _lastTouchDamageTime + _touchDamageCooldown)
        {
            _touchDamageBotLeft.Set(_touchDamageCheck.position.x - (_touchDamageWidth / 2), _touchDamageCheck.position.y - (_touchDamageHeight / 2));
            _touchDamageTopRight.Set(_touchDamageCheck.position.x + (_touchDamageWidth / 2), _touchDamageCheck.position.y + (_touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(_touchDamageBotLeft, _touchDamageTopRight, _whatIsPlayer);

            if(hit != null)
            {
                _lastTouchDamageTime = Time.time;
                _attackDetails[0] = _touchDamage;
                _attackDetails[1] = _alive.transform.position.x;

                hit.SendMessage("Damage", _attackDetails);
            }
        }
    }

    private void Flip()
    {
        _facingDirection *= -1;
        _alive.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void SwitchState(State state)
    {
        switch (_currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        _currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));

        Vector2 botLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2), _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 botRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2), _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 topRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2), _touchDamageCheck.position.y + (_touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2), _touchDamageCheck.position.y + (_touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
