using System.Collections.Generic;
using BulletHell.Emitter;
using UnityEngine;

namespace BulletHellTool.Telegraph
{
    public class TelegraphRenderer: MonoBehaviour
    {
        private GameObject _decalPrefab;
        private readonly List<GameObject> _decals = new();

        public void SetDecalPrefab(GameObject in_decalPrefab)
        {
            _decalPrefab = in_decalPrefab;
        }

        public void Show(IEnumerable<SpawnData> spawns)
        {
            Hide();
            
            foreach (var spawn in spawns)
            {
                Vector3 normal = transform.up;
                Vector3 localDirection = transform.InverseTransformDirection(spawn.direction);
                Vector3 projectedLocal = Vector3.ProjectOnPlane(localDirection, Vector3.up).normalized;
                
                if(projectedLocal.sqrMagnitude < 0.0001f)
                    projectedLocal = Vector3.right;
                
                Vector3 worldDir = transform.TransformDirection(projectedLocal);
                Vector3 yAxis = Vector3.Cross(normal, worldDir);
                
                Quaternion rotation = Quaternion.LookRotation(normal, yAxis);
                    
                var decal = Instantiate(
                    _decalPrefab,
                    spawn.position,
                    rotation,
                    transform
                );

                _decals.Add(decal);
            }
        }

        public void Hide()
        {
            foreach (var decal in _decals)
                Destroy(decal.gameObject);

            _decals.Clear();
        }
    }
}