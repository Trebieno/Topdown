using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public Rigidbody2D Rigidbody2D;

    private void Start()
    {
        Destroy(gameObject, 3);
    }
}
