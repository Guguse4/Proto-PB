using UnityEditor;
using UnityEngine;
using Tool.Emitter;

namespace Tool
{
    [CustomEditor(typeof(BulletHellTool))]
    [CanEditMultipleObjects]
    public class EmitterDataEditor : Editor
    {
        private Editor emitterDataEditor; // éditeur interne pour le ScriptableObject

        public override void OnInspectorGUI()
        {
            BulletHellTool comp = (BulletHellTool)target;

            // Champ pour assigner le ScriptableObject
            comp.emitterData = (EmitterData)EditorGUILayout.ObjectField(
                "Emitter Data", comp.emitterData, typeof(EmitterData), false);

            // Si un ScriptableObject est assigné, affiche ses propriétés dans l'inspecteur
            if (comp.emitterData != null)
            {
                // Crée un éditeur interne pour le ScriptableObject s'il n'existe pas ou si le SO a changé
                if (emitterDataEditor == null || emitterDataEditor.target != comp.emitterData)
                {
                    if (emitterDataEditor != null)
                        DestroyImmediate(emitterDataEditor);
                    emitterDataEditor = CreateEditor(comp.emitterData);
                }

                // Dessine le ScriptableObject
                emitterDataEditor.OnInspectorGUI();

                Debug.Log("Here");
            }

            // Dessine le reste du script (si tu avais d'autres champs)
            DrawDefaultInspector();

            // Marque les objets comme modifiés si GUI a changé
            if (GUI.changed)
            {
                EditorUtility.SetDirty(comp);
                if (comp.emitterData != null)
                    EditorUtility.SetDirty(comp.emitterData);
            }
        }

        private void OnDisable()
        {
            // Nettoyage pour éviter les fuites de mémoire
            if (emitterDataEditor != null)
            {
                DestroyImmediate(emitterDataEditor);
            }
        }
    }
}