using System.Collections;
using System.Collections.Generic;
using BossMechanicTool.VFX;
using UnityEngine;

namespace BossMechanicTool
{
    [RequireComponent(typeof(VFXPlayer))]
    public class MechanicPlayer: MonoBehaviour
    {
        // Tool to draw telegraph
        private VFXPlayer _vfxPlayer;
        
        private Dictionary<string, Mechanic> _idToMechanic = new Dictionary<string, Mechanic>();
        
        private void Start()
        {
            _vfxPlayer = GetComponent<VFXPlayer>();
        }

        public void ShowMechanicTelegraph(string id, Mechanic mechanic)
        {
            #if UNITY_EDITOR
            
            if(_vfxPlayer == null)
                _vfxPlayer = GetComponent<VFXPlayer>();
            
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    _vfxPlayer.ShowTelegraphPattern(id, pattern, mechanic.spawnPosition, null);
                }
            }
        }

        private IEnumerator ShowMechanicTelegraphDelayed(string id, Mechanic mechanic)
        {
            foreach (var pattern in mechanic.patterns)
            {
                _vfxPlayer.ShowTelegraphPattern(id, pattern, mechanic.spawnPosition, null);
                yield return new WaitForSeconds(mechanic.activationDelay);
            }
        }

        public void ActivateMechanic(string id, Mechanic mechanic)
        {
            #if UNITY_EDITOR
            
            if(_vfxPlayer == null)
                _vfxPlayer = GetComponent<VFXPlayer>();
            
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ActivateMechanicDelayed(id, mechanic));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    if (pattern != null && pattern.Action != null)
                    {
                        pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition,
                            pattern.SourceRelativeDirection);
                        _vfxPlayer.ShowActivationPattern(id, pattern, mechanic.spawnPosition, null);
                    }
                }
            }
        }

        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic)
        {
            foreach (var pattern in mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition,
                        pattern.SourceRelativeDirection);
                    _vfxPlayer.ShowActivationPattern(id, pattern, mechanic.spawnPosition, null);
                    yield return new WaitForSeconds(mechanic.activationDelay);
                }
            }
        }
        
        public void HideMechanic(string id)
        {
            #if UNITY_EDITOR
            
            if(_vfxPlayer == null)
                _vfxPlayer = GetComponent<VFXPlayer>();
            
            #endif
            
            if (_idToMechanic.ContainsKey(id))
            {
                Mechanic mechanic = _idToMechanic[id];

                if (mechanic.activationDelay > 0f)
                {
                    StartCoroutine(HideMechanicDelayed(id, mechanic));
                }
                else
                {
                    foreach (var pattern in mechanic.patterns)
                    {
                        _vfxPlayer.HidePattern(id);
                    }
                    _idToMechanic.Remove(id);
                }
                
                
            }
        }

        private IEnumerator HideMechanicDelayed(string id, Mechanic mechanic)
        {
            foreach (var pattern in mechanic.patterns)
            {
                _vfxPlayer.HidePattern(id);
                yield return new WaitForSeconds(mechanic.activationDelay);
            }
            _idToMechanic.Remove(id);
        }

        private void RegisterMechanic(string id, Mechanic mechanic)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                _idToMechanic.Add(id, mechanic);
            }
        }
    }
}