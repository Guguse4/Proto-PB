using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossMechanicTool.Timeline;
using BossMechanicTool.VFX;
using UnityEngine;

namespace BossMechanicTool
{
    public struct MechanicSpawnInformation
    {
        public Vector3 position;
        public Vector3 direction;
        public GameObject objectToFollow;
    }
    
    [RequireComponent(typeof(VFXPlayer))]
    public class MechanicPlayer: MonoBehaviour
    {
        private struct MechanicSaveInformation
        {
            public Mechanic mechanic;
            public List<MechanicSpawnInformation> spawnInformations;
        }
        
        // Tool to draw telegraph
        private VFXPlayer _vfxPlayer;
        // Save already computed mechanics
        private Dictionary<string, MechanicSaveInformation> _idToMechanic = new Dictionary<string, MechanicSaveInformation>();
        // List of all players
        List<Player> _players = new List<Player>();
        
        private void Start()
        {
            _vfxPlayer = GetComponent<VFXPlayer>();
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
        }

        #region Telegraph
        
        /*
         * Function called to draw mechanic telegraph phase
         * id: unique mechanic id for save
         * mechanic: mechanic data to use
         * data: origin position, direction and target
         */
        public void ShowMechanicTelegraph(string id, Mechanic mechanic, SpawnBehaviour spawnBehaviour)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            #endif
            
            MechanicSaveInformation mechanicSaveInformation = new MechanicSaveInformation();
            mechanicSaveInformation.mechanic = mechanic;
            mechanicSaveInformation.spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);

            // Register mechanic in dictionary to use it latter
            RegisterMechanic(id, mechanicSaveInformation);
            
            // Switch activation delay, start coroutine or execute immediately
            if (mechanic.activationDelay > 0f)
            {
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic, mechanicSpawnInformation));
                }
            }
            else
            {
                // Show telegraph for each pattern in mechanic
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    foreach (var pattern in mechanic.patterns)
                    {
                        ShowPatternTelegraph(id, pattern, mechanicSpawnInformation);
                    }
                }
            }
        }

        /*
         * Delayed version of the function above
         */
        private IEnumerator ShowMechanicTelegraphDelayed(string id, Mechanic mechanic, MechanicSpawnInformation spawnInformation)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                ShowPatternTelegraph(id, pattern, spawnInformation);
                yield return wait;
            }
        }

        private void ShowPatternTelegraph(string id, Pattern pattern, MechanicSpawnInformation spawnInformation)
        {
            Vector3 origin = pattern.SourceRelativePosition;
            if (spawnInformation.objectToFollow != null)
            {
                origin += spawnInformation.objectToFollow.transform.position;
            }
            else
            {
                origin += spawnInformation.position;
            }
                    
            Vector3 direction = spawnInformation.direction + pattern.SourceRelativeDirection;
            
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                
            _vfxPlayer.ShowVfx(
                id, 
                pattern.TelegraphPrefab, 
                origin, 
                rotation, 
                pattern.GetActionSize(), 
                spawnInformation.objectToFollow != null ? spawnInformation.objectToFollow.transform : null
                );
        }
        #endregion

        #region Activation
        public void ActivateMechanic(string id, Mechanic mechanic, SpawnBehaviour spawnBehaviour)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            #endif

            MechanicSaveInformation mechanicSaveInformation = new MechanicSaveInformation();
            if (_idToMechanic.ContainsKey(id) == false)
            {
                mechanicSaveInformation.mechanic = mechanic;
                mechanicSaveInformation.spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);
            }
            else
            {
                mechanicSaveInformation = _idToMechanic[id];
            }

            // Register mechanic in dictionary to use it latter
            RegisterMechanic(id, mechanicSaveInformation);
                
            if (mechanic.activationDelay > 0f)
            {
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    StartCoroutine(ActivateMechanicDelayed(id, mechanic, mechanicSpawnInformation));
                }
            }
            else
            {
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    foreach (var pattern in mechanic.patterns)
                    {
                        if (pattern != null && pattern.Action != null)
                        {
                            ActivatePattern(id, pattern, mechanicSpawnInformation);
                        }
                    }
                }
            }
        }

        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic, MechanicSpawnInformation mechanicSpawnInformation)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            foreach (var pattern in mechanic.patterns)
            {
                if (pattern != null && pattern.Action != null)
                {
                    ActivatePattern(id, pattern, mechanicSpawnInformation);
                    // wait delay
                    yield return wait;
                }
            }
        }

        private void ActivatePattern(string id, Pattern pattern, MechanicSpawnInformation mechanicSpawnInformation)
        {
            Vector3 origin = pattern.SourceRelativePosition;
            if (mechanicSpawnInformation.objectToFollow != null)
            {
                origin += mechanicSpawnInformation.objectToFollow.transform.position;
            }
            else
            {
                origin += mechanicSpawnInformation.position;
            }
            
            Vector3 direction = mechanicSpawnInformation.direction + pattern.SourceRelativeDirection;
            
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            
            direction.y = 0f;
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                        
            // activate action
            pattern.Action.ActivateAction(origin, direction);
            // play vfx
            _vfxPlayer.ShowVfx(id, pattern.ActivationVFX, origin, rotation, pattern.GetActionSize(), null);
        }
        #endregion
        
        #region Hide
        public void HideMechanic(string id)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            #endif
            
            if (_idToMechanic.ContainsKey(id))
            {
                Mechanic mechanic = _idToMechanic[id].mechanic;

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

        private void RegisterMechanic(string id, MechanicSaveInformation mechanicInfos)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                _idToMechanic.Add(id, mechanicInfos);
            }
        }
        
        #region Compute pattern position and movement
        private List<MechanicSpawnInformation> ComputeMechanicSpawnInformation(SpawnBehaviour spawnBehaviour)
        {
            List<MechanicSpawnInformation> behaviours = new List<MechanicSpawnInformation>();
            switch (spawnBehaviour.movementBehaviour)
            {
                case MovementBehaviour.Static:
                    List<Vector3> origins = ComputeSpawnPosition(spawnBehaviour);
                    foreach (var origin in origins)
                    {
                        MechanicSpawnInformation data = new MechanicSpawnInformation();
                        data.position = origin;
                        data.objectToFollow = null;
                        data.direction = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
                
                case MovementBehaviour.FollowSpawnPositionObject:
                    List<GameObject> targets = ComputeSpawnTarget(spawnBehaviour);
                    foreach (var target in targets)
                    {
                        MechanicSpawnInformation data = new MechanicSpawnInformation();
                        data.position = Vector3.zero;
                        data.objectToFollow = target;
                        data.direction = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
            }

            return null;
        }

        private List<Vector3> ComputeSpawnPosition(SpawnBehaviour spawnBehaviour)
        {
            List<Vector3> positions = new List<Vector3>();
            
            switch (spawnBehaviour.spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenPosition:
                    positions.Add(spawnBehaviour.spawnPosition);
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

        private List<GameObject> ComputeSpawnTarget(SpawnBehaviour spawnBehaviour)
        {
            List<GameObject> targets = new List<GameObject>();
            
            switch (spawnBehaviour.spawnPositionBehaviour)
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