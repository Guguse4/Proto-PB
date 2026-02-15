using UnityEditor;
using UnityEngine;
using BossMechanicTool;

namespace BossMechanicTool.Editor
{
    [CustomPropertyDrawer(typeof(Pattern))]
    public class PatternEditor : PropertyDrawer
    {
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = LineHeight; // Foldout

            if (!property.isExpanded)
                return height;

            height += LineHeight + Spacing; // Position
            height += LineHeight + Spacing; // Direction
            height += LineHeight + Spacing; // ActionType

            SerializedProperty actionTypeProp = property.FindPropertyRelative("_actionType");
            ActionType type = (ActionType)actionTypeProp.enumValueIndex;

            SerializedProperty actionProp = GetActionProperty(property, type);

            if (actionProp != null)
            {
                height += EditorGUI.GetPropertyHeight(actionProp, true) + Spacing;
            }

            height += LineHeight + Spacing; // Telegraph

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

            SerializedProperty positionProp = property.FindPropertyRelative("_sourceRelativePosition");
            SerializedProperty directionProp = property.FindPropertyRelative("_sourceRelativeDirection");
            SerializedProperty actionTypeProp = property.FindPropertyRelative("_actionType");
            SerializedProperty telegraphProp = property.FindPropertyRelative("_telegraphPrefab");

            // Position
            y = DrawField(position, y, positionProp);

            // Direction
            y = DrawField(position, y, directionProp);

            // ActionType
            y = DrawField(position, y, actionTypeProp);

            // Draw corresponding action
            ActionType type = (ActionType)actionTypeProp.enumValueIndex;
            SerializedProperty actionProp = GetActionProperty(property, type);

            if (actionProp != null)
            {
                float actionHeight = EditorGUI.GetPropertyHeight(actionProp, true);
                Rect actionRect = new Rect(position.x, y, position.width, actionHeight);
                EditorGUI.PropertyField(actionRect, actionProp, true);
                y += actionHeight + Spacing;
            }

            // Telegraph
            DrawField(position, y, telegraphProp);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        private SerializedProperty GetActionProperty(SerializedProperty property, ActionType type)
        {
            switch (type)
            {
                case ActionType.DamageArea:
                    return property.FindPropertyRelative("_damageArea");

                case ActionType.SpawnMob:
                    return property.FindPropertyRelative("_spawnMob");

                case ActionType.Bullet:
                    return property.FindPropertyRelative("_bullet");

                default:
                    return null;
            }
        }

        private float DrawField(Rect position, float y, SerializedProperty prop)
        {
            Rect rect = new Rect(position.x, y, position.width, LineHeight);
            EditorGUI.PropertyField(rect, prop, true);
            return y + LineHeight + Spacing;
        }
    }
}
