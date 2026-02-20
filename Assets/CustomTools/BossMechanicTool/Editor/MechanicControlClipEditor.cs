using BossMechanicTool.Timeline;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace BossMechanicTool.Editor
{
    [CustomTimelineEditor(typeof(MechanicControlClip))]
    public class MechanicControlClipEditor: ClipEditor
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var clipOptions = base.GetClipOptions(clip);
            clipOptions.highlightColor = Color.red;
            
            MechanicControlClip mechanicControlClip = clip.asset as MechanicControlClip;
            if (mechanicControlClip == null)
            {
                return clipOptions;
            }
            
            
            MechanicControlBehaviour behaviour = mechanicControlClip.GetTemplate();
            if (behaviour == null)
            {
                return clipOptions;
            }

            switch (behaviour.Action)
            {
                case MechanicControlAction.ShowMechanicTelegraph:
                    clipOptions.highlightColor = Color.yellow;
                    break;
                case MechanicControlAction.ActivateMechanic:
                    clipOptions.highlightColor = Color.green;
                    break;
            }
            
            return clipOptions;
        }
    }
}