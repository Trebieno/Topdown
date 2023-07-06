using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public string Name;
    public SpriteRenderer Skin;
    public int Level;
    public float Damage;
    public float Spread;

    public float DelayTime;
    public float MaxDelayTime;

    public float ReloadTime;
    public float MaxRealoadTime;

    public float Weight;
    public float SpeedBullet;

    public int Bullet;
    public int MaxBullet;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speedDrop;
    private void Start()
    {
        DropAll.Instance.Drops.Add(gameObject, this);
        _rb.AddForce(transform.up * Time.deltaTime * _speedDrop, ForceMode2D.Impulse);
    }
}
