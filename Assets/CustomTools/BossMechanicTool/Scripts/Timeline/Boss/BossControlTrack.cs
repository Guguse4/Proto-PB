using UnityEngine.Timeline;
    
namespace BossMechanicTool.Scripts.Timeline.Boss
{
    [TrackColor(125f/255f, 68f/255f, 0f/255f)]
    [TrackBindingType(typeof(Entity.Boss.Boss))]
    [TrackClipType(typeof(BossControlClip))]
    public class BossControlTrack: TrackAsset
    {
        
    }
}