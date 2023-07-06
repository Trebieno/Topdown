using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Character : MonoBehaviour
{
    [SerializeField] private Healing _healing;

    [SerializeField] private float _timeChangePos;
    [SerializeField] private float _positionChangeRadius;

    [SerializeField] private float _radiusFov;
    [SerializeField] private float _angleFov;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private AIPath _agent;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _nearAttackDistance;
    [SerializeField] private float _damage;


    private float _maxTimeChangePos;
    private float _maxAttackCooldown;
    public Healing Healing => _healing;

    
    private void Start()
    {
        EnemyAll.Instance.Enemies.Add(gameObject, this);
        _maxTimeChangePos = _timeChangePos;
        _maxAttackCooldown = _attackCooldown;
        _healing.Damaged += SetTarget;
    }

    private void OnDestroy()
    {
        _healing.Damaged -= SetTarget;
    }

    public void FixedUpdate()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(transform.position, _radiusFov, _enemyMask);
        for (int i = 0; i < colliders2D.Length; i++)
        {
            Vector2 direction = colliders2D[i].transform.position - transform.position;
            float angleBetween = Vector3.SignedAngle(direction, transform.up, Vector2.up);

            if(_destinationSetter.target == null && angleBetween <= _angleFov)
            {
                _destinationSetter.target = colliders2D[i].transform;
                Debug.Log("Обнаружил");
            }
        }
        
        if(_destinationSetter.target != null)
        {
            bool isRadius = false;
            if(colliders2D.Length > 0)
                for (int i = 0; i < colliders2D.Length; i++)
                {
                    if(_destinationSetter.target == colliders2D[i].transform)
                        isRadius = true;
                }
            

            if(!isRadius)
                _destinationSetter.target = null;

        }

        if(_timeChangePos <= 0)
        {
            if(_destinationSetter.target == null)
            {
                Vector3 randomPos = GetRandomPosition(transform.position, _positionChangeRadius);
                _agent.destination = randomPos;
            }
            _timeChangePos = _maxTimeChangePos;
        }
        else
            _timeChangePos -= Time.fixedDeltaTime;

        if(_destinationSetter.target != null)
        {
            float distance = (transform.position - _destinationSetter.target.position).magnitude;
            if(distance <= _nearAttackDistance)
            {
                if(_attackCooldown <= 0)
                {
                    Collider2D[] attackTargets = Physics2D.OverlapCircleAll(transform.position, _attackRadius, _enemyMask);
                    if(attackTargets.Length > 0)
                    {
                        // PlayerCache.Instance.Players[attackTargets[0].gameObject].Healing.SetDamage(_damage, _attackPoint.position);
                        PlayerCache.Instance.Player.Healing.SetDamage(_damage, _attackPoint.position);
                    }
                    _attackCooldown = _maxAttackCooldown;
                }
                else
                    _attackCooldown -= Time.fixedDeltaTime;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusFov);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _positionChangeRadius);

        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

    private Vector3 GetRandomPosition(Vector3 center, float radius)
    {
        Vector3 randomPos = center + Random.insideUnitSphere * radius;
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(randomPos, 1f, _wallMask));

        while (colliders.Count > 0)
        {
            randomPos = center + Random.insideUnitSphere * radius;
            colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(randomPos, 1f, _wallMask));
        }

        return randomPos;
    }

    private void SetTarget(Vector3 position)
    {
        _agent.destination = PlayerCache.Instance.Player.transform.position;
    }
}
