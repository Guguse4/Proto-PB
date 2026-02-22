using System;
using UnityEngine;
using UnityEngine.Playables;

namespace BossMechanicTool.Timeline
{
    /*
     * Define the attached source behaviours
     */
    public enum SpawnPositionBehaviour
    {
        OnGivenPosition,
        OnNearestPlayer,    // The mechanic will be centered on the nearest player
        OnRandomPlayer,     // The mechanic will be centered on a random player
        OnAllPlayers,       // The mechanic will be played on all players
        OnAllPlayersAlive,  // The mechanic will be played on all alive players
        OnAllPlayersDead,   // The mechanic will be played on all dead players
        OnGivenObject,      // The mechanic will be centered on a given object/position
    }

    /*
     * Define if the pattern should stay static or follow the spawn position
     */
    public enum MovementBehaviour
    {
        Static,
        FollowSpawnPositionObject
    }

    [Serializable]
    public struct SpawnBehaviour
    {
        public SpawnPositionBehaviour spawnPositionBehaviour;
        public Vector3 spawnPosition;
        public MovementBehaviour movementBehaviour;
    }
    
    /*
     * This class is used in timeline to define mechanic durations and spawn behaviour
     * It only calls mechanic player to show/hide/activate mechanic
     */
    [Serializable]
    public class MechanicControlBehaviour: PlayableBehaviour
    {
        [Header("Mechanic parameters")]
        [SerializeField] private Mechanic _mechanicToPlay;
        
        // Define the attached source of the mechanic  
        [SerializeField] private SpawnBehaviour _spawnBehaviour;

        [SerializeField][Range(0,1)] private float telegraphDuration = 0.5f;
        public float TelegraphDuration{get{return telegraphDuration;}}

        private bool _firstFrameHeppened;
        private bool _telegraphDisplayed;
        private bool _activationDone;
        
        private MechanicPlayer _mechanicPlayer;
        private string uniqueMechanicId;

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
            }

            if (playable.GetTime() < telegraphDuration * playable.GetDuration())
            {
                if (_telegraphDisplayed == false)
                {
                    _telegraphDisplayed = true;
                    _mechanicPlayer.ShowMechanicTelegraph(uniqueMechanicId, _mechanicToPlay, _spawnBehaviour);
                }
            }
            else
            {
                if (_telegraphDisplayed)
                {
                    _telegraphDisplayed = false;
                    _mechanicPlayer.HideMechanic(uniqueMechanicId);
                }
                
                if (_activationDone == false)
                {
                    _activationDone = true;
                    _mechanicPlayer.ActivateMechanic(uniqueMechanicId, _mechanicToPlay,  _spawnBehaviour);
                }
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            _firstFrameHeppened = false;
            _telegraphDisplayed = false;
            _activationDone = false;
            
            if (_mechanicPlayer == null)
                return;

            // Reset area
            _mechanicPlayer.HideMechanic(uniqueMechanicId);
            uniqueMechanicId = null;
            
            base.OnBehaviourPause(playable, info);
        }
    }
}