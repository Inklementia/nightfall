using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject _winUI;

    [SerializeField] private TMP_Text _enemiesKilledField;
    [SerializeField] private TMP_Text _diedField;
    [SerializeField] private TMP_Text _killedYourselfField;

    private SoulsCollectScoreManager _soulScoreManager;
    private bool _once = true;

    // Start is called before the first frame update
    void Start()
    {
        _soulScoreManager = FindObjectOfType<SoulsCollectScoreManager>();
        _winUI.SetActive(false);
        _enemiesKilledField.text = "0";
        _diedField.text = "0";
        _killedYourselfField.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (_soulScoreManager.IsWinConditionMet() && _once)
        {
  
            _winUI.SetActive(true);

            _enemiesKilledField.text = EnemyKillScoreManager.score.ToString();
            _diedField.text = DeathTimesTracker.Deaths.ToString();
            _killedYourselfField.text = DeathTimesTracker.DeathsFromSelfDesctruct.ToString();
        }
    }

}
