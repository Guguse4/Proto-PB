using BossMechanicTool.Timeline;
using BossMechanicTool.Timeline.Mechanic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
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
}