using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool
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
     * Define the rotation behaviour
     */
    public enum SpawnRotationBehaviour
    {
        ToNearestPlayer,    // Rotate pattern to focus nearest player
        ToRandomPlayer,     // Rotate pattern to focus random player
        ToAllPlayers        // Rotate pattern to focus all players
    }

    /*
     * Define if the pattern should stay static or follow the spawn position
     */
    public enum MovementBehaviour
    {
        Static,
        FollowSpawnPositionObject
    }

    
    [CreateAssetMenu(menuName = "BossMechanics/Mechanic")]
    public class Mechanic: ScriptableObject
    {
        // List of patterns executed in the same time by the mechanic
        public List<Pattern> patterns;
        
        // Define the attached source of the mechanic  
        public SpawnPositionBehaviour spawnPositionBehaviour;
        public Vector3 spawnPosition;
        
        // Define the rotation behaviour of the mechanic
        public SpawnRotationBehaviour spawnRotationBehaviour;
        
        public MovementBehaviour movementBehaviour;
        
        public float activationDelay = 0f;
    }
}