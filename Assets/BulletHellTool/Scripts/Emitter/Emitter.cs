using BulletHellTool.Telegraph;
using UnityEngine;

namespace BulletHell.Emitter
{
    public class Emitter : MonoBehaviour
    {
        public BulletHell.Bullet.BulletPool bulletPool;
        public EmitterData emitterData;
        public TelegraphRenderer telegraphRenderer;
        
        private bool _isTelegraphing = false;
        private float _telegraphTimer = 0f;

        void Start()
        {
            if (emitterData == null)
            {
                Debug.LogError("Emitter has no data", this);
            }

            emitterData.ResetState();

            if (bulletPool == null)
            {
                Debug.LogError("Emitter has no bullet pool", this);
            }
        }

        void Update()
        {
            float dt = Time.deltaTime;
            if (_isTelegraphing)
            {
                _telegraphTimer += dt;

                if (_telegraphTimer >= emitterData.telegraphDuration)
                {
                    telegraphRenderer.Hide();
                    Fire();
                    emitterData.ConfirmFire();

                    _isTelegraphing = false;
                    _telegraphTimer = 0f;
                }

                return;
            }
            
            var result = emitterData.OnTick(dt);

            switch (result)
            {
                case EmitterTickResult.Telegraph:
                    StartTelegraph();
                    break;

                case EmitterTickResult.Fire:
                    Fire();
                    emitterData.ConfirmFire();
                    break;

                case EmitterTickResult.Finished:
                    gameObject.SetActive(false);
                    break;
            }
        }
        
        private void Fire()
        {
            foreach (var spawn in emitterData.GetSpawnData(Time.time, transform))
            {
                bulletPool.Spawn(spawn.position, spawn.direction, emitterData.bulletData);
            }
        }

        private void StartTelegraph()
        {
            _isTelegraphing = true;
            _telegraphTimer = 0f;

            var spawns = emitterData.GetSpawnData(Time.time, transform);
            telegraphRenderer.Show(spawns);
        }
    }
}
