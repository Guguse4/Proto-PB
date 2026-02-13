using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BossMechanicTool.Timeline
{
    public class MechanicControlClip: PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private MechanicControlBehaviour _template =  new MechanicControlBehaviour();

        public MechanicControlBehaviour GetTemplate()
        {
            return _template;
        }
        
        public ClipCaps clipCaps { get {return ClipCaps.None; } }
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<MechanicControlBehaviour>.Create(graph, _template);
        }
    }
}