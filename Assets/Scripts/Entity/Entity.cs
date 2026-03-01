using UnityEngine;

namespace Entity
{
    public abstract class Entity: MonoBehaviour, IDamageable
    {
        [SerializeField] private EntityData _data;
        protected int _currentHealth;
        public int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _currentHealth = _data.maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log(gameObject.name + " take damage: "+_currentHealth+" left.");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}