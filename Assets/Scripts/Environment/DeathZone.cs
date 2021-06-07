using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 20f;
    [SerializeField] private float _knockbackVelocity = 20f;
    [SerializeField] private Vector2 _knockbackAngle = new Vector2(0, 10);
    [SerializeField] private GameObject _collideParticles;
    [SerializeField] private Transform _playerGroungCheck;

    private AttackDetails _attackDetails; // struct with attack details (attack details for spikes :D)

    private void Start()
    {
        _attackDetails.DamageAmount = _damageAmount;
        _attackDetails.KnockbackAngle = _knockbackAngle;
        _attackDetails.KnockbackVeclocity = _knockbackVelocity;


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _attackDetails.Position = collision.transform.position;
            collision.transform.SendMessage("Damage", _attackDetails);
            Instantiate(_collideParticles, _playerGroungCheck.position, _playerGroungCheck.rotation);
        }
    }

}
