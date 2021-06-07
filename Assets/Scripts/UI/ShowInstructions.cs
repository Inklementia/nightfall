using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInstructions : MonoBehaviour
{
    [SerializeField] private GameObject _instructionText;

    [SerializeField] private bool _showOnce;
    [SerializeField] private bool _typing;

    [SerializeField] private TMP_Text _textField;

    private bool _wasShown;
    private string _originalText;

    private void Start()
    {
        _originalText = _textField.text;
        _instructionText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player" && !_wasShown)
        {
            //show text
            _instructionText.SetActive(true);

            if (_typing && !_wasShown)
            {
                //typing effect
                StartCoroutine(TypeSentence(_originalText));
            }
           //to show instructions only once
            if (_showOnce)
            {
                _wasShown = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
        _instructionText.SetActive(false);
        }
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
