using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _deathChunkParticles, _deathBloodParticles;

    private HealthBar _healthBar;
    private GameObject _gameUI;

    private GameManager _GM;

    private float _currentHealth;

    private void Start()
    {
        _gameUI = GameObject.Find("GameUI");

        _healthBar = _gameUI.transform.Find("HealthBar").GetComponent<HealthBar>();
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    public void DeacreaseHealth(float amount)
    {
        _currentHealth -= amount;
        _healthBar.SetHealth(_currentHealth);
        if(_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(_deathChunkParticles, transform.position, _deathChunkParticles.transform.rotation);
        Instantiate(_deathBloodParticles, transform.position, _deathBloodParticles.transform.rotation);
        //mb i will change that to epic death animation

        _GM.Respawn();
        Destroy(gameObject);
    }
}
