using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _damage;
    
    public void StartMove(float speedBullet, float destroyTime, float maxSpread, float damage)
    {
        _damage = damage;
        float spread = Random.Range(-maxSpread, maxSpread);
        Vector2 bulletDirection = Quaternion.Euler(0, 0, spread) * transform.right;
        _rb.AddForce(bulletDirection * speedBullet, ForceMode2D.Impulse);

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
            EnemyAll.Instance.Enemies[other.gameObject].Healing.SetDamage(_damage, transform.position);
        
        else if (other.CompareTag("Player"))
            other.GetComponent<Player>().Healing.SetDamage(_damage, transform.position);
        
        NetworkServer.Destroy(gameObject);
    }
}
