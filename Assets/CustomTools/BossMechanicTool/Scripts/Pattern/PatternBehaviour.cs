using System;
using UnityEngine;

namespace BossMechanicTool
{
    public class PatternBehaviour: MonoBehaviour
    {
        private GameObject _target = null;
        
        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target != null)
            {
                transform.position = _target.transform.position;
            }
        }
    }
}