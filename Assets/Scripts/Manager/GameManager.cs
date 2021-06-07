using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 
    private AudioManager audioManager;
    public GameObject currentCheckpoint;

    public GameObject _playerGO;
    [SerializeField] private float _respawnTime;

    private float _respawnTimeStart;
    private bool _respawn;

    // for "haha text" when Player destroys himself;
    [SerializeField] private GameObject _hahaText;
    [SerializeField] private TMP_Text _hahaTextField;

    private string _originalText;

    //private CinemachineVirtualCamera _CVC;
    [HideInInspector] public Player player;
    [HideInInspector] public float currentPlayerHealth = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {

        //_CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        player = _playerGO.GetComponent<Player>();
        audioManager = FindObjectOfType<AudioManager>();
        //player.transform.position = currentCheckpoint.transform.position;
        audioManager.Play("BgMusic");

        _originalText = _hahaTextField.text;
    }

    private void Update()
    {
        CheckRespawn();
    }
    public void Respawn()
    {
        _respawnTimeStart = Time.time;
        _respawn = true;
    }

    private void CheckRespawn()
    {
        if(Time.time >= _respawnTimeStart + _respawnTime && _respawn)
        {

            // var playerTemp = Instantiate(_player, CurrentCheckpoint.transform.position, Quaternion.identity);
       
            //seting health and hwere player should appear after death
            player.SetCurrentHealth(currentPlayerHealth);
            _playerGO.transform.position = currentCheckpoint.transform.position;
           
            _playerGO.SetActive(true);

            if (DeathTimesTracker.DeathsFromSelfDesctruct == 1)
            {
                _hahaText.SetActive(true);
                //type text and hide it
                StartCoroutine(TypeSentence(_originalText));
                StartCoroutine(WaitAndHide());
            }


            // _player.SetActive(true);
            //_CVC.m_Follow = playerTemp.transform; // make Cinemachine follow player again
            //TODO: Respawn sound
            _respawn = false;

        }
      
    }


    IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(3f);
        _hahaText.SetActive(false);

    }
    
    IEnumerator TypeSentence(string sentence)
    {
        _hahaTextField.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _hahaTextField.text += letter;
            yield return null;
        }

    }
}
