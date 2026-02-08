using UnityEngine;

namespace Tool.Bullet.BulletBehaviour
{
    public abstract class BulletBehaviour : ScriptableObject
    {
        public virtual void OnSpawn(Bullet bullet)
        {
        }

        public virtual void Tick(float dt)
        {
        }
    }
}
