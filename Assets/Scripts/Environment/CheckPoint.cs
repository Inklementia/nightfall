using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPoint : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private Player _player;

    [SerializeField] private GameObject _text;
    [SerializeField] private GameObject _newSavePointText;
    [SerializeField] private GameObject _windLeaves;

    [SerializeField] private TMP_Text _textField;
 

    private bool _actionInput;

    private bool _nearNewCheckPoint;

    private string _originalText;


    private void Start()
    {
         gameManager = FindObjectOfType<GameManager>();
         audioManager = FindObjectOfType<AudioManager>();
        _player = FindObjectOfType<Player>();

        // just text
        _originalText = _textField.text;
 

    }

    private void Update()
    {
        // pressing E
        _actionInput = _player.InputHandler.ActionInput;

        if(_actionInput && _nearNewCheckPoint)
        {
            SaveNewCheckPoint();
        }
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
           
            if (gameObject == gameManager.currentCheckpoint)
            {       
                // if current checkpoint just get players current health
                gameManager.currentPlayerHealth = _player.GetCurrentHealth();
            }
            else
            {
                // if new one -> show text (typing effect) and say that it is a new checkpoint
                _text.SetActive(true);
                StartCoroutine(TypeSentence(_originalText));
                _nearNewCheckPoint = true;
            }
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _newSavePointText.SetActive(false);
        _text.SetActive(false);
        _nearNewCheckPoint = false;
    }

    private void SaveNewCheckPoint()
    {
        // text field manipulations
        _text.SetActive(false);
        _newSavePointText.SetActive(true);

        //wind with leaves
        audioManager.Play("SaveWind");
        _windLeaves.SetActive(true);

        // save current health -> player will respwan here with same health as he saved
        gameManager.currentPlayerHealth = _player.GetCurrentHealth();
        gameManager.currentCheckpoint = gameObject;
        _nearNewCheckPoint = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        _textField.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _textField.text += letter;
            yield return null;
        }

    }
}
