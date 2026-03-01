using UnityEngine;

namespace GameSettings
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettings: ScriptableObject
    {
        public LayerMask _playerLayerMask;
    }
}