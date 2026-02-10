using UnityEditor;
using UnityEngine;

namespace BulletHell.Emitter
{
    [CustomEditor(typeof(BulletHellTool))]
    [CanEditMultipleObjects]
    public class BulletHellTooEditor : Editor
    {
        private Editor emitterDataEditor;

        public override void OnInspectorGUI()
        {
            BulletHellTool comp = (BulletHellTool)target;
            
            DrawDefaultInspector();
            
            // If there is emitter data linked in the tool
            if (comp.emitterData != null)
            {
                // Show emitter data parameters
                if (emitterDataEditor == null || emitterDataEditor.target != comp.emitterData)
                {
                    if (emitterDataEditor != null)
                        DestroyImmediate(emitterDataEditor);
                    emitterDataEditor = CreateEditor(comp.emitterData);
                }
                emitterDataEditor.OnInspectorGUI();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(comp);
                if (comp.emitterData != null)
                    EditorUtility.SetDirty(comp.emitterData);
            }
        }

        private void OnDisable()
        {
            if (emitterDataEditor != null)
            {
                DestroyImmediate(emitterDataEditor);
            }
        }
    }
}