using UnityEngine;

namespace BossMechanicTool.BulletSystem
{
    public class Bullet : MonoBehaviour
    {
        private BulletData _data;
        private Vector3 _startDirection;
        // private BulletPool _pool;
        private float _life;

        public void Init(BulletData in_data, Vector3 in_position, Vector3 in_direction)
        {
            _life = 0;
            _data = in_data;
            transform.position = in_position;
            _startDirection = in_direction;
        }

        /*
        public void SetPool(BulletPool in_pool)
        {
        
            _pool = in_pool;
        }
        */

        private void Update()
        {
            _life += Time.deltaTime;
            if (_life >= _data.maximumLifeTime)
            {
                Destroy(gameObject);
                // _pool.Recycle(this);
            }

            float speed = _data.speedOverLifeTime.Evaluate(_life/_data.maximumLifeTime);
            transform.position += _startDirection * speed * _data.speedMultiplier * Time.deltaTime;
        }
    }
}
