using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Hell/Shape/Spiral")]
public class SpiralShape: BulletShape
{
    public float rotationSpeed;

    public override IEnumerable<Vector2> GetDirections(int count, float time)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i + time * rotationSpeed;
            yield return Quaternion.Euler(0, 0, angle) * Vector2.up;
        }
    }
}
