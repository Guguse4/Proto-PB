
using UnityEngine;

namespace Tool.Bullet.BulletMovement
{
    [CreateAssetMenu(menuName = "Bullet Hell/Movement/Linear")]
    public class LinearMovement : BulletMovement
    {
        public override BulletMovementInstance Create()
        {
            return new LinearMovementInstance();
        }
    }

    public class LinearMovementInstance : BulletMovementInstance
    {
        public override void Tick(float dt)
        {
            if (_transform == null)
            {
                Debug.LogError("LinearMovementInstance: transform is null");
                return;
            }

            _transform.position += (Vector3)(_direction * _speed * dt);
        }
    }
}
