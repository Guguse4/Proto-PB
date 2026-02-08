using UnityEngine;

namespace Tool.Bullet.BulletMovement
{
    public abstract class BulletMovement : ScriptableObject
    {
        public abstract BulletMovementInstance Create();
    }

public abstract class BulletMovementInstance
    {
        protected Transform _transform;
        protected Vector2 _direction;
        protected float _speed;

        public virtual void Init(Transform in_transform, Vector2 in_direction, float in_speed)
        {
            _transform = in_transform;
            _direction = in_direction.normalized;
            _speed = in_speed;
        }

        public abstract void Tick(float dt);
    }
}
