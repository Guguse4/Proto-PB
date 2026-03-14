using UnityEngine;

namespace BossMechanicTool.BulletSystem
{
    public class Emitter : MonoBehaviour
    {
        private Bullet _bulletPrefab;
        // public BulletHell.Bullet.BulletPool bulletPool;
        private EmitterData _emitterData;

        public void Init(Bullet bulletPrefab, EmitterData emitterData)
        {
            _bulletPrefab = bulletPrefab;
            _emitterData = Instantiate(emitterData);
        }

        private void OnDisable()
        {
            _emitterData = null;
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Fire();
            _emitterData.ConfirmFire();
            
            var result = _emitterData.OnTick(dt);

            switch (result)
            {
                case EmitterTickResult.Fire:
                    Fire();
                    _emitterData.ConfirmFire();
                    break;

                case EmitterTickResult.Finished:
                    gameObject.SetActive(false);
                    break;
            }
        }
        
        private void Fire()
        {
            foreach (var spawn in _emitterData.GetSpawnData(Time.time, transform))
            {
                // bulletPool.Spawn(spawn.position, spawn.direction, _emitterData.bulletData);
                
                Bullet bullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.Init(_emitterData.bulletData, spawn.position, spawn.direction);
            }
        }
    }
}
