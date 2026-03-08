using UnityEditor;
using UnityEngine;
using BossMechanicTool.Action;

namespace BossMechanicTool.Editor
{
    [CustomPropertyDrawer(typeof(Pattern))]
    public class PatternEditor : PropertyDrawer
    {
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private const float Spacing = 2f;
        private bool firstInitHappened = false;

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

            height += LineHeight + Spacing; // Boolean
            height += LineHeight + Spacing; // Telegraph
            height += LineHeight + Spacing; // activation vfx

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, LineHeight);
            
            SerializedProperty actionTypePropTpm = property.FindPropertyRelative("_actionType");
            ActionType typeTpm = (ActionType)actionTypePropTpm.enumValueIndex;
            
            Color textColor = GetColor(typeTpm);
            GUIStyle coloredFoldout = new GUIStyle(EditorStyles.foldout);
            coloredFoldout.normal.textColor = textColor;
            coloredFoldout.onNormal.textColor = textColor;
            coloredFoldout.focused.textColor = textColor;
            coloredFoldout.onFocused.textColor = textColor;
            
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true, coloredFoldout);
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
            SerializedProperty useDefaultTelegraphProp = property.FindPropertyRelative("_useDefaultTelegraph");
            SerializedProperty telegraphProp = property.FindPropertyRelative("_telegraphPrefab");
            SerializedProperty activationVFXProp = property.FindPropertyRelative("_activationVFX");

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
            y = DrawField(position, y, useDefaultTelegraphProp);
            using (new EditorGUI.DisabledScope(useDefaultTelegraphProp.boolValue))
            {
                y = DrawField(position, y, telegraphProp);
                y = DrawField(position, y, activationVFXProp);
            }
            
            EditorGUI.indentLevel--;
            
            EditorGUI.EndProperty();
        }

        private SerializedProperty GetActionProperty(SerializedProperty property, ActionType type)
        {
            switch (type)
            {
                case ActionType.DamageArea:
                    SerializedProperty damageAreaProp = property.FindPropertyRelative("_damageArea");
                    SerializedProperty telegraphProp = property.FindPropertyRelative("_telegraphPrefab");
                    SerializedProperty activationProp = property.FindPropertyRelative("_activationVFX");
                    if (property.FindPropertyRelative("_useDefaultTelegraph").boolValue)
                    {
                        SerializedProperty shape = damageAreaProp.FindPropertyRelative("shape");
                        string basePath = "Assets/CustomTools/BossMechanicTool/Prefabs/DefaultTelegraph/";
                        string objectPath = "DefaultCircleTelegraph.prefab";
                        switch ((ShapeType)shape.enumValueIndex)
                        {
                            case ShapeType.Rectangle:
                                objectPath = "DefaultRectangleTelegraph.prefab";
                                break;
                        }
                        telegraphProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>(basePath+objectPath);
                        activationProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CustomTools/BossMechanicTool/Prefabs/DefaultTelegraph/OurExplosion.prefab");
                    }
                    
                    return damageAreaProp;

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

        private Color GetColor(ActionType type)
        {
            switch (type)
            {
                case ActionType.DamageArea:
                    return Color.lightGreen;
                case ActionType.SpawnMob:
                    return Color.aquamarine;
                case ActionType.Bullet:
                    return Color.darkGoldenRod;
                default:
                    return Color.white;
            }
        }
    }
}
