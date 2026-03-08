using UnityEngine;

namespace BossMechanicTool
{
    public class MechanicVisualizer: MonoBehaviour
    {
        #if UNITY_EDITOR
        
        private Mechanic _mechanic;

        public void SetMechanic(Mechanic in_mechanic)
        {
            _mechanic = in_mechanic;
        }

        void OnDrawGizmos()
        {
            if (_mechanic == null || _mechanic.patterns == null)
                return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);
            
            Gizmos.color = Color.yellow;
            foreach(Pattern pattern in _mechanic.patterns)
            {
                if(pattern != null && pattern.Action != null)
                    pattern.Action.DrawGizmos(transform.position + pattern.SourceRelativePosition, pattern.SourceRelativeDirection);
            }
        }
        
        #endif
    }
}