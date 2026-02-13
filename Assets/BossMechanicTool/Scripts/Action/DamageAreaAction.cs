using System;
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
        [NonSerialized]
        public float radius;
        
        // Rectangle
        [NonSerialized]
        public float width;
        [NonSerialized]
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
    }
}