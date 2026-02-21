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

        #region Telegraph
        public void ShowMechanicTelegraph(string id, Mechanic mechanic, Vector3 origin, GameObject target)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic, origin, target));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    _vfxPlayer.ShowTelegraphPattern(id, pattern, origin, target); 
                }
            }
        }

        private IEnumerator ShowMechanicTelegraphDelayed(string id, Mechanic mechanic, Vector3 origin, GameObject target)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                _vfxPlayer.ShowTelegraphPattern(id, pattern, origin, target);
                yield return wait;
            }
        }
        #endregion

        #region Activation
        public void ActivateMechanic(string id, Mechanic mechanic, Vector3 origin)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ActivateMechanicDelayed(id, mechanic, origin));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    if (pattern != null && pattern.Action != null)
                    {
                        // activate action
                        pattern.Action.ActivateAction(origin, pattern.SourceRelativeDirection);
                        // play vfx
                        _vfxPlayer.ShowActivationPattern(id, pattern, origin);
                    }
                }
            }
        }

        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic, Vector3 origin)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    // activate action
                    pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition,
                        pattern.SourceRelativeDirection);
                    // play vfx
                    _vfxPlayer.ShowActivationPattern(id, pattern, origin);
                    // wait delay
                    yield return wait;
                }
            }
        }
        #endregion
        
        #region Hide
        public void HideMechanic(string id)
        {
            #if UNITY_EDITOR
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
                    int count = _vfxPlayer.GetVFXNumber(id);
                    for(int i = 0; i < count; i++)
                    {
                        _vfxPlayer.HidePattern(id);
                    }
                    _idToMechanic.Remove(id);
                }
            }
        }

        private IEnumerator HideMechanicDelayed(string id, Mechanic mechanic)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            int count = _vfxPlayer.GetVFXNumber(id);
            for(int i = 0; i < count; i++)
            {
                _vfxPlayer.HidePattern(id);
                yield return wait;
            }
            _idToMechanic.Remove(id);
        }
        #endregion

        private void RegisterMechanic(string id, Mechanic mechanic)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                _idToMechanic.Add(id, mechanic);
            }
        }
    }
}