using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform[] _points;
    [SerializeField] private int _pointSelection; // to whoch point shall platform move first
    [SerializeField] private float _waitTimer; // TODO: timer for moving platform
    private Transform _currentPoint;

    private void Start()
    {
        _currentPoint = _points[_pointSelection];
    }

    private void FixedUpdate()
    {
        _platform.transform.position = Vector3.MoveTowards(_platform.transform.position, _currentPoint.position, Time.deltaTime * _movementSpeed);
        if(_platform.transform.position == _currentPoint.position)
        {
            // move to another point
            _pointSelection++;
            if(_pointSelection == _points.Length)
            {
                // if reached the end of array of points move to the first point
                _pointSelection = 0;
            }
            _currentPoint = _points[_pointSelection];
        }
    }

}
