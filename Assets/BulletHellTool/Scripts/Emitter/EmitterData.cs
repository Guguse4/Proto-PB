using UnityEngine;
using Tool.Bullet;

namespace Tool.Emitter
{
    [CreateAssetMenu(menuName = "Bullet Hell/Emitter")]
    public class EmitterData : ScriptableObject
    {
        [Header("Core Settings")] public BulletShape shape;
        public BulletData bulletData;

        [Header("Emission Properties")] [Range(1, 100)] [Tooltip("Number of projectiles per emission")]
        public int count;

        [Range(0.01f, 2f)] [Tooltip("Time between two emission (in seconds)")]
        public float fireInterval = 0.2f;

        [Min(-1f)] [Tooltip("Total active time. Set -1 for infinite.")]
        public float duration;

        private float _elapsed;
        private float _fireTimer;

        public void OnStart()
        {
            _elapsed = 0f;
            _fireTimer = 0f;
        }

        public void OnTick(Tool.Bullet.BulletPool pool, Vector3 position, float dt)
        {
            _elapsed += dt;
            _fireTimer += dt;

            if (duration > -1 && _elapsed >= duration)
                return;

            while (_fireTimer >= fireInterval)
            {
                Fire(pool, position);
                _fireTimer -= fireInterval;
            }
        }

        private void Fire(Tool.Bullet.BulletPool pool, Vector3 position)
        {
            var directions = shape.GetDirections(count, _elapsed);

            foreach (var dir in directions)
            {
                pool.Spawn(position, dir, bulletData);
            }
        }
    }
}
