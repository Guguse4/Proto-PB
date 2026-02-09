using UnityEngine;

namespace BulletHell.Bullet
{
    [CreateAssetMenu(menuName = "Bullet Hell/Bullet Data")]
    public class BulletData : ScriptableObject
    {
        [Header("Bullet Properties")] 
        public AnimationCurve speedOvertime = AnimationCurve.Constant(0f, 1f, 1f);
        public float speedMultiplier = 5f;
        public float maximumLifeTime = 10f;
    }
}
