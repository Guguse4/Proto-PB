using UnityEngine;

namespace BossMechanicTool
{
    public class MechanicVisualizer: MonoBehaviour
    {
        #if UNITY_EDITOR
        
        public Mechanic mechanic;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);
            
            Gizmos.color = Color.yellow;
            foreach(Pattern pattern in mechanic.patterns)
            {
                if(pattern != null && pattern.Action != null)
                    pattern.Action.DrawGizmos(mechanic.spawnPosition + pattern.SourceRelativePosition, pattern.SourceRelativeDirection);
            }
        }
        
        #endif
    }
}