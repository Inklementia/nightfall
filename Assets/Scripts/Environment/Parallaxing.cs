using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] private Transform[] _backgrounds;

    private float[] __parallaxScales; //propotion of camera movement to move backgrounds by
    [SerializeField] private float _smoothing = 0.5f; //for smoothing parallax effect

    private Transform _cam;
    private Vector3 _prevCamPos;

    private void Awake()
    {
        _cam = Camera.main.transform;
    }

    private void Start()
    {
        _prevCamPos = _cam.position;

        __parallaxScales = new float[_backgrounds.Length];

        //assigning paralaxScales
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            __parallaxScales[i] = _backgrounds[i].position.z * -1;

        }
    }
    private void FixedUpdate()
    {
        //for each background
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            //parallax is the opposite of the camera movement , cause previous fgrame is multiplied by the scale
            float parallax = (_prevCamPos.x - _cam.position.x) * __parallaxScales[i];

            float backgroundTargetPosX = _backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, _backgrounds[i].position.y, _backgrounds[i].position.z);

            //fade
            _backgrounds[i].position = Vector3.Lerp(_backgrounds[i].position, backgroundTargetPos, _smoothing * Time.deltaTime);
        }

        //set prev cam position to the cameras position at the end of the frame
        _prevCamPos = _cam.position;
    }
}
