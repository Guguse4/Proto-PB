using UnityEngine;
using UnityEditor;
using BossMechanicTool.Timeline;

[CustomPropertyDrawer(typeof(SpawnBehaviour))]
public class SpawnBehaviourDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var spawnPositionBehaviour = property.FindPropertyRelative("spawnPositionBehaviour");
        var movementBehaviour = property.FindPropertyRelative("movementBehaviour");

        float height = 0f;
        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;

        // Header Position
        height += line + space;

        // spawnPositionBehaviour
        height += line + space;

        // spawnPosition (conditional)
        if ((SpawnPositionBehaviour)spawnPositionBehaviour.enumValueIndex == SpawnPositionBehaviour.OnGivenPosition
            || (SpawnPositionBehaviour)spawnPositionBehaviour.enumValueIndex == SpawnPositionBehaviour.OnNearestPlayerFrom)
        {
            // Vector 3
            height += line + space;
            // GameObject
            height += line + space;
        }

        // Header Movement
        height += line + space;

        // movementBehaviour
        height += line + space;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;

        Rect rect = new Rect(position.x, position.y, position.width, line);

        var spawnPositionBehaviour = property.FindPropertyRelative("spawnPositionBehaviour");
        var spawnPosition = property.FindPropertyRelative("spawnPosition");
        var attachedObject = property.FindPropertyRelative("attachedObject");
        var movementBehaviour = property.FindPropertyRelative("movementBehaviour");

        // ===== Position Header =====
        EditorGUI.LabelField(rect, "Position", EditorStyles.boldLabel);
        rect.y += line + space;

        // spawnPositionBehaviour
        EditorGUI.PropertyField(rect, spawnPositionBehaviour);
        rect.y += line + space;

        // spawnPosition (conditional)
        if ((SpawnPositionBehaviour)spawnPositionBehaviour.enumValueIndex == SpawnPositionBehaviour.OnGivenPosition
            || (SpawnPositionBehaviour)spawnPositionBehaviour.enumValueIndex == SpawnPositionBehaviour.OnNearestPlayerFrom)
        {
            EditorGUI.PropertyField(rect, spawnPosition);
            rect.y += line + space;
            
            EditorGUI.PropertyField(rect, attachedObject);
            rect.y += line + space;
        }

        // ===== Movement Header =====
        EditorGUI.LabelField(rect, "Movement", EditorStyles.boldLabel);
        rect.y += line + space;

        // movementBehaviour
        EditorGUI.PropertyField(rect, movementBehaviour);

        EditorGUI.EndProperty();
    }
}