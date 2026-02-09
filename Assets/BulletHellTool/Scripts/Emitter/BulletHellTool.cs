using UnityEngine;

namespace BulletHell.Emitter
{
    public class BulletHellTool : MonoBehaviour
    {
        [Header("Default")]
        public Bullet.Bullet bulletPrefab;
        public EmitterData emitterData;

        void Start()
        {
            Bullet.BulletPool pool = gameObject.AddComponent<Bullet.BulletPool>();
            pool.bulletPrefab = bulletPrefab;
            
            Emitter emitter = gameObject.AddComponent<Emitter>();
            emitter.emitterData = emitterData;
            emitter.bulletPool = pool;
        }
    }
}
