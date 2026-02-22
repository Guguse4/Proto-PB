using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossMechanicTool.Timeline;
using BossMechanicTool.VFX;
using UnityEngine;

namespace BossMechanicTool
{
    /*
     * Minimum spawn information necessary to play a mechanic
     */
    public struct MechanicSpawnInformation
    {
        // The center position of the mechanic
        public Vector3 position;
        // The base direction of the mechanic
        public Vector3 direction;
        // If necessary, a moving object to attach the mechanic (player, boss,...)
        public GameObject objectToFollow;
    }
    
    /*
     * Main mechanic class used to play all mechanics in the game
     */
    [RequireComponent(typeof(VFXPlayer))]
    public class MechanicPlayer: MonoBehaviour
    {
        /*
         * Mechanic information to save from start of telegraph phase to the activation end 
         */
        private struct MechanicSaveInformation
        {
            public Mechanic mechanic;
            
            // Multiple spawn information here if we need to spawn the mechanic on multiple position
            // i.e: on all player
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
            _idToMechanic = new Dictionary<string, MechanicSaveInformation>();
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
            
            // Init a new mechanic save information
            MechanicSaveInformation mechanicSaveInformation = new MechanicSaveInformation();
            mechanicSaveInformation.mechanic = mechanic;
            mechanicSaveInformation.spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);

            // Register mechanic information in dictionary to use it latter
            RegisterMechanic(id, mechanicSaveInformation);
            
            // Switch activation delay, start coroutine or execute immediately
            if (mechanic.activationDelay > 0f)
            {
                // Spawn mechanic for on each needed position
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    StartCoroutine(ShowMechanicTelegraphDelayed(id, mechanic, mechanicSpawnInformation));
                }
            }
            else
            {
                // Spawn mechanic for on each needed position
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    // Show telegraph for each pattern in mechanic
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
            // Show telegraph for each pattern in mechanic
            foreach (var pattern in mechanic.patterns)
            {
                ShowPatternTelegraph(id, pattern, spawnInformation);
                yield return wait;
            }
        }

        /*
         * Function used to spawn telegraph for the given pattern with the given spawn information
         */
        private void ShowPatternTelegraph(string id, Pattern pattern, MechanicSpawnInformation spawnInformation)
        {
            // Init the origin of the pattern
            Vector3 origin = spawnInformation.position + pattern.SourceRelativePosition;
            // Init the direction of the pattern        
            Vector3 direction = spawnInformation.direction + pattern.SourceRelativeDirection;
            
            // Set default forward if invalid direction
            if (direction == Vector3.zero)
                direction = Vector3.forward;
            direction.y = 0f;
            direction.Normalize();
            
            // Compute rotation according to the computed direction
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            // Show VFX with the computed information
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
        /*
         * Function called to activate mechanic
         * id: unique mechanic id for save
         * mechanic: mechanic data to use (necessary if activation without telegraph)
         * data: origin position, direction and target (necessary if activation without telegraph)
         */
        public void ActivateMechanic(string id, Mechanic mechanic, SpawnBehaviour spawnBehaviour)
        {
            #if UNITY_EDITOR
            _vfxPlayer = GetComponent<VFXPlayer>();
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            #endif

            // Try to find mechanic information if available, or compute it if necessary
            MechanicSaveInformation mechanicSaveInformation = new MechanicSaveInformation();
            if (_idToMechanic.ContainsKey(id) == false)
            {
                mechanicSaveInformation.mechanic = mechanic;
                mechanicSaveInformation.spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);
                // Register mechanic in dictionary to use it latter
                RegisterMechanic(id, mechanicSaveInformation);
            }
            else
            {
                mechanicSaveInformation = _idToMechanic[id];
            }
            
            // Switch activation delay, start coroutine or execute immediately
            if (mechanic.activationDelay > 0f)
            {
                // Spawn mechanic for on each needed position
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    StartCoroutine(ActivateMechanicDelayed(id, mechanic, mechanicSpawnInformation));
                }
            }
            else
            {
                // Spawn mechanic for on each needed position
                foreach (var mechanicSpawnInformation in mechanicSaveInformation.spawnInformations)
                {
                    // Activate all pattern in the mechanic
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

        /*
         * Delayed version of the function above
         */
        private IEnumerator ActivateMechanicDelayed(string id, Mechanic mechanic, MechanicSpawnInformation mechanicSpawnInformation)
        {
            WaitForSeconds wait = new WaitForSeconds(mechanic.activationDelay);
            // Activate all pattern in the mechanic
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

        /*
         * Function called to activate a given pattern with the given spawn information
         */
        private void ActivatePattern(string id, Pattern pattern, MechanicSpawnInformation mechanicSpawnInformation)
        {
            // Compute the origin if the pattern is attached to a moving object
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

        /*
         * Function called to save given mechanic
         */
        private void RegisterMechanic(string id, MechanicSaveInformation mechanicInfos)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                _idToMechanic.Add(id, mechanicInfos);
            }
        }
        
        #region Compute pattern position and movement
        /*
         * Function called to computed the mechanic spawn information according to the given spawn behaviour
         */
        private List<MechanicSpawnInformation> ComputeMechanicSpawnInformation(SpawnBehaviour spawnBehaviour)
        {
            List<MechanicSpawnInformation> behaviours = new List<MechanicSpawnInformation>();
            switch (spawnBehaviour.movementBehaviour)
            {
                // If the mechanic don't move
                case MovementBehaviour.Static:
                    // Compute the start positions
                    List<Vector3> origins = ComputeSpawnPosition(spawnBehaviour);
                    // For each, init a mechanic spawn information
                    foreach (var origin in origins)
                    {
                        MechanicSpawnInformation data = new MechanicSpawnInformation();
                        data.position = origin;
                        data.objectToFollow = null;
                        data.direction = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
                // If the mechanic move
                case MovementBehaviour.FollowSpawnPositionObject:
                    // Compute the target objects
                    List<GameObject> targets = ComputeSpawnTarget(spawnBehaviour);
                    // For each, init a mechanic spawn information
                    foreach (var target in targets)
                    {
                        MechanicSpawnInformation data = new MechanicSpawnInformation();
                        data.position = target.transform.position;
                        data.objectToFollow = target;
                        data.direction = Vector3.zero;
                        behaviours.Add(data);
                    }
                    return behaviours;
            }

            return null;
        }

        /*
         * Function called to compute the start positions of a mechanic according to the spawn behaviour (Static)
         */
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

        /*
         * Function called to compute the targets of a mechanic according to the spawn behaviour (FollowSpawnPositionObject) 
         */
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