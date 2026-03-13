using System;
using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.BulletSystem
{
    [Serializable]
    public struct BulletData
    {
        public AnimationCurve speedOverLifeTime;
        public float speedMultiplier;
        public float maximumLifeTime;
    }

    public struct SpawnData
    {
        public Vector3 position;
        public Vector3 direction;
    }

    public enum ShotPattern
    {
        Arc,
        Line,
        RandomLine
    }
    
    public enum EmitterTickResult
    {
        None,
        Fire,
        Finished
    }
    
    [CreateAssetMenu(menuName = "Bullet Hell/Emitter")]
    public class EmitterData : ScriptableObject
    {
        [Header("Emitter data informations")]
        public BulletData bulletData;

        [Header("Emitter Properties")] 
        [Range(1, 100)] [Tooltip("Number of projectiles per emission")]
        public int count;

        [Range(0.01f, 2f)] [Tooltip("Time between two emission (in seconds)")]
        public float fireInterval = 0.2f;
        
        [Min(-1f)] [Tooltip("Total active time. Set less than 0 for infinite.")]
        public float duration = -1f;

        [Tooltip("If enabled, override duration to play bust count times emission.")]
        public bool isBurst = false;
        
        [Min(1)][Tooltip("Number of burst emission.")]
        public int burstCount = 1;
        
        [Header("Shape Properties")]
        public float rotationSpeed;
        public ShotPattern shotShape;
        
        [Header("Arc shape parameters")]
        public float arcAngle = 360f;
        
        [Header("Line shape parameters")]
        public float lineLength = 5f;

        private float _elapsed = 0f;
        private float _fireTimer = 0f;
        private int _currentBurstCount = 0;
        
        public EmitterTickResult OnTick(float dt)
        {
            if (isBurst == false)
            {
                _elapsed += dt;
                if (duration > 0 && _elapsed >= duration)
                    return EmitterTickResult.Finished;
            }
            else if (_currentBurstCount >= burstCount)
            {
                return EmitterTickResult.Finished;
            }

            _fireTimer += dt;
            
            if (_fireTimer >= fireInterval)
            {
                return EmitterTickResult.Fire;
            }

            return EmitterTickResult.None;
        }
        
        public void ConfirmFire()
        {
            _fireTimer -= fireInterval;

            if (isBurst)
                _currentBurstCount++;
        }

        # region Spawns Shapes
        public IEnumerable<SpawnData> GetSpawnData(float time, Transform transform)
        {
            switch (shotShape)
            {
                case ShotPattern.Line:
                    return GetLineSpawns(time, transform);
                case ShotPattern.Arc:
                default:
                    return GetArcSpawns(time, transform);
            }
        }
        
        public IEnumerable<SpawnData> GetLineSpawns(float time, Transform transform)
        {
            if (count <= 0)
                yield break;

            float rotation = time * rotationSpeed;
            Quaternion rot = Quaternion.AngleAxis(rotation, transform.up);

            Vector3 forward = rot * transform.forward;
            Vector3 right = rot * transform.right;

            // Ligne centrée sur l’émetteur
            float halfLength = lineLength * 0.5f;
            float step = count > 1 ? lineLength / (count - 1) : 0f;

            for (int i = 0; i < count; i++)
            {
                float offset = -halfLength + step * i;

                Vector3 position = transform.position + right * offset;

                yield return new SpawnData
                {
                    position = position,
                    direction = forward
                };
            }
        }
        
        public IEnumerable<SpawnData> GetArcSpawns(float time, Transform transform)
        {
            if (count <= 0 || arcAngle <= 0f)
                yield break;

            float startAngle = -arcAngle * 0.5f;
            float step = arcAngle / count;
            float rotation = time * rotationSpeed;
            
            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + step * i + rotation;
                Quaternion rot = Quaternion.AngleAxis(angle, transform.up);
                yield return new SpawnData
                {
                    position = transform.position,
                    direction = rot * transform.forward
                };
            }
        }
        #endregion
    }
}
