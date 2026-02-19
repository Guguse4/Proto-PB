using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerData playerData;
    private int _currentHealth;
    public int CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = playerData.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player dead");
        Destroy(gameObject);
    }
}
