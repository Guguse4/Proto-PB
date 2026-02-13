using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool
{
    public enum SpawnPositionBehaviour
    {
        OnNearestPlayer,
        OnRandomPlayer,
        OnGivenObject,
        OnAllPlayers
    }

    public enum SpawnRotationBehaviour
    {
        ToNearestPlayer,
        ToRandomPlayer,
        ToAllPlayers
    }
    
    [CreateAssetMenu(menuName = "BossMechanics/Mechanic")]
    public class Mechanic: ScriptableObject
    {
        public List<Pattern> patterns;
        public SpawnPositionBehaviour spawnPositionBehaviour;
        
        // TODO: Useless ? Move ?
        public SpawnRotationBehaviour spawnRotationBehaviour;
    }
}