using BossMechanicTool.Action;
using UnityEditor;
using UnityEngine;

namespace BossMechanicTool.Editor
{
    [CustomPropertyDrawer(typeof(DamageAreaAction), true)]
    public class DamageAreaActionEditor: PropertyDrawer
    {
        SerializedProperty shapeProp;
        
        SerializedProperty damageProp;
        
        SerializedProperty radiusProp;
        
        SerializedProperty widthProp;
        SerializedProperty heightProp;
        
        SerializedProperty innerRadiusProp;
        SerializedProperty outerRadiusProp;
        
        SerializedProperty angleProp;
        
        SerializedProperty meshRefProp;
        SerializedProperty scaleProp;

        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private const float Spacing = 2f;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = LineHeight; // foldout

            if (!property.isExpanded)
                return height;

            var shapeProp = property.FindPropertyRelative("shape");
            var shape = (ShapeType)shapeProp.enumValueIndex;

            height += LineHeight + Spacing; // shape field
            height += LineHeight + Spacing; // damage field

            switch (shape)
            {
                case ShapeType.Rectangle:
                    height += (LineHeight + Spacing) * 2;
                    break;

                case ShapeType.Circle:
                    height += (LineHeight + Spacing) * 2;
                    break;

                case ShapeType.Pizza:
                    height += (LineHeight + Spacing) * 3;
                    break;

                case ShapeType.Mesh:
                    height += (LineHeight + Spacing) * 2;
                    break;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, LineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            float y = position.y + LineHeight + Spacing;

            SerializedProperty shapeProp = property.FindPropertyRelative("shape");
            SerializedProperty damageProp = property.FindPropertyRelative("damage");
            SerializedProperty widthProp = property.FindPropertyRelative("width");
            SerializedProperty heightProp = property.FindPropertyRelative("height");
            SerializedProperty innerRadiusProp = property.FindPropertyRelative("innerRadius");
            SerializedProperty outerRadiusProp = property.FindPropertyRelative("outerRadius");
            SerializedProperty angleProp = property.FindPropertyRelative("angle");
            SerializedProperty meshRefProp = property.FindPropertyRelative("meshRef");
            SerializedProperty scaleProp = property.FindPropertyRelative("scale");

            // Draw Shape
            DrawField(ref y, position, shapeProp);
            DrawField(ref y, position, damageProp);

            ShapeType shape = (ShapeType)shapeProp.enumValueIndex;

            switch (shape)
            {
                case ShapeType.Circle:
                    DrawField(ref y, position, innerRadiusProp);
                    DrawField(ref y, position, outerRadiusProp);
                    break;

                case ShapeType.Rectangle:
                    DrawField(ref y, position, widthProp);
                    DrawField(ref y, position, heightProp);
                    break;

                case ShapeType.Pizza:
                    DrawField(ref y, position, innerRadiusProp);
                    DrawField(ref y, position, outerRadiusProp);
                    DrawField(ref y, position, angleProp);
                    break;

                case ShapeType.Mesh:
                    DrawField(ref y, position, meshRefProp);
                    DrawField(ref y, position, scaleProp);
                    break;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
        
        private void DrawField(ref float y, Rect position, SerializedProperty prop)
        {
            Rect rect = new Rect(position.x, y, position.width, LineHeight);
            EditorGUI.PropertyField(rect, prop);
            y += LineHeight + Spacing;
        }
    }
}