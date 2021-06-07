using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BtnFX : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _color;

    private float _originalTextSize;
    private Color _originalColor;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        _originalTextSize = _text.fontSize;
        _originalColor = _text.color;
    }
    public void HoverOn()
    {
        audioManager.Play("Pop");
        _text.color = _color;
        _text.fontSize = _originalTextSize + 5;
    }
    public void HoverOff()
    {
        _text.color = _originalColor;
        _text.fontSize = _originalTextSize;
    }
    public void Click()
    {
        audioManager.Play("Pop");
        _text.color = _color;
        _text.fontSize = _originalTextSize + 5;
    }
    public void UnClick()
    {
        _text.color = _originalColor;
        _text.fontSize = _originalTextSize;
    }
}
