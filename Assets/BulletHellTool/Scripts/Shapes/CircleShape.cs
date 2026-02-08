using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Hell/Shape/Circle")]
public class CirlceShape: BulletShape
{
    public float startAngle;
    
    public override IEnumerable<Vector2> GetDirections(int count, float time)
    {
        if (count <= 0)
            yield break;

        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + step * i;
            float rad = angle * Mathf.Deg2Rad;

            yield return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }
    }
}
