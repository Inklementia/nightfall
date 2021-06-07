using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails _attackDetails;

    private bool _isGravityOn;
    private bool _hasHitGround;

    private float _speed;
    private float _travelDistance;
    private float _xStartPos;

    [SerializeField] private float _gravity;
    [SerializeField] private float _damageRadius;

    private Rigidbody2D _rb;

    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _damagePosition;

    public GameObject _projectileBurstParticles;

    private AudioManager AM;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        AM = FindObjectOfType<AudioManager>();

        _rb.gravityScale = 0.0f;
        _isGravityOn = false;

        _rb.velocity = transform.right * _speed;

        _xStartPos = transform.position.x;
        
    }
    private void Update()
    {
        //if projectile is not yet on the ground
        if (!_hasHitGround)
        {
            //where spawn projectile
            _attackDetails.Position = transform.position;

            // when gravity is on -> projectile shall rotate in the direction it is falling
            if (_isGravityOn)
            {
                float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!_hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(_damagePosition.position, _damageRadius, _whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(_damagePosition.position, _damageRadius, _whatIsGround);

            //if hit Player
            if (damageHit)
            {
                damageHit.transform.SendMessage("Damage", _attackDetails);
                GameObject.Instantiate(_projectileBurstParticles, transform.position, _projectileBurstParticles.transform.rotation);
                Destroy(gameObject);
                AM.Play("RangedEnemyHit"); 
            }
            //if hit ground
            if (groundHit)
            {
                _hasHitGround = true;
                _rb.gravityScale = 0f;
                _rb.velocity = Vector2.zero;
                GameObject.Instantiate(_projectileBurstParticles, transform.position, _projectileBurstParticles.transform.rotation);
                Destroy(gameObject);
                AM.Play("RangedEnemyHit");
            }

            if (Mathf.Abs(_xStartPos - transform.position.x) >= _travelDistance && !_isGravityOn)
            {
                _isGravityOn = true;
                _rb.gravityScale = _gravity;
            }
        }
      
    }

    public void FireProjectile(float speed, float travelDistance, float damage, Vector2 knockbackAngle, float knockbackVelocity)
    {
        _speed = speed;
        _travelDistance = travelDistance;
        _attackDetails.DamageAmount = damage;
        _attackDetails.KnockbackAngle = knockbackAngle;
        _attackDetails.KnockbackVeclocity = knockbackVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_damagePosition.position, _damageRadius);
    }
}
