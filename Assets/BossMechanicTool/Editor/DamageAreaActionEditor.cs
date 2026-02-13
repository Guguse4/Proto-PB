using BossMechanicTool.Action;
using UnityEditor;

namespace BossMechanicTool.Editor
{
    [CustomEditor(typeof(DamageAreaAction))]
    public class DamageAreaActionEditor: UnityEditor.Editor
    {
        SerializedProperty shapeProp;
        SerializedProperty radiusProp;
        SerializedProperty widthProp;
        SerializedProperty heightProp;

        private void OnEnable()
        {
            shapeProp = serializedObject.FindProperty("shape");
            radiusProp = serializedObject.FindProperty("radius");
            widthProp = serializedObject.FindProperty("width");
            heightProp = serializedObject.FindProperty("height");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(shapeProp);

            ShapeType shape = (ShapeType)shapeProp.enumValueIndex;
            switch (shape)
            {
                case ShapeType.Circle:
                    EditorGUILayout.PropertyField(radiusProp);
                    break;

                case ShapeType.Rectangle:
                    EditorGUILayout.PropertyField(widthProp);
                    EditorGUILayout.PropertyField(heightProp);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}