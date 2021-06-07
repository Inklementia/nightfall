using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _knockbackSpeedX;
    [SerializeField] private float _knockbackSpeedY;
    [SerializeField] private float _knockbackDuration;
    [SerializeField] private float _knockbackDeathSpeedX, _knockbackDeathSpeedY, _deathTorque;//for top part spin

    [SerializeField] private GameObject _hitParticle;

    [SerializeField] private bool _applyKnockback;


    private float _currentHealth;
    private float _knockbackStart;

    private int _playerFacingDirection;
    private bool _playerOnLeft;
    private bool _knockback;

    private PlayerController _pc;
    private GameObject _aliveGo, _brokenTopGo, _brokenBottomGo;
    private Rigidbody2D _aliveRb, _brokenTopRb, _brokenBottomRb;
    private Animator _aliveAnim;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _pc = GameObject.Find("Player").GetComponent<PlayerController>();

        _aliveGo = transform.Find("Alive").gameObject;
        _brokenTopGo = transform.Find("Broken Top").gameObject;
        _brokenBottomGo = transform.Find("Broken Bottom").gameObject;

        _aliveAnim = _aliveGo.GetComponent<Animator>();

        _aliveRb = _aliveGo.GetComponent<Rigidbody2D>();
        _brokenTopRb = _brokenTopGo.GetComponent<Rigidbody2D>();
        _brokenBottomRb = _brokenBottomGo.GetComponent<Rigidbody2D>();

        _aliveGo.SetActive(true);
        _brokenTopGo.SetActive(false);
        _brokenBottomGo.SetActive(false);

    }
    private void Update()
    {
        CheckKnockback();
    }
    private void Damage(AttackDetails attackDetails)
    {
        _currentHealth -= attackDetails.DamageAmount;
        
        if(attackDetails.Position.x < _aliveGo.transform.position.x) //if from left
        {
            _playerFacingDirection = 1;
        }
        else
        {
            _playerFacingDirection = -1;
        }

        Instantiate(_hitParticle, _aliveAnim.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if(_playerFacingDirection == 1)
        {
            _playerOnLeft = true;
        }
        else
        {
            _playerOnLeft = false;
        }

        _aliveAnim.SetBool("playerOnLeft", _playerOnLeft);
        _aliveAnim.SetTrigger("damage");

        if(_applyKnockback && _currentHealth > 0.0f)
        {
            //knockback
            Knockback();
        }

        if(_currentHealth < 0.0f)
        {
            //die
            Die();
        }
    }
    private void Knockback()
    {
        _knockback = true;
        _knockbackStart = Time.time;
        _aliveRb.velocity = new Vector2(_knockbackSpeedX * _playerFacingDirection, _knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= _knockbackStart + _knockbackDuration && _knockback)
        {
            _knockback = false;
            _aliveRb.velocity = new Vector2(0.0f, _aliveRb.velocity.y);
        }
    }
    private void Die()
    {
        _aliveGo.SetActive(false);
        _brokenTopGo.SetActive(true);
        _brokenBottomGo.SetActive(true);

        _brokenTopGo.transform.position = _aliveGo.transform.position;
        _brokenBottomGo.transform.position = _aliveGo.transform.position;

        _brokenBottomRb.velocity = new Vector2(_knockbackSpeedX * _playerFacingDirection, _knockbackSpeedY);
        _brokenTopRb.velocity = new Vector2(_knockbackDeathSpeedX * _playerFacingDirection, _knockbackDeathSpeedY);
        _brokenTopRb.AddTorque(_deathTorque * -_playerFacingDirection, ForceMode2D.Impulse);
    }
}
