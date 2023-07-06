using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private List<Weapon> _allWeapons = new List<Weapon>();
    [SerializeField] private List<Weapon> _allSlots = new List<Weapon>();
    [SerializeField] private CameraShake _shake;
    [SerializeField] private float _speedDrop = 1;
    [SerializeField] private Transform _usePoint;
    [SerializeField] private float _useRadius;
    [SerializeField] private LayerMask _dropMask;

    [SerializeField] private WeaponData _weaponDataPref;
    
    private Weapon _weapon => _allSlots[_index];
    private bool _isFire => Input.GetButton("Fire1");
    private bool _isDrop => Input.GetButtonDown("Drop");
    private bool _isUse => Input.GetButtonDown("Use");
    private int _index = 0;

    public event Action<float> SelectedGun;

    public void Update()
    {
        if(_isFire && _weapon != null && _weapon.DelayTime <= 0 && _weapon.ReloadTime <= 0)
        {
            _weapon.Shoot();
            _shake.Shake();
        }

        if(_isDrop && _weapon != null)
        {
            Drop();
        }

        if(_isUse)
        {
            var colliders = Physics2D.OverlapCircleAll(_usePoint.position, _useRadius, _dropMask);
            if(colliders.Length > 0)
            {
                var weaponData = DropAll.Instance.Drops[colliders[0].gameObject];                
                for (int i = 0; i < _allWeapons.Count; i++)
                {
                    if(_allWeapons[i].Name == weaponData.Name)
                    {
                        if(_weapon != null)
                            Drop();
                        
                        _allSlots[_index] = _allWeapons[i];
                        _allWeapons[i].gameObject.SetActive(true);
                        Destroy(weaponData.gameObject);
                    }
                        
                }
            }
        }
        
        ScrollingWeapon();
    }
    private void ScrollingWeapon()
    {
        var scrollView = Input.GetAxis("Mouse ScrollWheel");
        int i = _index;
        if(scrollView < 0)
            _index++;

        if(scrollView > 0)
            _index--;
        
        if(_index > _allSlots.Count - 1)
            _index = 0;
        
        if(_index < 0)
            _index = _allSlots.Count - 1;
        
        if(i != _index)
        {
            if(_weapon != null)
                SelectedGun?.Invoke(_weapon.Weight);
        }            
    }

    private void Drop()
    {
        var weaponData = Instantiate(_weaponDataPref, transform.position, transform.rotation);
        SetParametersWeaponData(_weapon, weaponData);

        _weapon.gameObject.SetActive(false);
        _allSlots[_index] = null;
        Debug.Log("123");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(_usePoint.position, _useRadius);
    }

    private void SetParametersWeaponData(Weapon weapon , WeaponData weaponData)
    {
        weaponData.Name = weapon.Name;
        weaponData.Level = weapon.Level;
        weaponData.Damage = weapon.Damage;
        weaponData.Spread = weapon.Spread;
        weaponData.DelayTime = weapon.DelayTime;
        weaponData.MaxDelayTime = weapon.MaxDelayTime;
        weaponData.ReloadTime = weapon.ReloadTime;
        weaponData.Weight = weapon.Weight;
        weaponData.SpeedBullet = weapon.SpeedBullet;
        weaponData.Bullet = weapon.Bullet;
        weaponData.MaxBullet = weapon.MaxBullet;
    }
}
