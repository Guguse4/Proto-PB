using UnityEngine;
using UnityEngine.Pool;
using BossMechanicTool.BulletSystem;

namespace BossMechanicTool.BulletSystem
{
    public class BulletPool : MonoBehaviour
    {
        /*
        public Bullet bulletPrefab;
        private IObjectPool<Bullet> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<Bullet>(
                createFunc: CreateBullet,
                actionOnGet: OnGetBullet,
                actionOnRelease: OnReleaseBullet,
                actionOnDestroy: OnDestroyBullet,
                collectionCheck: true,
                defaultCapacity: 0,
                maxSize: 200
            );
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, transform);
            bullet.gameObject.SetActive(false);
            bullet.SetPool(this);
            return bullet;
        }

        private void OnGetBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnReleaseBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }

        public void Spawn(Vector3 position, Vector3 direction, BulletData data)
        {
            Bullet bullet = _pool.Get();
            bullet.transform.position = position;
            bullet.Init(data, direction);
        }

        public void Recycle(Bullet bullet)
        {
            _pool.Release(bullet);
        }
        */
    }
}
