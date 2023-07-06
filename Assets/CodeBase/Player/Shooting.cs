using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Shooting : NetworkBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxTimeReload;
    [SerializeField] private float _maxTimeDelay;
    [SerializeField] private int _bullets;
    [SerializeField] private int _maxBullets;

    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _pointShoot;
    
    private float _timeReload;
    private float _timeDelay;

    private bool _isFire => Input.GetButton("Fire1");

    private void Update()
    {
        if (_isFire && _timeDelay <= 0 && _timeReload <= 0 && _bullets > 0)
        {
            Shoot(_pointShoot.position, _pointShoot.rotation, _damage);
        }

        if (_timeDelay > 0)
            _timeDelay -= Time.deltaTime;

        if (_timeReload > 0)
        {
            _timeReload -= Time.deltaTime;
            if (_timeReload <= 0)
                Reload();
        }
    }
    
    [Command]
    private void Shoot(Vector3 pointShoot, Quaternion rotation, float damage)
    {
        var bullet = Instantiate(_bullet, pointShoot, rotation);
        NetworkServer.Spawn(bullet.gameObject);

        bullet.Damage = damage;
        bullet.Rigidbody2D.AddForce(bullet.transform.up);

        _bullets -= 1;
        _timeDelay = _maxTimeDelay;

        if (_bullets <= 0)
            _timeReload = _maxTimeReload;
    }

    private void Reload()
    {
        _bullets = _maxBullets;
    }
}
