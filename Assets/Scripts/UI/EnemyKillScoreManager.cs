using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyKillScoreManager : MonoBehaviour
{
    public static int score;

    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        //_text = GetComponentInChildren<TMP_Text>();

        score = 0;
    }

    private void Update()
    {
        if(score < 0)
        {
            score = 0;
        }

        _text.text = "x " + score;
    }

    public void AddKill()
    {
        score += 1;
    }

}
