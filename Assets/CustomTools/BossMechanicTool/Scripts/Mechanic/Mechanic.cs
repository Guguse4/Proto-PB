using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool
{
    /*
     * Data class to define a mechanic
     */
    [CreateAssetMenu(menuName = "BossMechanics/Mechanic")]
    public class Mechanic: ScriptableObject
    {
        // List of patterns executed in the same time by the mechanic
        public List<Pattern> patterns;
        // Delay between each pattern
        public float activationDelay = 0f;
    }
}