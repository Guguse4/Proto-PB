using System;
using BulletHellTool.Telegraph;
using UnityEngine;

namespace BulletHell.Emitter
{
    public class Emitter : MonoBehaviour
    {
        public BulletHell.Bullet.BulletPool bulletPool;
        public EmitterData emitterDataBase;
        public TelegraphRenderer telegraphRenderer;
        
        private bool _isTelegraphing = false;
        private float _telegraphTimer = 0f;
        private EmitterData _emitterData;

        void Start()
        {
            if (emitterDataBase == null)
            {
                Debug.LogError("Emitter has no data", this);
            }
            else
            {
                emitterDataBase.ResetState();
            }

            if (telegraphRenderer == null)
            {
                Debug.LogError("Emitter has no telegraph renderer", this);
            }
            else
            {
                telegraphRenderer.SetDecalPrefab(emitterDataBase.telegraphPrefab);
            }

            if (bulletPool == null)
            {
                Debug.LogError("Emitter has no bullet pool", this);
            }
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _emitterData = Instantiate(emitterDataBase);
        }

        private void OnDisable()
        {
            _emitterData = null;
        }

        void Update()
        {
            float dt = Time.deltaTime;
            if (_isTelegraphing)
            {
                _telegraphTimer += dt;

                if (_telegraphTimer >= _emitterData.telegraphDuration)
                {
                    telegraphRenderer.Hide();
                    Fire();
                    _emitterData.ConfirmFire();

                    _isTelegraphing = false;
                    _telegraphTimer = 0f;
                }

                return;
            }
            
            var result = _emitterData.OnTick(dt);

            switch (result)
            {
                case EmitterTickResult.Telegraph:
                    StartTelegraph();
                    break;

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
                bulletPool.Spawn(spawn.position, spawn.direction, _emitterData.bulletData);
            }
        }

        private void StartTelegraph()
        {
            _isTelegraphing = true;
            _telegraphTimer = 0f;

            var spawns = _emitterData.GetSpawnData(Time.time, transform);
            telegraphRenderer.Show(spawns);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            var spawns = emitterDataBase.GetSpawnData(Time.time, transform);
            foreach (var spawn in spawns)
            {
                Gizmos.DrawLine(spawn.position, spawn.position + spawn.direction);
                Gizmos.DrawSphere(spawn.position, 0.1f);
                Gizmos.DrawSphere(spawn.position + spawn.direction, 0.1f);
            }
        }
    }
}
