using UnityEditor;
using UnityEngine;

namespace BulletHell.Emitter
{
    [CustomEditor(typeof(BulletHellTool))]
    [CanEditMultipleObjects]
    public class EmitterDataEditor : Editor
    {
        private Editor emitterDataEditor;
        private Editor bulletDataEditor;

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
                
                // Show bullet data parameters
                if (comp.emitterData.bulletData != null)
                {
                    if (bulletDataEditor == null || bulletDataEditor.target != comp.emitterData.bulletData)
                    {
                        if (bulletDataEditor != null)
                            DestroyImmediate(bulletDataEditor);
                        bulletDataEditor = CreateEditor(comp.emitterData.bulletData);
                    }

                    bulletDataEditor.OnInspectorGUI();
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(comp);
                if (comp.emitterData != null)
                    EditorUtility.SetDirty(comp.emitterData);
                if(comp.emitterData.bulletData != null)
                    EditorUtility.SetDirty(comp.emitterData.bulletData);
            }
        }

        private void OnDisable()
        {
            if (emitterDataEditor != null)
            {
                DestroyImmediate(emitterDataEditor);
            }

            if (bulletDataEditor != null)
            {
                DestroyImmediate(bulletDataEditor);
            }
        }
    }
}