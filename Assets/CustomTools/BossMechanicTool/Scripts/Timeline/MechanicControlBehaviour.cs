using System;
using UnityEngine;
using UnityEngine.Playables;

namespace BossMechanicTool.Timeline
{
    public enum MechanicControlAction
    {
        ShowMechanicTelegraph,
        ActivateMechanic
    }
    
    [System.Serializable]
    public class MechanicControlBehaviour: PlayableBehaviour
    {
        [SerializeField]
        private Mechanic _mechanicToPlay;
        private string uniqueMechanicId;
        
        [SerializeField] 
        private MechanicControlAction _action;
        public MechanicControlAction Action{get{return _action;}}

        private bool _firstFrameHeppened;
        private MechanicPlayer _mechanicPlayer;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _mechanicPlayer = playerData as MechanicPlayer;
            if(_mechanicPlayer == null) 
                return;

            if (_firstFrameHeppened == false)
            {
                _firstFrameHeppened = true;
                // Save area
                uniqueMechanicId = Guid.NewGuid().ToString();
                // Do once
                switch (_action)
                {
                    case MechanicControlAction.ShowMechanicTelegraph:
                        _mechanicPlayer.ShowMechanicTelegraph(_mechanicToPlay, uniqueMechanicId);
                        break;
                    case  MechanicControlAction.ActivateMechanic:
                        _mechanicPlayer.ActivateMechanic(_mechanicToPlay, uniqueMechanicId);
                        break;
                }

                return;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            _firstFrameHeppened = false;
            if (_mechanicPlayer == null)
                return;

            // Reset area
            _mechanicPlayer.HideMechanicTelegraph(uniqueMechanicId);
            uniqueMechanicId = null;
            
            base.OnBehaviourPause(playable, info);
        }
    }
}