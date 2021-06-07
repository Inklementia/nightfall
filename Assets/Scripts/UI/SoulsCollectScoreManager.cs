using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulsCollectScoreManager : MonoBehaviour
{
    private GameObject[] _souls;

    private int score;

    private bool _isWin = false;

    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        score = 0;
        _souls = GameObject.FindGameObjectsWithTag("Soul");
       
        _text.text = "0/" + _souls.Length.ToString();
    }

    private void Update()
    {
        if (score < 0)
        {
            score = 0;
        }

        _text.text = score.ToString() +"/" + _souls.Length.ToString();

        if(score == _souls.Length)
        {
            _isWin = true;

        }

    }

    public void AddSoul()
    {
        score += 1;
    }
    
    public bool IsWinConditionMet()
    {
        return _isWin;
    }
    

  
}
