using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulCollect : MonoBehaviour
{
    private bool _actionInput;
    private bool _nearSoul;

    public float toAdd = 10;

    private Player _player;
    private AudioManager AM;

    [SerializeField] private Animator _anim;

    [SerializeField] private SoulsCollectScoreManager _scoreManager;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        AM = FindObjectOfType<AudioManager>();
    }
    private void Update()
    {
        // pressing E
        _actionInput = _player.InputHandler.ActionInput;
        
        if(_actionInput && _nearSoul)
        {
            _player.InputHandler.UseActionInput();
            DevourSoul();
            _scoreManager.AddSoul(); // to show how much souls he collected
        }
    }

    private void DevourSoul()
    {
        AM.Play("Soul");
        _player.AddToCurrentHealth(toAdd);
        _anim.SetTrigger("destroy");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _nearSoul = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _nearSoul = false;
    }
}
