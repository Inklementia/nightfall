using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunResistanceBar : MonoBehaviour
{
    public Slider _slider;
    public void SetStunResistance(float stunRes)
    {
        _slider.value = stunRes;

    }
    public void SetMaxStunResistance(float maxStunRes)
    {
        _slider.maxValue = maxStunRes;
        _slider.value = maxStunRes;

    }
}
