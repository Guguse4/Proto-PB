using UnityEngine.Timeline;

namespace BossMechanicTool.Timeline
{
    [TrackColor(241f / 255f, 249f / 255f, 99f / 255f)]
    [TrackBindingType(typeof(MechanicPlayer))]
    [TrackClipType(typeof(MechanicControlClip))]
    public class MechanicControlTrack : TrackAsset
    {
        
    }
}