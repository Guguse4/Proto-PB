using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossMechanicTool.VFX;
using NUnit.Framework;
using UnityEngine;

namespace BossMechanicTool
{
    [RequireComponent(typeof(VFXPlayer))]
    public class MechanicPlayer: MonoBehaviour
    {
        private struct BehavioursData
        {
            public Vector3 position;
            public Vector3 direction;
            public GameObject target;
        }
        
        // Tool to draw telegraph
        private VFXPlayer _vfxPlayer;
        private List<Player> _players;
        
        private Dictionary<string, Mechanic> _idToMechanic = new Dictionary<string, Mechanic>();
        
        private void Start()
        {
            _vfxPlayer = GetComponent<VFXPlayer>();
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
        }

        #region Telegraph
        public void ShowMechanicTelegraph(string id, Mechanic mechanic)
        {
            #if UNITY_EDITOR
            
            Start();
            
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic));
            }
            else
            {
                List<BehavioursData> behaviours = ComputeBehaviourData(mechanic);

                foreach (BehavioursData behaviourData in behaviours)
                {
                    foreach (var pattern in mechanic.patterns)
                    {
                        _vfxPlayer.ShowTelegraphPattern(id, pattern, behaviourData.position, behaviourData.target);
                    }
                }
            }
        }

        private IEnumerator ShowMechanicTelegraphDelayed(string id, Mechanic mechanic)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            List<BehavioursData> behaviours = ComputeBehaviourData(mechanic);

            foreach (BehavioursData behaviourData in behaviours)
            {
                foreach (var pattern in mechanic.patterns)
                {
                    _vfxPlayer.ShowTelegraphPattern(id, pattern, behaviourData.position, behaviourData.target);
                    yield return wait;
                }
            }
        }
        #endregion

        #region Activation
        public void ActivateMechanic(string id, Mechanic mechanic)
        {
            #if UNITY_EDITOR
            
            Start();
            
            #endif

            RegisterMechanic(id, mechanic);
                
            if (mechanic.activationDelay > 0f)
            {
                StartCoroutine(ActivateMechanicDelayed(id, mechanic));
            }
            else
            {
                foreach (var pattern in mechanic.patterns)
                {
                    if (pattern != null && pattern.Action != null)
                    {
                        // activate action
                        pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition,
                            pattern.SourceRelativeDirection);
                        // play vfx
                        _vfxPlayer.ShowActivationPattern(id, pattern, mechanic.spawnPosition);
                    }
                }
            }
        }

        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    // activate action
                    pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition,
                        pattern.SourceRelativeDirection);
                    // play vfx
                    _vfxPlayer.ShowActivationPattern(id, pattern, mechanic.spawnPosition);
                    // wait delay
                    yield return wait;
                }
            }
        }
        #endregion
        
        #region Hide
        public void HideMechanic(string id)
        {
            #if UNITY_EDITOR
            
            Start();
            
            #endif
            
            if (_idToMechanic.ContainsKey(id))
            {
                Mechanic mechanic = _idToMechanic[id];

                if (mechanic.activationDelay > 0f)
                {
                    StartCoroutine(HideMechanicDelayed(id, mechanic));
                }
                else
                {
                    int count = _vfxPlayer.GetVFXNumber(id);
                    for(int i = 0; i < count; i++)
                    {
                        _vfxPlayer.HidePattern(id);
                    }
                    _idToMechanic.Remove(id);
                }
            }
        }

        private IEnumerator HideMechanicDelayed(string id, Mechanic mechanic)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            int count = _vfxPlayer.GetVFXNumber(id);
            for(int i = 0; i < count; i++)
            {
                _vfxPlayer.HidePattern(id);
                yield return wait;
            }
            _idToMechanic.Remove(id);
        }
        #endregion

        private void RegisterMechanic(string id, Mechanic mechanic)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                _idToMechanic.Add(id, mechanic);
            }
        }
        
        #region Compute pattern position and movement
        private List<BehavioursData> ComputeBehaviourData(Mechanic mechanic)
        {
            List<BehavioursData> behaviours = new List<BehavioursData>();
            switch (mechanic.movementBehaviour)
            {
                case MovementBehaviour.Static:
                    List<Vector3> origins = ComputeSpawnPosition(mechanic);
                    foreach (var origin in origins)
                    {
                        BehavioursData data = new BehavioursData();
                        data.position = origin;
                        data.direction = Vector3.zero;
                        data.target = null;
                        behaviours.Add(data);
                    }
                    return behaviours;
                
                case MovementBehaviour.FollowSpawnPositionObject:
                    List<GameObject> targets = ComputeSpawnTarget(mechanic);
                    foreach (var target in targets)
                    {
                        BehavioursData data = new BehavioursData();
                        data.position = Vector3.zero;
                        data.direction = Vector3.zero;
                        data.target = target;
                        behaviours.Add(data);
                    }
                    return behaviours;
            }

            return null;
        }

        private List<Vector3> ComputeSpawnPosition(Mechanic mechanic)
        {
            List<Vector3> positions = new List<Vector3>();
            
            switch (mechanic.spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenPosition:
                    positions.Add(mechanic.spawnPosition);
                    break;
                case SpawnPositionBehaviour.OnAllPlayers:
                    foreach (Player player in _players)
                    {
                        positions.Add(player.transform.position);
                    }
                    break;
                case SpawnPositionBehaviour.OnRandomPlayer:
                    positions.Add(_players[Random.Range(0, _players.Count)].transform.position);
                    break;
            }

            return positions;
        }

        private List<GameObject> ComputeSpawnTarget(Mechanic mechanic)
        {
            List<GameObject> targets = new List<GameObject>();
            
            switch (mechanic.spawnPositionBehaviour)
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
                    targets.Add(_players[Random.Range(0, _players.Count)].gameObject);
                    break;
            }
            
            return targets;
        }
        #endregion
    }
}