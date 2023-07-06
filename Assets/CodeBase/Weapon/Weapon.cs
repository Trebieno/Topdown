using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private String _name;
    [SerializeField] private int _level;
    [SerializeField] private float _damage;
    [SerializeField] private float _spread;
    [SerializeField] private float _delayTime;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _weight;
    [SerializeField] private float _speedBullet;

    [SerializeField] private int _bullet;
    [SerializeField] private int _maxBullet;
    [SerializeField] private Transform _firePoint;

    public string Name => _name;
    public int Level => _level;
    public float Damage => _damage;
    public float Spread => _spread;
    public float DelayTime => _delayTime;
    public float MaxDelayTime => _maxDelayTime;
    public float ReloadTime => _reloadTime;
    public float Weight => _weight;
    public float SpeedBullet => _speedBullet;

    public int Bullet => _bullet;
    public int MaxBullet => _maxBullet;

    public event Action Shooted;
    public event Action Reloaded;


    [SerializeField] private Bullet _bulletPrefab;

    private float _maxDelayTime;
    private float _maxReloadTime;


    private void Start()
    {
        _maxDelayTime = _delayTime;
        _maxReloadTime = _reloadTime;
    }

    public void FixedUpdate()
    {
        if(_delayTime > 0)
            _delayTime -= Time.fixedDeltaTime;
        
        if(_reloadTime > 0)
        {
            _reloadTime -= Time.fixedDeltaTime;
            if(_reloadTime <= 0)
                Reload();
        }
    }
    
    public void Shoot()
    {
        if(_bullet <= 0)
        {
            _reloadTime = _maxReloadTime;
            return;
        }

        SpawnBullet(_firePoint.position , _firePoint.rotation, _speedBullet, _spread, _damage);
        _delayTime = _maxDelayTime;
        Shooted?.Invoke();
    }
    
    [Command]
    private void SpawnBullet(Vector3 position, Quaternion quaternion, float speedBullet, float spread, float damage)
    {
        Spawn(position, quaternion, speedBullet, spread, damage);
    }
    
    [ClientRpc]
    private void Spawn(Vector3 position, Quaternion quaternion, float speedBullet, float spread, float damage)
    {
        var bullet = Instantiate(_bulletPrefab, position, quaternion);
        NetworkServer.Spawn(bullet.gameObject);
        
        var rb = bullet.GetComponent<Rigidbody2D>();
        spread = Random.Range(- spread, spread);
        Vector2 bulletDirection = Quaternion.Euler(0, 0, spread) * transform.right;
        rb.AddForce(bulletDirection * speedBullet, ForceMode2D.Impulse);
        bullet.StartMove(speedBullet, 2, spread, damage);
        _bullet -= 1;
    }

    public void Reload()
    {
        _bullet = _maxBullet;
        Reloaded?.Invoke();
    }

    public void Upgrade()
    {

    }

    public void UpgradeDamage()
    {

    }

    public void UpgradeSpread()
    {
        
    }

    public void UpgradeDelay()
    {
        
    }

    public void UpgradeReload()
    {
        
    }

    public WeaponData ConvertToWeaponData()
    {
        WeaponData weaponData = new WeaponData();
        weaponData.Name = _name;
        weaponData.Level = _level;
        weaponData.Damage = _damage;
        weaponData.Spread = _spread;

        weaponData.DelayTime = _delayTime;
        weaponData.MaxDelayTime = _maxDelayTime;

        weaponData.ReloadTime = _reloadTime;
        weaponData.MaxRealoadTime = _maxReloadTime;

        weaponData.Weight = _weight;
        weaponData.SpeedBullet = _speedBullet;
        weaponData.Bullet = _bullet;
        weaponData.MaxBullet = _maxBullet;
        return weaponData;
    }

    public void SetParameters(WeaponData weaponData)
    {
        _name = weaponData.Name;
        _level = weaponData.Level;
        _damage = weaponData.Damage;
        _spread = weaponData.Spread;

        _delayTime = weaponData.DelayTime;
        _maxDelayTime = weaponData.MaxDelayTime;

        _reloadTime = weaponData.ReloadTime;
        _maxReloadTime = weaponData.MaxRealoadTime;

        _weight = weaponData.Weight;
        _speedBullet = weaponData.SpeedBullet;
        _bullet = weaponData.Bullet;
        _maxBullet = weaponData.MaxBullet;

    }
}
