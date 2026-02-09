using System.Collections.Generic;
using UnityEngine;
using BulletHell.Bullet;

namespace BulletHell.Emitter
{
    [CreateAssetMenu(menuName = "Bullet Hell/Emitter")]
    public class EmitterData : ScriptableObject
    {
        [Header("Emitter data informations")]
        
        [Header("Data")] 
        public BulletData bulletData;

        [Header("Emitter Properties")] 
        [Range(1, 100)] [Tooltip("Number of projectiles per emission")]
        public int count;

        [Range(0.01f, 2f)] [Tooltip("Time between two emission (in seconds)")]
        public float fireInterval = 0.2f;

        [Min(-1f)] [Tooltip("Total active time. Set less than 0 for infinite.")]
        public float duration = -1f;

        [Header("Shape Properties")]
        public float rotationSpeed;
        public float arcAngle = 360f;

        private float _elapsed = 0f;
        private float _fireTimer = 0f;
        
        public void OnTick(BulletHell.Bullet.BulletPool pool, Transform transform, float dt)
        {
            _elapsed += dt;
            _fireTimer += dt;

            if (duration > 0 && _elapsed >= duration)
                return;

            while (_fireTimer >= fireInterval)
            {
                Fire(pool, transform);
                _fireTimer -= fireInterval;
            }
        }

        private void Fire(BulletHell.Bullet.BulletPool pool, Transform transform)
        {
            IEnumerable<Vector3> directions = GetDirections(_elapsed, transform);

            foreach (var dir in directions)
            {
                pool.Spawn(transform.position, dir, bulletData);
            }
        }
        
        public IEnumerable<Vector3> GetDirections(float time, Transform transform)
        {
            if (count <= 0 || arcAngle <= 0f)
                yield break;

            float startAngle = -arcAngle * 0.5f;
            float step = (count == 1) ? 0f : arcAngle / (count - 1);
            float rotation = time * rotationSpeed;
            
            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + step * i + rotation;
                Quaternion rot = Quaternion.AngleAxis(angle, transform.up);
                yield return rot * transform.forward;
            }
        }
    }
}
