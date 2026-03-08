using System;
using UnityEngine;
using UnityEngine.Playables;

namespace BossMechanicTool.Timeline.Mechanic
{
    /*
     * Define the attached source behaviours
     */
    public enum SpawnPositionBehaviour
    {
        OnGivenPosition,        // The mechanic will be centered on the given position
        OnNearestPlayerFrom,    // The mechanic will be centered on the nearest player
        OnRandomPlayer,         // The mechanic will be centered on a random player
        OnAllPlayers,           // The mechanic will be spawned on all players
        // OnAllPlayersAlive,   // The mechanic will be spawned on all alive players
        // OnAllPlayersDead,    // The mechanic will be spawned on all dead players
    }

    /*
     * Define if the mechanic should stay static or follow the spawn position object
     */
    public enum MovementBehaviour
    {
        Static,
        FollowSpawnPositionObject
    }

    /*
     * Define the global spawn behaviour information
     */
    [Serializable]
    public struct SpawnBehaviour
    {
        public SpawnPositionBehaviour spawnPositionBehaviour;
        public Vector3 spawnPosition;   // if spawnPositionBehaviour is set OnGivenPosition
        public ExposedReference<GameObject> attachedObject;
        public MovementBehaviour movementBehaviour; 
    }
    
    /*
     * This class is used in timeline to define mechanic durations and spawn behaviour
     * It only calls mechanic player to show/hide/activate the given mechanic with the given spawn behaviour
     */
    [Serializable]
    public class MechanicControlBehaviour: PlayableBehaviour
    {
        [Header("Mechanic parameters")]
        // The mechanic data reference to play
        [SerializeField] private BossMechanicTool.Mechanic _mechanicToPlay;
        
        // Define the global spawn behaviour of the mechanic  
        [SerializeField] private SpawnBehaviour _spawnBehaviour;
        public SpawnBehaviour SpawnBehaviour { get { return _spawnBehaviour; } }
        
        [Header("Durations parameters")]
        // Percent duration of telegraph phase in total clip duration
        [SerializeField][Range(0,1)] private float telegraphDuration = 0.5f;
        public float TelegraphDuration{get{return telegraphDuration;}}

        [SerializeField] [Range(0, 1)] private float followDuration = 1.0f;
        public float FollowDuration{get{return followDuration;}}

        // use to do stuff once at the first frame
        private bool _firstFrameHappened;
        // use to save if the mechanic player has already displayed the telegraph of the mechanic
        private bool _telegraphDisplayed;
        // 
        private bool _isFollowing;
        // use to save if the mechanic player has already activated the mechanic
        private bool _activationDone;
        
        // reference to mechanic player that can show/hide/activate the mechanic
        private MechanicPlayer _mechanicPlayer;
        
        // a unique id generated at the first frame to manager multiple dynamic in the same time
        private string uniqueMechanicId;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Save mechanic player
            _mechanicPlayer = playerData as MechanicPlayer;
            if (_mechanicPlayer == null)
            {
                Debug.LogWarning("MechanicControlBehaviour: Mechanic player is null.");
                return;
            }

            // Do once at the first frame
            if (_firstFrameHappened == false)
            {
                _firstFrameHappened = true;

                // Generate random id
                uniqueMechanicId = Guid.NewGuid().ToString();
            }

            // If the current frame time is in the range of the telegraph phase
            if (playable.GetTime() < telegraphDuration * playable.GetDuration())
            {
                // Display telegraph if not already displayed
                if (_telegraphDisplayed == false)
                {
                    _telegraphDisplayed = true;
                    _mechanicPlayer.ShowMechanicTelegraph(uniqueMechanicId, _mechanicToPlay, _spawnBehaviour);
                }
                // Follow object if necessary
                if (playable.GetTime() < followDuration * telegraphDuration * playable.GetDuration())
                {
                    _isFollowing = true;
                    _mechanicPlayer.FollowMechanic(uniqueMechanicId);
                }
                else
                {
                    _isFollowing = false;
                    _mechanicPlayer.StopFollowingMechanic(uniqueMechanicId);
                }
            }
            // If the current frame time is in the range of the activation phase
            else
            {
                // Hide telegraph if necessary
                if (_telegraphDisplayed)
                {
                    _telegraphDisplayed = false;
                    _mechanicPlayer.HideMechanicTelegraph(uniqueMechanicId);
                    _isFollowing = false;
                    _mechanicPlayer.StopFollowingMechanic(uniqueMechanicId);
                }
                // Activate mechanic if not already done
                if (_activationDone == false)
                {
                    _activationDone = true;
                    _mechanicPlayer.ActivateMechanic(uniqueMechanicId, _mechanicToPlay,  _spawnBehaviour);
                }
            }
        }
        
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // Reset do once booleans
            _firstFrameHappened = false;
            _telegraphDisplayed = false;
            _activationDone = false;

            if (_mechanicPlayer == null)
            {
                return;
            }

            // Hide all instantiated mechanic information
            _mechanicPlayer.HideMechanicTelegraph(uniqueMechanicId);

            _mechanicPlayer.DestroyMechanicObject(uniqueMechanicId);
            
            // reset mechanic id
            uniqueMechanicId = null;
            
            base.OnBehaviourPause(playable, info);
        }
    }
}