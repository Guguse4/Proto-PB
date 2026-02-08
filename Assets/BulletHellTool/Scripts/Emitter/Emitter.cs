using System;
using UnityEngine;

namespace Tool.Emitter
{
    public class Emitter : MonoBehaviour
    {
        public Tool.Bullet.BulletPool bulletPool;
        public EmitterData emitterData;

        void Start()
        {
            if (emitterData == null)
            {
                Debug.LogError("Emitter has no data", this);
                gameObject.SetActive(false);
                return;
            }

            emitterData.OnStart();
        }

        void Update()
        {
            emitterData.OnTick(bulletPool, transform.position, Time.deltaTime);
        }
    }
}
