using BossMechanicTool.Action;
using Unity.VisualScripting;
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
                Vector3 origin = pattern.SourceRelativePosition;
                if (pattern.Action is DamageAreaAction)
                {
                    DamageAreaAction action = pattern.Action as DamageAreaAction;
                    switch (action.shape)
                    {
                        case ShapeType.Circle:
                            Gizmos.DrawSphere(origin, action.radius);
                            break;
                    }
                }
            }
        }
    }
}