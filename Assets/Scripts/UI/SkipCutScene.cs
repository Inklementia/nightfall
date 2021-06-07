using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipCutScene : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayableDirector _plDirector;
    [SerializeField] private float _waitForSecondsStart;
    [SerializeField] private float _waitForSecondsFinish;
    [SerializeField] private GameObject _skipText;
    [SerializeField] private GameObject _dialogsBox;
    [SerializeField] private Player _player;

    private void Start()
    {
        StartCoroutine(WaitAndShow());
    }
    private void Update()
    {
        if(_plDirector.state != PlayState.Playing)
        {
           
            _anim.SetTrigger("fadeOut");
            StartCoroutine(WaitAndHide());
        }
        else{
            _player.DashState.SetCanDash(false);
            _player.IdleState.SetCanDie(false);
            _player.SetRightFacingDirectionAfterCutscene();
        }
    }
    public void StopCutScene()
    {
        _player.SetRightFacingDirectionAfterCutscene();
        _plDirector.Stop();
        _dialogsBox.SetActive(false);
        _anim.SetTrigger("fadeOut");
        StartCoroutine(WaitAndHide());
        
    }
    IEnumerator WaitAndShow()
    {
      
        yield return new WaitForSeconds(_waitForSecondsStart);
        _skipText.SetActive(true);
    }
    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(_waitForSecondsFinish);
        _player.IdleState.SetCanDie(true);
        _player.DashState.SetCanDash(true);
        _player.AttackState.EnableCombat();
        //_skipText.SetActive(false);
        Destroy(gameObject);

    }
}
