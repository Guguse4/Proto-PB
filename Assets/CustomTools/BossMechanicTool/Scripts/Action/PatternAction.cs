using UnityEngine;

namespace BossMechanicTool.Action
{
    public abstract class PatternAction
    {
        public virtual void ActivateAction(Vector3 position, Vector3 direction, MechanicObject mechanicObject) { }
        public virtual void DrawGizmos(Vector3 position, Vector3 direction){}
        public virtual SizeInformation GetSize(){return new SizeInformation();}
        
        public virtual void StopAction(){}
    }
}