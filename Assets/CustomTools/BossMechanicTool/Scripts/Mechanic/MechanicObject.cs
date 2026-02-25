using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool
{
    public class MechanicObject: MonoBehaviour
    {
        private GameObject _target;
        private Mechanic _mechanic;
        Queue<GameObject> _telegraphVFX = new Queue<GameObject>();
        private bool isMechanicActivated = false;

        public void SetMechanic(Mechanic mechanic)
        {
            _mechanic = mechanic;
        }
        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        public void ShowTelegraph()
        {
            if (_mechanic.activationDelay <= 0)
            {
                // Show telegraph for each pattern in mechanic
                foreach (var pattern in _mechanic.patterns)
                {
                    PlayPattern(pattern, true);
                }
            }
            else
            {
                StartCoroutine(ShowTelegraph_delayed());
            }
        }

        private IEnumerator ShowTelegraph_delayed()
        {
            WaitForSeconds wait = new WaitForSeconds(_mechanic.activationDelay);
            // Show telegraph for each pattern in mechanic
            foreach (var pattern in _mechanic.patterns)
            {
                PlayPattern(pattern, true);
                yield return wait;
            }
        }

        public void ActivateMechanic()
        {
            isMechanicActivated = true;
            
            if (_mechanic.activationDelay <= 0)
            {
                // Activate all pattern in the mechanic
                foreach (var pattern in _mechanic.patterns)
                {
                    if (pattern != null && pattern.Action != null)
                    {
                        PlayPattern(pattern, false);
                    }
                }
            }
            else
            {
                StartCoroutine(ActivateMechanic_delayed());
            }
        }

        private IEnumerator ActivateMechanic_delayed()
        {
            WaitForSeconds wait = new WaitForSeconds(_mechanic.activationDelay);
            // Activate all pattern in the mechanic
            foreach (var pattern in _mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    PlayPattern(pattern, false);
                    yield return wait;
                }
            }
        }

        public void HideVFX()
        {
            if (_mechanic.activationDelay <= 0)
            {
                foreach (var vfx in _telegraphVFX)
                {
                    DestroyImmediate(vfx);
                }
                _telegraphVFX.Clear();
            }
            else
            {
                StartCoroutine(HideVFX_delayed());
            }
        }

        private IEnumerator HideVFX_delayed()
        {
            WaitForSeconds wait = new WaitForSeconds(_mechanic.activationDelay);
            foreach (var vfx in _telegraphVFX)
            {
                DestroyImmediate(vfx);
                yield return wait;
            }
            _telegraphVFX.Clear();
        }

        private void Update()
        {
            if (_target != null && isMechanicActivated == false)
            {
                transform.position = _target.transform.position;
            }
        }
        
        /*
         * Function used to spawn telegraph for the given pattern with the given spawn information
         */
        private void PlayPattern(Pattern pattern, bool isTelegraph)
        {
            // Init the origin of the pattern
            Vector3 origin = transform.position + pattern.SourceRelativePosition;
            // Init the direction of the pattern        
            Vector3 direction = pattern.SourceRelativeDirection;
            
            // Set default forward if invalid direction
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            direction.y = 0f;
            direction.Normalize();
            
            // Compute rotation according to the computed direction
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            GameObject visual;
            if (isTelegraph)
            {
                // Instantiate vfx
                visual = Instantiate(
                    pattern.TelegraphPrefab,
                    origin,
                    rotation,
                    transform
                );
                // Scale it
                visual.transform.localScale = pattern.GetActionSize();
                // Save it
                _telegraphVFX.Enqueue(visual);
            }
            else
            {
                visual = Instantiate(
                    pattern.ActivationVFX,
                    origin,
                    rotation,
                    transform
                );
                // Scale it
                visual.transform.localScale = pattern.GetActionSize();
                // activate action
                pattern.Action.ActivateAction(origin, direction);
            }
        }
    }
}