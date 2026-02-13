using BossMechanicTool.Action;
using UnityEditor;
using UnityEngine;

namespace BossMechanicTool.Editor
{
    [CustomEditor(typeof(DamageAreaAction))]
    public class DamageAreaActionEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            DamageAreaAction damageAreaAction = (DamageAreaAction)target;

            switch (damageAreaAction.shape)
            {
                case ShapeType.Circle:
                    damageAreaAction.radius = EditorGUILayout.FloatField("Radius: ", damageAreaAction.radius);
                    break;
                case ShapeType.Rectangle:
                    damageAreaAction.width = EditorGUILayout.FloatField("Width: ", damageAreaAction.width);
                    damageAreaAction.height = EditorGUILayout.FloatField("Height: ", damageAreaAction.height);
                    break;
            }
            
            if (GUI.changed)
                EditorUtility.SetDirty(damageAreaAction);
        }
    }
}