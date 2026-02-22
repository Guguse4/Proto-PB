using System.Collections;
using System.Collections.Generic;
using BossMechanicTool.Timeline;
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
        
        /*
         * Function called to draw mechanic telegraph phase
         * id: unique mechanic id for save
         * mechanic: mechanic data to use
         * data: origin position, direction and target
         */
        public void ShowMechanicTelegraph(string id, Mechanic mechanic, BehavioursData data)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            #endif

            // Register mechanic in dictionary to use it latter
            RegisterMechanic(id, mechanic);
            
            // Switch activation delay, start coroutine or execute immediately
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic, data));
            }
            else
            {
                // Show telegraph for each pattern in mechanic
                foreach (var pattern in mechanic.patterns)
                {
                    ShowPatternTelegraph(id, pattern, data);
                }
            }
        }

        /*
         * Delayed version of the function above
         */
        private IEnumerator ShowMechanicTelegraphDelayed(string id, Mechanic mechanic, BehavioursData data)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                ShowPatternTelegraph(id, pattern, data);
                yield return wait;
            }
        }

        private void ShowPatternTelegraph(string id, Pattern pattern, BehavioursData data)
        {
            Transform parent = null;
            Vector3 origin = pattern.SourceRelativePosition;
            if (data.target != null)
            {
                parent = data.target.transform;
                origin += parent.position;
            }
            else
            {
                origin += data.position;
            }
                    
            Vector3 direction = data.rotation + pattern.SourceRelativeDirection;
            
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                
            _vfxPlayer.ShowVfx(id, pattern.TelegraphPrefab, origin, rotation, pattern.GetActionSize(), parent);
        }
        #endregion

        #region Activation
        public void ActivateMechanic(string id, Mechanic mechanic, BehavioursData data)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ActivateMechanicDelayed(id, mechanic, data));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    if (pattern != null && pattern.Action != null)
                    {
                        ActivatePattern(id, pattern, data);
                    }
                }
            }
        }

        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic, BehavioursData data)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    ActivatePattern(id, pattern, data);
                    // wait delay
                    yield return wait;
                }
            }
        }

        private void ActivatePattern(string id, Pattern pattern, BehavioursData data)
        {
            Vector3 origin = pattern.SourceRelativePosition;
            if (data.target != null)
            {
                origin += data.target.transform.position;
            }
            else
            {
                origin += data.position;
            }
            
            Vector3 direction = data.rotation + pattern.SourceRelativeDirection;
            
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                        
            // activate action
            pattern.Action.ActivateAction(origin, direction);
            // play vfx
            _vfxPlayer.ShowVfx(id, pattern.ActivationVFX, origin, rotation, pattern.GetActionSize(), null);
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