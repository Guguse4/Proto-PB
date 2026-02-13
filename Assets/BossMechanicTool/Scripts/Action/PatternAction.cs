using UnityEngine;

namespace BossMechanicTool.Action
{
    public abstract class PatternAction: ScriptableObject
    {
        public virtual void DrawGizmos(Vector3 origin){}
    }
}