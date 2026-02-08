using System.Collections.Generic;
using UnityEngine;
using Tool.Bullet.BulletMovement;
using Tool.Bullet.BulletBehaviour;

namespace Tool.Bullet
{
    [CreateAssetMenu(menuName = "Bullet Hell/Bullet Data")]
    public class BulletData : ScriptableObject
    {
        public float speed = 5f;
        public float maximumLifeTime = 10f;

        public BulletMovement.BulletMovement movement;
        public List<BulletBehaviour.BulletBehaviour> behaviours;

        public BulletMovementInstance CreateMovement()
        {
            return movement.Create();
        }

        public List<BulletBehaviour.BulletBehaviour> CreateBehaviours()
        {
            return new List<BulletBehaviour.BulletBehaviour>(behaviours);
        }
    }
}
