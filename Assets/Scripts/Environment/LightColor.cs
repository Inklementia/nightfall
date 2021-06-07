using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColor : MonoBehaviour
{
     // Interpolate light color between two colors back and forth
     [SerializeField] private float duration = 1.0f;
    [SerializeField] private Color color0 = Color.red;
    [SerializeField] private Color color1 = Color.blue;
    [SerializeField] private Light2D _light;

     void Update()
     {
         // set light color
         float t = Mathf.PingPong(Time.time, duration) / duration;
        _light.color = Color.Lerp(color0, color1, t);
     }
}
