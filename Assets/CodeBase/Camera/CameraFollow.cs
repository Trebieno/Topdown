using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;

    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private float _smoothTime = 1;

    [SerializeField] private float _minOrtograpgics = 5;
    [SerializeField] private float _maxOrtograpgics = 15;

    [SerializeField] private float _minDistance = 1;
    [SerializeField] private float _maxDistance = 10;

    [SerializeField] private float _maxFlightDistance = 1;

    private Vector3 currentVelocity; // это поле нужно для работы метода Vector3.SmoothDamp

    private void Start()
    {
        if(_camera == null)
            _camera = Camera.main;
    }

    public void FixedUpdate()
    {
        var mouseScreenPos = Input.mousePosition;
        var mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x , mouseScreenPos.y, _camera.nearClipPlane));

        var currentPos = _camera.transform.position;
        var targetPos = _target.position + _followOffset;
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref currentVelocity, _smoothTime);

        var directionToMouse = mouseWorldPos - _target.position;
        directionToMouse = Vector3.ClampMagnitude(directionToMouse, _maxFlightDistance);

        var lookAtPos = _target.position + directionToMouse + _followOffset;
        currentPos = Vector3.SmoothDamp(currentPos, lookAtPos, ref currentVelocity, _smoothTime);

        var distance = Vector2.Distance(_target.position, lookAtPos);
        float t = Mathf.InverseLerp(_minDistance, _maxDistance, distance);
        _camera.transform.position = Vector3.Lerp(currentPos, lookAtPos, t);
        _camera.orthographicSize = Mathf.Lerp(_minOrtograpgics, _maxOrtograpgics, t);
    }
}
