using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider _slider;
    public Image _fill;
    public Gradient _gradient;

    private void Update()
    {

    }

    public void SetHealth(float health)
    {
        _slider.value = health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);

      

    }
    public void SetMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
        _fill.color = _gradient.Evaluate(1f);

    }
}
