using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BossMechanicTool.Scripts.Timeline.Boss
{
    public class BossControlClip: PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private BossControlBehaviour _template = new BossControlBehaviour();

        public BossControlBehaviour GetTemplate()
        {
            return _template;
        }
        
        public ClipCaps clipCaps {get{return ClipCaps.None;}}

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<BossControlBehaviour>.Create(graph, _template);
        }
    }
}