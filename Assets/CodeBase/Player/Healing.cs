using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Healing : NetworkBehaviour
{
    [SyncVar] [SerializeField] private float _currentHealth;
    [SyncVar] [SerializeField] private float _maximumHealth;

    [SerializeField] private ParticleSystem _deathEffectPrefab;
    [SerializeField] private ParticleSystem _healEffectPrefab;
    [SerializeField] private ParticleSystem _damageEffectPrefab;

    public event Action Died;
    public event Action<Vector3> Damaged;
    public event Action HealHealth;
    public event Action<float, float> HealthChanged;
    public event Action<float, float> MaximumHealthChanged;
    public event Action Murdered;

    [Command]
    public void SetDamage(float damage, Vector3 damagedPosition)
    {
        Debug.Log(damage);
        _currentHealth -= damage;
        HealthChanged?.Invoke(_currentHealth, _maximumHealth);
        
        if (_currentHealth <= 0)
        {
            Death();
            Murdered?.Invoke();
        }

        if (_damageEffectPrefab != null)
        {
            var effect = Instantiate(_damageEffectPrefab, damagedPosition, Quaternion.identity);
            NetworkServer.Spawn(effect.gameObject);
        }
			
        Damaged?.Invoke(damagedPosition);
    }

    [Command]
    public virtual void Death()
    {
        if (_deathEffectPrefab != null)
        {
            var effect = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect.gameObject);
        }
			
        gameObject.SetActive(false);

        Died?.Invoke();
    }

    [Command]
    public virtual void Heal(float health)
    {
        if (_healEffectPrefab != null)
        {
            var effect = Instantiate(_healEffectPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect.gameObject);
        }

        _currentHealth += health;

        if(_currentHealth > _maximumHealth)
            _currentHealth = _maximumHealth;
        
        HealthChanged?.Invoke(_currentHealth, _maximumHealth);
        HealHealth?.Invoke();
    }
}