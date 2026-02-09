using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Bullet
{
    public class Bullet : MonoBehaviour
    {
        private BulletData _data;
        private Vector3 _startDirection;
        private BulletPool _pool;
        private float _life;

        public void Init(BulletData in_data, Vector3 in_direction)
        {
            _life = 0;
            _data = in_data;
            _startDirection = in_direction;
        }

        public void SetPool(BulletPool in_pool)
        {
            _pool = in_pool;
        }

        private void Update()
        {
            _life += Time.deltaTime;
            if (_life >= _data.maximumLifeTime)
            {
                _pool.Recycle(this);
            }

            float speed = _data.speedOvertime.Evaluate(_life/_data.maximumLifeTime);
            transform.position += _startDirection * speed * _data.speedMultiplier * Time.deltaTime;
        }
    }
}
