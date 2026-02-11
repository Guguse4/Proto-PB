using BulletHell.Emitter;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BulletHellTool.Timeline
{
    [System.Serializable]
    public class PatternSignal: Marker, INotification
    {
        public ExposedReference<Emitter> emitter;

        public PropertyName id => new PropertyName();
    }
}