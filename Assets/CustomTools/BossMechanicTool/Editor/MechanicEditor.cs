using UnityEditor;
using UnityEngine;

namespace BossMechanicTool.Editor
{
    [CustomEditor(typeof(Mechanic))]
    public class MechanicEditor : UnityEditor.Editor
    {
        #region Pattern Layout Management
        private enum PatternLayoutType
        {
            Circle,
            Line,
            RectangleGrid,
            Arc
        }
        
        private PatternLayoutType _patternLayoutType;
        
        // Common
        private bool _orientTowardCenter = false;
        
        // Circle
        private float _circleRadius = 5f;
        
        // Line
        private float _lineSpacing = 2f;
        private Vector3 _lineDirection = Vector3.right;
        
        // Rectangle
        private int _gridColumns = 3;
        private float _gridSpacingX = 2f;
        private float _gridSpacingZ = 2f;
        
        // Arc
        private float _arcRadius = 5f;
        private float _arcAngle = 180f;
        
        // Translate
        private Vector3 _translateVector = Vector3.zero;
        #endregion
        
        #region Default properties
        SerializedProperty patternsProp;
        private SerializedProperty activationDelayProp;
        #endregion
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            Mechanic mechanic = (Mechanic)target;
            
            patternsProp = serializedObject.FindProperty("patterns");
            EditorGUILayout.PropertyField(patternsProp);
            
            activationDelayProp =  serializedObject.FindProperty("activationDelay");
            EditorGUILayout.PropertyField(activationDelayProp);
            
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pattern Layout Tool", EditorStyles.boldLabel);

            _patternLayoutType = (PatternLayoutType)EditorGUILayout.EnumPopup("Layout Type", _patternLayoutType);
            _orientTowardCenter = EditorGUILayout.Toggle("Orient Toward Center", _orientTowardCenter);

            DrawLayoutSettings();

            if (GUILayout.Button("Apply Layout"))
            {
                ApplyLayout(mechanic);
            }
            
            EditorGUILayout.Space();
            _translateVector = EditorGUILayout.Vector3Field("Translation", _translateVector);
            if (GUILayout.Button("Translate all"))
            {
                TranslateAllPatterns(mechanic);
            }
            
            // Auto display visualization
            MechanicVisualizer visualizer = FindFirstObjectByType<MechanicVisualizer>();
            if (visualizer == null)
            {
                Debug.LogError("No MechanicVisualizer found in scene. You must add MechanicManager");
                return;
            }

            visualizer.SetMechanic(mechanic);
            EditorUtility.SetDirty(visualizer);
        }
        
        private void DrawLayoutSettings()
        {
            switch (_patternLayoutType)
            {
                case PatternLayoutType.Circle:
                    _circleRadius = EditorGUILayout.FloatField("Radius", _circleRadius);
                    break;

                case PatternLayoutType.Line:
                    _lineSpacing = EditorGUILayout.FloatField("Spacing", _lineSpacing);
                    _lineDirection = EditorGUILayout.Vector3Field("Direction", _lineDirection.normalized);
                    break;

                case PatternLayoutType.RectangleGrid:
                    _gridColumns = EditorGUILayout.IntField("Columns", _gridColumns);
                    _gridSpacingX = EditorGUILayout.FloatField("Spacing X", _gridSpacingX);
                    _gridSpacingZ = EditorGUILayout.FloatField("Spacing Z", _gridSpacingZ);
                    break;

                case PatternLayoutType.Arc:
                    _arcRadius = EditorGUILayout.FloatField("Radius", _arcRadius);
                    _arcAngle = EditorGUILayout.FloatField("Angle", _arcAngle);
                    break;
            }
        }
        
        #region Layout Application Functions
        private void ApplyLayout(Mechanic mechanic)
        {
            if (mechanic.patterns == null || mechanic.patterns.Count == 0)
                return;

            Undo.RecordObject(mechanic, "Apply Pattern Layout");

            switch (_patternLayoutType)
            {
                case PatternLayoutType.Circle:
                    ApplyCircle(mechanic);
                    break;

                case PatternLayoutType.Line:
                    ApplyLine(mechanic);
                    break;

                case PatternLayoutType.RectangleGrid:
                    ApplyRectangle(mechanic);
                    break;

                case PatternLayoutType.Arc:
                    ApplyArc(mechanic);
                    break;
            }

            EditorUtility.SetDirty(mechanic);
        }
        
        private void ApplyCircle(Mechanic mechanic)
        {
            int count = mechanic.patterns.Count;
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(
                    Mathf.Cos(angle) * _circleRadius,
                    0f,
                    Mathf.Sin(angle) * _circleRadius
                );

                ApplyToPattern(mechanic, i, pos);
            }
        }

        private void ApplyLine(Mechanic mechanic)
        {
            Vector3 dir = _lineDirection.normalized;

            for (int i = 0; i < mechanic.patterns.Count; i++)
            {
                Vector3 pos = dir * _lineSpacing * i;
                ApplyToPattern(mechanic, i, pos);
            }
        }

        private void ApplyRectangle(Mechanic mechanic)
        {
            int count = mechanic.patterns.Count;

            for (int i = 0; i < count; i++)
            {
                int row = i / _gridColumns;
                int col = i % _gridColumns;

                Vector3 pos = new Vector3(
                    col * _gridSpacingX,
                    0f,
                    row * _gridSpacingZ
                );

                ApplyToPattern(mechanic, i, pos);
            }
        }

        private void ApplyArc(Mechanic mechanic)
        {
            int count = mechanic.patterns.Count;

            if (count == 1)
            {
                ApplyToPattern(mechanic, 0, Vector3.forward * _arcRadius);
                return;
            }

            float step = _arcAngle / (count - 1);
            float startAngle = -_arcAngle / 2f;

            for (int i = 0; i < count; i++)
            {
                float angle = (startAngle + step * i) * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(
                    Mathf.Sin(angle) * _arcRadius,
                    0f,
                    Mathf.Cos(angle) * _arcRadius
                );

                ApplyToPattern(mechanic, i, pos);
            }
        }

        private void ApplyToPattern(Mechanic mechanic, int index, Vector3 position)
        {
            var pattern = mechanic.patterns[index];

            pattern.SetRelativePosition(position);

            if (_orientTowardCenter)
            {
                Vector3 dir = -position.normalized;
                pattern.SetRelativeDirection(dir);
            }
        }

        private void TranslateAllPatterns(Mechanic mechanic)
        {
            foreach (var pattern in mechanic.patterns)
            {
                pattern.SetRelativePosition(pattern.SourceRelativePosition + _translateVector);
            }
        }
        #endregion
    }
}