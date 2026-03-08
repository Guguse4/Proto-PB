using System.Collections.Generic;
using System.Linq;
using BossMechanicTool.Timeline;
using BossMechanicTool.Timeline.Mechanic;
using Entity.Player;
using UnityEngine;
using UnityEngine.Playables;

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
    [RequireComponent(typeof(PlayableDirector))]
    public class MechanicPlayer: MonoBehaviour
    {
        // Save already computed mechanics
        private Dictionary<string, List<MechanicObject>> _idToMechanic = new Dictionary<string, List<MechanicObject>>();
        // List of all players
        List<Player> _players = new List<Player>();
        // Object resolver
        PlayableDirector _playableDirector;
        
        private void Start()
        {
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            _playableDirector = GetComponent<PlayableDirector>();
            _idToMechanic = new Dictionary<string, List<MechanicObject>>();
        }

        private MechanicObject SpawnMechanicObject(string id, Mechanic mechanic, MechanicSpawnInformation mechanicSpawnInformation)
        {
            GameObject mechanicGO = new GameObject(id);
            mechanicGO.transform.position = mechanicSpawnInformation.position;
            mechanicGO.transform.rotation = Quaternion.identity;
            mechanicGO.transform.parent = transform;
            
            MechanicObject mechanicObject = mechanicGO.AddComponent<MechanicObject>();
            mechanicObject.SetMechanic(mechanic);
            mechanicObject.SetTarget(mechanicSpawnInformation.objectToFollow);
            // Register mechanic object in dictionary to use it latter
            RegisterMechanic(id, mechanicObject);
            return mechanicObject;
        }
        
        /*
         * Function called to draw mechanic telegraph phase
         * id: unique mechanic id for save
         * mechanic: mechanic data to use
         * data: origin position, direction and target
         */
        public void ShowMechanicTelegraph(string id, Mechanic mechanic, SpawnBehaviour spawnBehaviour)
        {
            #if UNITY_EDITOR
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            _playableDirector = GetComponent<PlayableDirector>();
            #endif
            
            // Init a new mechanic save information
            List<MechanicSpawnInformation> spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);
            
            // Spawn mechanic for on each needed position
            foreach (var mechanicSpawnInformation in spawnInformations)
            {
                MechanicObject mechanicObject = SpawnMechanicObject(id, mechanic, mechanicSpawnInformation);
                mechanicObject.ShowTelegraph();
            }
        }

        /*
         * Function called to activate mechanic
         * id: unique mechanic id for save
         * mechanic: mechanic data to use (necessary if activation without telegraph)
         * data: origin position, direction and target (necessary if activation without telegraph)
         */
        public void ActivateMechanic(string id, Mechanic mechanic, SpawnBehaviour spawnBehaviour)
        {
            #if UNITY_EDITOR
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            _playableDirector = GetComponent<PlayableDirector>();
            #endif

            // Try to find mechanic information if available, or compute it if necessary
            if (_idToMechanic.ContainsKey(id) == false)
            {
                List<MechanicSpawnInformation> spawnInformations = ComputeMechanicSpawnInformation(spawnBehaviour);
                // Spawn mechanic for on each needed position
                foreach (var mechanicSpawnInformation in spawnInformations)
                {
                    MechanicObject mechanicObject = SpawnMechanicObject(id, mechanic, mechanicSpawnInformation);
                    mechanicObject.ActivateMechanic();
                }
            }
            else
            {
                List<MechanicObject> mechanicObjects = _idToMechanic[id];
                foreach (MechanicObject mechanicObject in mechanicObjects)
                {
                    mechanicObject.ActivateMechanic();
                }
            }
        }
        
        #region Hide
        public void HideMechanicTelegraph(string id)
        {
            if (_idToMechanic.ContainsKey(id))
            {
                List<MechanicObject> mechanics = _idToMechanic[id];
                foreach (var mechanic in mechanics)
                {
                    mechanic.HideVFX();
                }
            }
        }

        public void DestroyMechanicObject(string id)
        {
            if (_idToMechanic.ContainsKey(id))
            {
                List<MechanicObject> mechanics = _idToMechanic[id];
                foreach (var mechanic in mechanics)
                {
                    DestroyImmediate(mechanic.gameObject);
                }
                _idToMechanic.Remove(id);
            }
        }
        #endregion

        /*
         * Function called to save given mechanic
         */
        private void RegisterMechanic(string id, MechanicObject mechanicObject)
        {
            if (_idToMechanic.ContainsKey(id) == false)
            {
                List<MechanicObject> mechanicObjects = new List<MechanicObject>();
                mechanicObjects.Add(mechanicObject);
                _idToMechanic.Add(id, mechanicObjects);
            }
            else
            {
                _idToMechanic[id].Add(mechanicObject);
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
            GameObject attachedObject;
            
            switch (spawnBehaviour.spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenPosition:
                    attachedObject = spawnBehaviour.attachedObject.Resolve(_playableDirector);
                    if (attachedObject != null)
                    {
                        positions.Add(attachedObject.transform.position);
                    }
                    else
                    {
                        positions.Add(spawnBehaviour.spawnPosition);
                    }
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
                case SpawnPositionBehaviour.OnNearestPlayerFrom:
                    attachedObject = spawnBehaviour.attachedObject.Resolve(_playableDirector);
                    if (attachedObject != null)
                    {
                        positions.Add(FindNearestPlayerFrom(attachedObject.transform.position).transform.position);
                    }
                    else
                    {
                        positions.Add(FindNearestPlayerFrom(spawnBehaviour.spawnPosition).transform.position);
                    }

                    break;
            }
            Debug.Log("Pre translate pos 0 : " + positions[0]);
            if (spawnBehaviour.translationOffset != Vector3.zero)
            {
                for(int i = 0; i < positions.Count; i++)
                {
                   positions[i] += spawnBehaviour.translationOffset;
                   Debug.Log("Translate added : " + positions[i]);
                }
            }
            
            return positions;
        }

        /*
         * Function called to compute the targets of a mechanic according to the spawn behaviour (FollowSpawnPositionObject) 
         */
        private List<GameObject> ComputeSpawnTarget(SpawnBehaviour spawnBehaviour)
        {
            List<GameObject> targets = new List<GameObject>();
            GameObject attachedObject;
            
            switch (spawnBehaviour.spawnPositionBehaviour)
            {
                case SpawnPositionBehaviour.OnGivenPosition:
                    attachedObject = spawnBehaviour.attachedObject.Resolve(_playableDirector);
                    if (attachedObject != null)
                    {
                        targets.Add(attachedObject);
                    }
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
                case  SpawnPositionBehaviour.OnNearestPlayerFrom:
                    attachedObject = spawnBehaviour.attachedObject.Resolve(_playableDirector);
                    if (attachedObject != null)
                    {
                        targets.Add(FindNearestPlayerFrom(attachedObject.transform.position).gameObject);
                    }
                    else
                    {
                        targets.Add(FindNearestPlayerFrom(spawnBehaviour.spawnPosition).gameObject);
                    }
                    break;
            }
            
            return targets;
        }

        private Player FindNearestPlayerFrom(Vector3 position)
        {
            if (_players.Count <= 0)
                return null;
            
            Player nearestPlayer = null;
            float nearestDistance =  float.MaxValue;
            foreach (Player player in _players)
            {
                if (Vector3.Distance(player.transform.position, position) < nearestDistance)
                {
                    nearestDistance = Vector3.Distance(player.transform.position, position);
                    nearestPlayer = player;
                }
            }
            
            return nearestPlayer;
        }
        #endregion
    }
}