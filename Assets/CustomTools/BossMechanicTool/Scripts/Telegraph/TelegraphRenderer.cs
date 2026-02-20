using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.Telegraph
{
    public class TelegraphRenderer: MonoBehaviour
    {
        private readonly List<GameObject> _decals = new();

        public void Show(List<Pattern> in_patterns, Vector3 origin)
        {
            foreach (var pattern in in_patterns)
            {
                Vector3 position = origin + pattern.SourceRelativePosition;
                
                Vector3 normal = transform.up;
                Vector3 localDirection = transform.InverseTransformDirection(pattern.SourceRelativeDirection);
                Vector3 projectedLocal = Vector3.ProjectOnPlane(localDirection, Vector3.up).normalized;
                
                if(projectedLocal.sqrMagnitude < 0.0001f)
                    projectedLocal = Vector3.right;
                
                Vector3 worldDir = transform.TransformDirection(projectedLocal);
                Vector3 yAxis = Vector3.Cross(normal, worldDir);
                
                Quaternion rotation = Quaternion.LookRotation(normal, yAxis);
                    
                var decal = Instantiate(
                    pattern.TelegraphPrefab,
                    position,
                    rotation,
                    transform
                );
                
                decal.transform.localScale = pattern.GetActionSize();

                _decals.Add(decal);
            }
        }

        public void Hide()
        {
            foreach (var decal in _decals)
                DestroyImmediate(decal.gameObject);

            _decals.Clear();
        }
    }
}