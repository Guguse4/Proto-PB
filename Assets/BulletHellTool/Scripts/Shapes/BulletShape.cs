using System.Collections.Generic;
using UnityEngine;

public abstract class BulletShape: ScriptableObject
{
    public abstract IEnumerable<Vector2> GetDirections(int count, float time);
}
