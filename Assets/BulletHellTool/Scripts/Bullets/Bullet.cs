using System.Collections.Generic;
using UnityEngine;
using Tool.Bullet.BulletMovement;
using Tool.Bullet.BulletBehaviour;

namespace Tool.Bullet
{
    public class Bullet : MonoBehaviour
    {
        private BulletMovementInstance _movement;
        private List<BulletBehaviour.BulletBehaviour> _behaviours;
        private BulletPool _pool;

        private float _life;
        private float _maximumLifeTime;

        public void Init(BulletData in_data, Vector2 in_direction)
        {
            _life = 0;
            _movement = in_data.CreateMovement();
            _behaviours = in_data.CreateBehaviours();
            _maximumLifeTime = in_data.maximumLifeTime;
            _movement.Init(transform, in_direction, in_data.speed);

            foreach (var b in _behaviours)
            {
                b.OnSpawn(this);
            }
        }

        public void SetPool(BulletPool in_pool)
        {
            _pool = in_pool;
        }

        private void Update()
        {
            _life += Time.deltaTime;
            if (_life >= _maximumLifeTime)
            {
                _pool.Recycle(this);
            }

            _movement.Tick(Time.deltaTime);
            foreach (var b in _behaviours)
            {
                b.Tick(Time.deltaTime);
            }
        }
    }
}
