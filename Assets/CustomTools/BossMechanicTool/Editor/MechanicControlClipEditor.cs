using BossMechanicTool.Scripts.Timeline.Boss;
using BossMechanicTool.Timeline.Mechanic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BossMechanicTool.Editor
{
    [CustomTimelineEditor(typeof(MechanicControlClip))]
    public class MechanicControlClipEditor: ClipEditor
    {
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            MechanicControlClip mechanicControlClip = clip.asset as MechanicControlClip;
            if (mechanicControlClip == null)
            {
                return;
            }
            MechanicControlBehaviour behaviour = mechanicControlClip.GetTemplate();
            
            Rect rect = region.position;
            float y = rect.y + 3 * rect.height / 4;
            float height = rect.height/4;

            Rect telegraph = new Rect(rect.x, y, rect.width * behaviour.TelegraphDuration, height);
            EditorGUI.DrawRect(telegraph, Color.darkOliveGreen);
            
            if (behaviour.SpawnBehaviour.movementBehaviour == MovementBehaviour.FollowSpawnPositionObject)
            {
                Rect follow = new Rect(
                    rect.x, y, 
                    rect.width * behaviour.FollowDuration * behaviour.TelegraphDuration, height);
                EditorGUI.DrawRect(follow, Color.aquamarine);
            }

            
            Rect activation = new Rect(rect.x + rect.width * behaviour.TelegraphDuration, y, rect.width * (1-behaviour.TelegraphDuration), height);
            EditorGUI.DrawRect(activation, Color.brown);
        }
    }
    
    [CustomEditor(typeof(MechanicControlClip))]
    public class MechanicControlClipPropertiesEditor : UnityEditor.Editor
    {
        private SerializedProperty template;

        private SerializedProperty mechanicToPlay;
        private SerializedProperty spawnBehaviour;
        private SerializedProperty telegraphDuration;
        private SerializedProperty followDuration;

        void OnEnable()
        {
            template = serializedObject.FindProperty("_template");

            mechanicToPlay = template.FindPropertyRelative("_mechanicToPlay");
            spawnBehaviour = template.FindPropertyRelative("_spawnBehaviour");

            telegraphDuration = template.FindPropertyRelative("telegraphDuration");
            followDuration = template.FindPropertyRelative("followDuration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawMechanicSection();
            EditorGUILayout.Space(10);
            DrawDurationSection();
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Add loadbar"))
            {
                AddLoadbar();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void DrawMechanicSection()
        {
            EditorGUILayout.LabelField("Mechanic Parameters", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(mechanicToPlay);
            EditorGUILayout.PropertyField(spawnBehaviour);
        }

        void DrawDurationSection()
        {
            EditorGUILayout.LabelField("Durations", EditorStyles.boldLabel);

            EditorGUILayout.Slider(telegraphDuration, 0f, 1f);
            EditorGUILayout.Slider(followDuration, 0f, 1f);
        }
        
        private void AddLoadbar()
        {
            PlayableDirector director = TimelineEditor.inspectedDirector;
            if (director == null)
                return;
            
            TimelineAsset timeline = director.playableAsset as TimelineAsset;
            if (timeline == null)
                return;

            BossControlTrack track = GetOrCreateTrack<BossControlTrack>(timeline, "BossControlTrack");
            
            Entity.Boss.Boss boss = FindObjectOfType<Entity.Boss.Boss>();
            if (boss == null)
            {
                Debug.LogWarning("No Boss found in the scene.");
            }
            director.SetGenericBinding(track, boss);
            
            var clip = track.CreateClip<BossControlClip>();
            
            clip.start = TimelineEditor.selectedClip.start;
            clip.duration = TimelineEditor.selectedClip.duration * telegraphDuration.floatValue;
            
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        private static T GetOrCreateTrack<T>(TimelineAsset timeline, string trackName = null) where T : TrackAsset, new()
        {
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is T typedTrack)
                {
                    return typedTrack;
                }
            }
            
            Undo.RegisterCompleteObjectUndo(timeline, "Create Timeline Track");
            
            T newTrack = timeline.CreateTrack<T>(null, trackName ?? typeof(T).Name);
            
            EditorUtility.SetDirty(timeline);
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            
            return newTrack;
        }
    }
}