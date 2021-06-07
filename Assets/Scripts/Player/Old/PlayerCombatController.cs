using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _attack1HitBoxPos;
    [SerializeField] private LayerMask _whatIsDamageable;

    [SerializeField] private bool _combatEnabled;

    [SerializeField] private float _inputTimer;
    [SerializeField] private float _attack1Radius;
    [SerializeField] private float _attack1Damage;
    [SerializeField] private float _stunDamageAmount = 1f;

    //private float[] _attackDetails = new float[2];
    private AttackDetails _attackDetails;

    private bool _gotInput;
    private bool _isAttacking;
    private bool _isFirstAttack;

    private float _lastInputTime = Mathf.NegativeInfinity;

    private PlayerController _PC;
    private PlayerStats _PS;

    private void Start()
    {
        _anim.SetBool("canAttack", _combatEnabled);
        _PC = GetComponent<PlayerController>();
        _PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttack();
    }
    private void CheckCombatInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_combatEnabled)
            {
                //attempt combat
                _gotInput = true;
                _lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttack()
    {
        if (_gotInput)
        {
            //perform attack
            if (!_isAttacking)
            {
                _gotInput = false;
                _isAttacking = true;
                _isFirstAttack = !_isFirstAttack;

                _anim.SetBool("attack1", true);
                _anim.SetBool("firstAttack", _isFirstAttack);
                _anim.SetBool("isAttacking", _isAttacking);
            }
        }
        if(Time.time >= _lastInputTime + _inputTimer)
        {
            //wait for new input
            _gotInput = false;
        }

    }

    //to be called from animator
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attack1HitBoxPos.position, _attack1Radius, _whatIsDamageable);

        _attackDetails.DamageAmount = _attack1Damage;
        _attackDetails.Position = transform.position;
        _attackDetails.StunDamageAmount = _stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", _attackDetails);
            //Instanciate hit particles
            //will instanciate in different script so enemies will have different particles
        }
    }

    private void FinishAttack1()
    {
        _isAttacking = false;
        _anim.SetBool("isAttacking", _isAttacking);
        _anim.SetBool("attack1", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!_PC.GetDashStatus())
        {
        int direction;

        _PS.DeacreaseHealth(attackDetails.DamageAmount);

        if(attackDetails.Position.x < transform.position.x)
        {
            direction = 1; //right 
        }
        else
        {
            direction = -1; //left
        }

        _PC.KnockBack(direction);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attack1HitBoxPos.position, _attack1Radius);
    }
}
