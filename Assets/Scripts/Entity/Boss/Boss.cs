using UnityEngine.Events;

namespace Entity.Boss
{
    public class Boss: Entity
    {
        public UnityEvent<int> OnTakeDamage = new UnityEvent<int>();
        
        public void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            OnTakeDamage.Invoke(_currentHealth);
        }
    }
}