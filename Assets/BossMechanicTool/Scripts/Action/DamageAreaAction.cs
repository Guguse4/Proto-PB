using UnityEngine;

namespace BossMechanicTool.Action
{
    public enum ShapeType
    {
        Circle,
        Rectangle
    }
    
    [CreateAssetMenu(menuName = "BossMechanics/DamageAreaAction")]
    public class DamageAreaAction: PatternAction
    {
        public ShapeType shape;
        public int damage;
        
        // Circle
        public float radius;
        
        // Rectangle
        public float width;
        public float height;

        public void ActivateAction()
        {
            switch (shape)
            {
                case ShapeType.Circle:
                    break;
                case ShapeType.Rectangle:
                    break;
            }
        }
        
        public override void DrawGizmos(Vector3 origin)
        {
            switch (shape)
            {
                case ShapeType.Circle:
                    Gizmos.DrawSphere(origin, radius);
                    break;
            }
        }
    }
}