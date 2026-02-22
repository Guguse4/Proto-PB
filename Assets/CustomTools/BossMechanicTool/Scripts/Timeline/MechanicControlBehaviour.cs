using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

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
    
    public struct BehavioursData
    {
        public Vector3 position;
        public Vector3 rotation;
        public GameObject target;
    }
    
    [Serializable]
    public class MechanicControlBehaviour: PlayableBehaviour
    {
        [Header("Mechanic parameters")]
        [SerializeField] private Mechanic _mechanicToPlay;
        
        // Define the attached source of the mechanic  
        [SerializeField] private SpawnPositionBehaviour spawnPositionBehaviour;
        [SerializeField] private Vector3 spawnPosition;
        
        [SerializeField] private MovementBehaviour movementBehaviour;

        [SerializeField][Range(0,1)] private float telegraphDuration = 0.5f;
        public float TelegraphDuration{get{return telegraphDuration;}}

        private bool _firstFrameHeppened;
        private bool _telegraphDisplayed;
        private bool _activationDone;
        
        private MechanicPlayer _mechanicPlayer;
        private string uniqueMechanicId;
        private List<Player> _players;
        private List<BehavioursData> _behaviours;

        public override void PrepareData(Playable playable, FrameData info)
        {
            base.PrepareData(playable, info);
            _players = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
        }

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
#if UNITY_EDITOR
                _players = GameObject.FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
#endif
            }

            if (playable.GetTime() < telegraphDuration * playable.GetDuration())
            {
                if (_telegraphDisplayed == false)
                {
                    _telegraphDisplayed = true;
                    _behaviours = ComputeBehaviourData(_mechanicToPlay);
                    foreach (var behaviour in _behaviours)
                    {
                        _mechanicPlayer.ShowMechanicTelegraph(uniqueMechanicId, _mechanicToPlay, behaviour);
                    }
                }
            }
            else
            {
                if (_telegraphDisplayed)
                {
                    _telegraphDisplayed = false;
                    _mechanicPlayer.HideMechanic(uniqueMechanicId);
                }
                else
                {
                    _behaviours = ComputeBehaviourData(_mechanicToPlay);
                }
                
                if (_activationDone == false)
                {
                    _activationDone = true;
                    foreach (var behaviour in _behaviours)
                    {
                        _mechanicPlayer.ActivateMechanic(uniqueMechanicId, _mechanicToPlay, behaviour);
                    }
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
        
        #region Compute pattern position and movement
        private List<BehavioursData> ComputeBehaviourData(Mechanic mechanic)
        {
            List<BehavioursData> behaviours = new List<BehavioursData>();
            switch (movementBehaviour)
            {
                case MovementBehaviour.Static:
                    List<Vector3> origins = ComputeSpawnPosition(mechanic);
                    foreach (var origin in origins)
                    {
                        BehavioursData data = new BehavioursData();
                        data.position = origin;
                        data.target = null;
                        data.rotation = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
                
                case MovementBehaviour.FollowSpawnPositionObject:
                    List<GameObject> targets = ComputeSpawnTarget(mechanic);
                    foreach (var target in targets)
                    {
                        BehavioursData data = new BehavioursData();
                        data.position = Vector3.zero;
                        data.target = target;
                        data.rotation = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
            }

            return null;
        }

        private List<Vector3> ComputeSpawnPosition(Mechanic mechanic)
        {
            List<Vector3> positions = new List<Vector3>();
            
            switch (spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenPosition:
                    positions.Add(spawnPosition);
                    break;
                case SpawnPositionBehaviour.OnAllPlayers:
                    foreach (Player player in _players)
                    {
                        positions.Add(player.transform.position);
                    }
                    break;
                case SpawnPositionBehaviour.OnRandomPlayer:
                    if(_players.Count > 0)
                        positions.Add(_players[Random.Range(0, _players.Count)].transform.position);
                    break;
            }

            return positions;
        }

        private List<GameObject> ComputeSpawnTarget(Mechanic mechanic)
        {
            List<GameObject> targets = new List<GameObject>();
            
            switch (spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenObject:
                    targets.Add(null);
                    break;
                case SpawnPositionBehaviour.OnAllPlayers:
                    foreach (Player player in _players)
                    {
                        targets.Add(player.gameObject);
                    }
                    break;
                case SpawnPositionBehaviour.OnRandomPlayer:
                    if(_players.Count > 0)
                        targets.Add(_players[Random.Range(0, _players.Count)].gameObject);
                    break;
            }
            
            return targets;
        }
        #endregion
    }
}