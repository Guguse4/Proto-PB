using UnityEngine;

namespace BulletHell.Emitter
{
    public class Emitter : MonoBehaviour
    {
        public BulletHell.Bullet.BulletPool bulletPool;
        public EmitterData emitterData;

        void Start()
        {
            if (emitterData == null)
            {
                Debug.LogError("Emitter has no data", this);
            }

            if (bulletPool == null)
            {
                Debug.LogError("Emitter has no bullet pool", this);
            }
        }

        void Update()
        {
            emitterData.OnTick(bulletPool, transform, Time.deltaTime);
        }
    }
}
