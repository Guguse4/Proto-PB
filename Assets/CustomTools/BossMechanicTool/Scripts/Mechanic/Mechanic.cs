using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool
{
    [CreateAssetMenu(menuName = "BossMechanics/Mechanic")]
    public class Mechanic: ScriptableObject
    {
        // List of patterns executed in the same time by the mechanic
        public List<Pattern> patterns;
        
        public float activationDelay = 0f;
    }
}