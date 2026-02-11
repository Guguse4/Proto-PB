using System;
using BulletHell.Emitter;
using UnityEngine;
using UnityEngine.Playables;

namespace BulletHellTool.Timeline
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PatternSignalReceiver: MonoBehaviour, INotificationReceiver
    {
        private PlayableDirector playableDirector;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is PatternSignal patternSignal)
            {
                HandleEmitterActivation(patternSignal);
            }
        }

        private void HandleEmitterActivation(PatternSignal signal)
        {
            Emitter e = signal.emitter.Resolve(playableDirector);
            e.gameObject.SetActive(true);
        }
    }
}