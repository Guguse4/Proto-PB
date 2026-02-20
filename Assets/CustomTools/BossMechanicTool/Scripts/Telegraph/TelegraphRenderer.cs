using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.Telegraph
{
    public class TelegraphRenderer: MonoBehaviour
    {
        private readonly Dictionary<string, List<GameObject>> _idToDecals = new();

        public void PlayMechanic(MechanicPlayerData in_mechanicPlayerData)
        {
            if (in_mechanicPlayerData.data.activationDelay > 0f)
            {
                StartCoroutine(PlayDelayedMechanic(in_mechanicPlayerData));    
            }
            else
            {
                List<GameObject> decals = new List<GameObject>();
                foreach (var pattern in in_mechanicPlayerData.data.patterns)
                {
                    decals.Add(ShowPattern(pattern, in_mechanicPlayerData.origin, in_mechanicPlayerData.isTelegraph));
                }
                
                if (_idToDecals.ContainsKey(in_mechanicPlayerData.id))
                {
                    _idToDecals[in_mechanicPlayerData.id].AddRange(decals);
                }
                else
                {
                    _idToDecals.Add(in_mechanicPlayerData.id, decals);
                }
            }
        }

        IEnumerator PlayDelayedMechanic(MechanicPlayerData in_mechanicPlayerData)
        {
            List<GameObject> decals = new List<GameObject>();
            foreach (var pattern in in_mechanicPlayerData.data.patterns)
            {
                decals.Add(ShowPattern(pattern, in_mechanicPlayerData.origin, in_mechanicPlayerData.isTelegraph));
                yield return new WaitForSeconds(in_mechanicPlayerData.data.activationDelay);
            }

            if (_idToDecals.ContainsKey(in_mechanicPlayerData.id))
            {
                _idToDecals[in_mechanicPlayerData.id].AddRange(decals);
            }
            else
            {
                _idToDecals.Add(in_mechanicPlayerData.id, decals);
            }
        }

        private GameObject ShowPattern(Pattern pattern, Vector3 origin, bool isTelegraph)
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

                GameObject objRef = isTelegraph ? pattern.TelegraphPrefab : pattern.ActivationVFX;
                
                var decal = Instantiate(
                    objRef,
                    position,
                    Quaternion.identity,
                    transform
                );
                    
                decal.transform.localScale = pattern.GetActionSize();
                decal.gameObject.GetComponentInChildren<ParticleSystem>().Play(true);

                return decal;
        }

        public void Hide(MechanicPlayerData in_mechanicPlayerData)
        {
            if (!_idToDecals.ContainsKey(in_mechanicPlayerData.id)) 
                return;

            if (in_mechanicPlayerData.data.activationDelay > 0f)
            {
                StartCoroutine(HideDelayed(in_mechanicPlayerData));
            }
            else
            {
                foreach (var decal in _idToDecals[in_mechanicPlayerData.id])
                    DestroyImmediate(decal.gameObject);

                _idToDecals.Remove(in_mechanicPlayerData.id);
            }
        }

        private IEnumerator HideDelayed(MechanicPlayerData in_mechanicPlayerData)
        {
            foreach (var decal in _idToDecals[in_mechanicPlayerData.id])
            {
                DestroyImmediate(decal.gameObject);
                yield return new WaitForSeconds(in_mechanicPlayerData.data.activationDelay);
            }

            _idToDecals.Remove(in_mechanicPlayerData.id);
        }
    }
}