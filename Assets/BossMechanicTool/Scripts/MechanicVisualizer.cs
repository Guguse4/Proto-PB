using UnityEngine;

namespace BossMechanicTool
{
    public class MechanicVisualizer: MonoBehaviour
    {
        public Mechanic mechanic;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
            
            Gizmos.color = Color.yellow;
            foreach(Pattern pattern in mechanic.patterns)
            {
                pattern.Action.DrawGizmos(transform.position + pattern.SourceRelativePosition);
            }
        }
    }
}