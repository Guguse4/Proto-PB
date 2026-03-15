using System;
using BossMechanicTool.Timeline.Mechanic;
using UnityEngine;
using UnityEngine.Playables;

namespace BossMechanicTool.Scripts.Timeline.Boss
{
    [Serializable]
    public class BossControlBehaviour: PlayableBehaviour
    {
        [SerializeField] private Vector3 _moveTo;
        [SerializeField] private string _mechanicName;

        private bool _firstFrameHappened = false;
        Entity.Boss.Boss _boss;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _boss = playerData as Entity.Boss.Boss;
            if (_boss == null)
                return;
            
            // Do once at the first frame
            if (_firstFrameHappened == false)
            {
                _firstFrameHappened = true;
                _boss.InitMechanicLoader(_mechanicName);
            }

            float value = (float)(playable.GetTime() / playable.GetDuration());
            value = Mathf.RoundToInt(value * 100) * 0.01f;
            _boss.UpdateMechanicLoader(value);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            _firstFrameHappened = false;
            if(_boss != null)
                _boss.HideMechanicLoader();
        }
    }
}