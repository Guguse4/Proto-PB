using System;
using UnityEngine;

namespace BossMechanicTool.Action
{
    // Available shape to define the damage area
    public enum ShapeType
    {
        Circle,
        Rectangle,
        Donut,
        Pizza,
        Mesh
    }
    
    /*
     * Data class to define a damage area pattern action
     */
    [Serializable]
    public class DamageAreaAction: PatternAction
    {
        // The shape of the damage area
        public ShapeType shape;
        
        // The damage amount of the damage area
        public int damage = 1;
        
        // Parameters according to the given shape
        // Circle parameters
        public float radius;
        
        // Rectangle parameters
        public float width;
        public float height;
        
        // Donut
        public float innerRadius;
        public float outerRadius;
        
        // Pizza (also use radius)
        public float angle;
        
        // Mesh
        public Mesh meshRef;
        public Vector3 scale;

        // Return the pattern action shape according to the current shape
        public override Vector3 GetSize()
        {
            switch (shape)
            {
                case ShapeType.Circle:
                    return new Vector3(radius, 1, radius);
                case ShapeType.Rectangle:
                    return new Vector3(width, 1, height);
                default:
                    return Vector3.one;
            }
        }

        #region Activation
        /*
         * Function called when mechanic containing this pattern action need to activate it
         */
        public override void ActivateAction(Vector3 position, Vector3 direction)
        {
            // Activation according to the shape
            switch (shape)
            {
                case ShapeType.Circle:
                    ActivateCircle(position);
                    break;
                case ShapeType.Rectangle:
                    ActivateRectangle(position, direction);
                    break;
                case ShapeType.Donut:
                    ActivateDonut(position);
                    break;
                case ShapeType.Pizza:
                    ActivatePizza(position, direction);
                    break;
            }
        }
        
        /*
         *  Find players in circle range and apply damage 
         */
        private void ActivateCircle(Vector3 origin)
        {
            Collider[] hits = Physics.OverlapSphere(origin, radius);
            ApplyDamage(hits);
        }

        /*
         *  Find players in rectangle range and apply damage
         */
        private void ActivateRectangle(Vector3 origin, Vector3 direction)
        {
            Vector3 halfExtents = new Vector3(width/2f, 1f, height/2f);
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            Collider[] hits = Physics.OverlapBox(origin, halfExtents, rotation);
            ApplyDamage(hits);
        }

        /*
         *  Find players in donut range and apply damage
         */
        private void ActivateDonut(Vector3 origin)
        {
            Collider[] hits = Physics.OverlapSphere(origin, radius);
            foreach (var col in hits)
            {
                float dist = Vector3.Distance(origin, col.transform.position);
                if (dist >= innerRadius)
                {
                    ApplyDamage(col);
                }
            }
        }

        /*
         *  Find players in pizza range and apply damage
         */
        private void ActivatePizza(Vector3 origin,  Vector3 direction)
        {
            Collider[] hits = Physics.OverlapSphere(origin, radius);

            foreach (var col in hits)
            {
                Vector3 dir = (col.transform.position - origin).normalized;
                float dot = Vector3.Dot(direction.normalized, dir);
                float currentAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (currentAngle <= angle * 0.5f)
                {
                    ApplyDamage(col);
                }
            }
        }

        #endregion
        
        #region Damage

        /*
         * Apply damage to all given players
         */
        private void ApplyDamage(Collider[] colliders)
        {
            foreach (var col in colliders)
                ApplyDamage(col);
        }
        
        /*
         * Apply damage to given player
         */
        private void ApplyDamage(Collider col)
        {
            Debug.Log("Collider : " + col.gameObject.name);
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(10); //damage
            }
        }
        
        #endregion
        
        #region Gizmos

        public override void DrawGizmos(Vector3 position, Vector3 direction)
        {
            switch (shape)
            {
                case ShapeType.Circle:
                    DrawCircle(position);
                    break;
                case ShapeType.Rectangle:
                    DrawRectangle(position, direction);
                    break;
                case ShapeType.Donut:
                    DrawDonut(position);
                    break;
                case ShapeType.Pizza:
                    DrawPizza(position, direction);
                    break;
                case ShapeType.Mesh:
                    DrawMesh(position, direction);
                    break;
                    
            }
        }

        private void DrawCircle(Vector3 position)
        {
            Gizmos.DrawWireSphere(position, radius);
        }

        private void DrawRectangle(Vector3 origin, Vector3 direction)
        {
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 0.1f, height));

            Gizmos.matrix = oldMatrix;
        }

        private void DrawDonut(Vector3 origin)
        {
            Gizmos.DrawWireSphere(origin, outerRadius);
            Gizmos.DrawWireSphere(origin, innerRadius);
        }

        private void DrawPizza(Vector3 origin, Vector3 direction)
        {
            if (direction == Vector3.zero)
                direction = Vector3.forward;

            direction.y = 0f;
            direction.Normalize();

            float halfAngle = angle * 0.5f;
            int segments = 40;

            Quaternion baseRotation = Quaternion.LookRotation(direction, Vector3.up);

            Vector3 previousPoint = origin + (baseRotation * Quaternion.Euler(0, -halfAngle, 0) * Vector3.forward) * radius;

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -halfAngle + (angle / segments) * i;

                Vector3 nextPoint =
                    origin +
                    (baseRotation * Quaternion.Euler(0, currentAngle, 0) * Vector3.forward) * radius;

                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }

            // Draw side lines
            Vector3 leftDir = baseRotation * Quaternion.Euler(0, -halfAngle, 0) * Vector3.forward;
            Vector3 rightDir = baseRotation * Quaternion.Euler(0, halfAngle, 0) * Vector3.forward;

            Gizmos.DrawLine(origin, origin + leftDir * radius);
            Gizmos.DrawLine(origin, origin + rightDir * radius);
        }

        private void DrawMesh(Vector3 origin, Vector3 direction)
        {
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            Gizmos.DrawWireMesh(meshRef, origin, rotation, scale);
        }

        #endregion
    }
}