using BossMechanicTool.Timeline;
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
            Rect left = new Rect(rect.x, rect.y + 3*rect.height/4, rect.width * behaviour.TelegraphDuration, rect.height/4);
            Rect right = new Rect(rect.x + rect.width * behaviour.TelegraphDuration, rect.y + 3*rect.height/4, rect.width * (1-behaviour.TelegraphDuration), rect.height/4);
            
            EditorGUI.DrawRect(left, Color.aquamarine);
            EditorGUI.DrawRect(right, Color.brown);
        }
    }
}